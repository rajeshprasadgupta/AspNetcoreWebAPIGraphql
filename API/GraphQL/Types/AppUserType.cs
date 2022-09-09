using API.Entities;
using GraphQL.Types;
namespace API.GraphQL.Types
{
    public class AppUserType : ObjectGraphType<AppUser>
    {
        public AppUserType()
        {
            Field(t => t.Id);
            Field(t => t.UserName);
            Field(t => t.Introduction);
            Field(t => t.KnownAs);
            Field(t => t.LookingFor);
            Field(t => t.LastActive);
            Field(t => t.Created);
            Field(t => t.City);
            Field(t => t.Country);
        }
    }
}
