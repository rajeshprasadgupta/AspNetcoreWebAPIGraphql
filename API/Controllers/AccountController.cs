using System;
using System.Security.Cryptography;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using API.Data;
using API.Entities;
using Microsoft.EntityFrameworkCore;
using System.Text;
using API.DTOs;
using API.Interfaces;

namespace API.Controllers
{
    public class AccountController : BaseApiController
    {
        public DataContext DataContext { get; }
        private readonly ITokenService _tokenService;
        public AccountController(DataContext dataContext, ITokenService tokenService)
        {
            _tokenService = tokenService;
            DataContext = dataContext;
        } 

     
        private async Task<bool> UserExists(string username)
        {
            return await DataContext.Users.AnyAsync( i => i.UserName == username.ToLower());
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto register)
        {
            if(await UserExists(register.username)) return BadRequest("Username is taken");
            using var hmac = new HMACSHA512();
            var user = new AppUser{
                UserName = register.username.ToLower(),
                PasswordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(register.password)),
                PasswordSalt = hmac.Key
            };
            DataContext.Users.Add(user);
            await DataContext.SaveChangesAsync();
            return new UserDto{
                username = user.UserName,
                token = _tokenService.CreateToken(user)
            };
        }  

        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto) {
            var user = await DataContext.Users.SingleOrDefaultAsync( x => x.UserName == loginDto.username);
            if (user == null) return Unauthorized("User not found");
            using var hmac = new HMACSHA512(user.PasswordSalt);
            var computedPassword = hmac.ComputeHash(Encoding.UTF8.GetBytes(loginDto.password));
            for(int i = 0; i < computedPassword.Length; i++)
            {
                if (computedPassword[i] != user.PasswordHash[i])
                    return Unauthorized("password did not match");
            }
            return new UserDto{
                username = user.UserName,
                token = _tokenService.CreateToken(user)
            };
        }
    }
}