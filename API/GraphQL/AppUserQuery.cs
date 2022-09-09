using API.GraphQL.Types;
using GraphQL.Types;
using API.Interfaces;
using API.Data;

namespace API.GraphQL
{
    public class AppUserQuery : ObjectGraphType
    {
        public AppUserQuery(IUserRepository userRepository)
        {
            Field<ListGraphType<AppUserType>>("AppUser")
                .ResolveAsync(async context => await userRepository.GetAll());
        }
    }
}
