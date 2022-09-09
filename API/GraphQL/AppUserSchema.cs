using GraphQL.Types;
using GraphQL;
namespace API.GraphQL
{
    public class AppUserSchema : Schema
    {
        public AppUserSchema(AppUserQuery appUserQuery)
        {
            Query = appUserQuery;
        }
    }
}
