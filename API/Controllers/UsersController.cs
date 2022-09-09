using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using API.Interfaces;
using API.DTOs;
using AutoMapper;
using System.Security.Claims;
using API.Helpers;
using API.Extensions;
namespace API.Controllers
{
    
    //[Authorize]
    public class UsersController : BaseApiController
    {
        private readonly IMapper _mapper;
        private readonly  IPhotoService _photoService;
        private readonly ILogger<UsersController> _logger;       
        private readonly IUserRepository Repository;
        public UsersController(IUserRepository repository, IMapper mapper, IPhotoService photoService, ILogger<UsersController> logger)
        {
            _logger = logger;
            _photoService = photoService;
            _mapper = mapper;
            Repository = repository;
        }  

        
        [HttpGet]
        //supports pagination
        public async Task<ActionResult<IEnumerable<MemberDto>>> GetUsers([FromQuery]UserParams userParams)
        {
            var users = await Repository.GetMembersAsyc(userParams);
            Response.AddPaginationHeader(users.CurrentPage, users.PageSize,users.TotalCount, users.TotalPages);
            return Ok(users);
        }

       
        [HttpGet("{username}", Name ="GetUser")]
        public async Task<ActionResult<MemberDto>> GetUser(string username)
        {
            return await Repository.GetMemberAsync(username);
        }

        [HttpPut]
        public async Task<ActionResult> UpdateUser(MemberUpdateDto memberUpdateDto){
            var username = User.GetUserName() ;
            var user = await Repository.GetUserAsync(username);
            _mapper.Map(memberUpdateDto,user);
            Repository.Update(user);
            if(await Repository.SaveAllAsync()) return NoContent();
            return BadRequest("Failed to update user");
        }

        [HttpPost]
        public async Task<ActionResult<PhotoDto>> AddPhoto(IFormFile file)
        {
            var username = User.GetUserName();
            var user = await Repository.GetUserAsync(username);
            var result = await  _photoService.AddPhotoAsync(file);
            if(result.Error !=null) return BadRequest(result.Error.Message);
            var photo = new Photo{
                Url = result.SecureUrl.AbsoluteUri,
                PublicId = result.PublicId
            };
            if(user.Photos.Count == 0)
            photo.IsMain = true;
            if(await Repository.SaveAllAsync()){
                return CreatedAtRoute("GetUser", new {username = user.UserName}, _mapper.Map<PhotoDto>(photo));
            }
            return BadRequest("Error Adding Photo");
        }

        [HttpDelete]
        public async Task<ActionResult> DeletePhoto(string publicId){
            var username = User.GetUserName();
            var user = await Repository.GetUserAsync(username);
            var photo = user.Photos.FirstOrDefault(x => x.PublicId == publicId);
            if(photo == null) return NotFound();
            if(photo.IsMain) return BadRequest("Main photo cannot be deleted");
            if(photo.PublicId != null)
            {
                var result = await _photoService.DeleteAsync(publicId);
                if(result.Error != null){
                    _logger.LogError(result.Error.Message);
                    return BadRequest("Failed to Remove photo");
                }
            }
            user.Photos.Remove(photo);
            if(await Repository.SaveAllAsync()) return Ok();
            return BadRequest("Failed to Remove photo");        
        }
    }
}