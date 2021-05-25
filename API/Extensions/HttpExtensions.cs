using System.Text.Json;
using API.Helpers.HttpHeaders;
using Microsoft.AspNetCore.Http;

namespace API.Extensions
{
    public static class HttpExtensions
    {
        public static void AddPaginationHeader(this HttpResponse httpResponse, int pageNumber, int pageSize, int totalCount, int totalPages)
        {
            const string paginationHeaderName = "Pagination";

            var paginationHeader = new PaginationHeader(pageNumber, pageSize, totalCount, totalPages);
            
            httpResponse.Headers.Add(paginationHeaderName, JsonSerializer.Serialize(paginationHeader));
            httpResponse.Headers.Add("Access-Control-Expose-Headers", paginationHeaderName);
        }
    }
}
