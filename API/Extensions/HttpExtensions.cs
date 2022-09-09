using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using API.Helpers;
using System.Text.Json;
namespace API.Extensions
{
    public static class HttpExtensions
    {
        //Adds Pagination information to Response header
        public static void AddPaginationHeader(this HttpResponse response, int currentPage, 
        int itemsPerPage, int totalItems, int totalPages)
        {
            var pageHeader = new PaginationHeader(currentPage, itemsPerPage, totalItems, totalPages);
            //camel case serialization
            var options = new JsonSerializerOptions{
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
            //default serialization is Type case, so provide camecase option to the serializer
            response.Headers.Add("Pagination", JsonSerializer.Serialize(pageHeader, options));
            //add a CORS header Access-Control-Expose-Headers to make the Pagination header available
            response.Headers.Add("Access-Control-Expose-Headers", "Pagination");
        }
    }
}