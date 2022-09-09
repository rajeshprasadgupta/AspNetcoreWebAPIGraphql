using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Interfaces;
using Microsoft.EntityFrameworkCore;
using AutoMapper.QueryableExtensions;
using AutoMapper;
using API.Helpers;

namespace API.Data
{
    public class UserRepository : IUserRepository
    {
        private readonly DataContext _context;
        private readonly IMapper _mapper;

        public UserRepository(DataContext context, IMapper mapper)
        {
            _mapper = mapper;
            _context = context;
        }
        public async Task<MemberDto> GetMemberAsync(string name)
        {
           var user = await _context.Users.Where(x => x.UserName == name)
            .ProjectTo<MemberDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
           return user;
        }

        public async Task<AppUser> GetUserAsync(string name)
        {
           var user = await _context.Users.Where(x => x.UserName == name)
            .SingleOrDefaultAsync();
             return user;
        }

        public async Task<MemberDto> GetMemberAsync(int id)
        {
            var user = await _context.Users.Where(x => x.Id == id)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
           return user;
        }

        public async Task<PagedList<MemberDto>> GetMembersAsyc(UserParams userParams)
        {
            var query = _context.Users.AsQueryable();
            //add any filters to query if exists in userParams prior to applying dto conversion
            return await PagedList<MemberDto>.CreateAsync(query.ProjectTo<MemberDto>(_mapper.ConfigurationProvider).AsNoTracking(), 
                userParams.PageNumber, userParams.PageSize);
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _context.SaveChangesAsync() > 0;
        }

        public void Update(AppUser user)
        {
            _context.Entry(user).State = EntityState.Modified;
        }

        public async Task<MemberDto> GetMemberAsyc(string name)
        {
             var user = await _context.Users.Where(x => x.UserName == name)
                .ProjectTo<MemberDto>(_mapper.ConfigurationProvider).SingleOrDefaultAsync();
           return user;
        }

        public async Task<IEnumerable<AppUser>> GetAll()
        {
            return await _context.Users.ToListAsync();
        }
    }
}