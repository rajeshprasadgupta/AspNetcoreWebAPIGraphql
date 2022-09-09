using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.DTOs;
using API.Entities;
using API.Helpers;
namespace API.Interfaces
{
    public interface IUserRepository
    {
        void Update(AppUser user);
        
        Task<bool> SaveAllAsync();

        Task<PagedList<MemberDto>> GetMembersAsyc(UserParams userParams);

        Task<MemberDto> GetMemberAsync(string name);

        Task<AppUser> GetUserAsync(string name);

        Task<MemberDto> GetMemberAsync(int id);

        Task<IEnumerable<AppUser>> GetAll();

    }
}