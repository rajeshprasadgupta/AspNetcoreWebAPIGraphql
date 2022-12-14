using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Claims;
namespace API.Extensions
{
    public static class ClaimsPrincipalExtentions
    {
        public static string GetUserName(this ClaimsPrincipal user)
        {
            return user.FindFirst(ClaimTypes.Name)?.Value;
        }

        public static int GetUserid(this ClaimsPrincipal user)
        {
            return int.Parse( user.FindFirst(ClaimTypes.NameIdentifier)?.Value);
        }
    }
}