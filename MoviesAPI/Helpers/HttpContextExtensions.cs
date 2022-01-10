using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Helpers
{
    public static class HttpContextExtensions
    {
        public async static Task InsertParameterPagination<T>(this HttpContext httpContext, IQueryable<T> queryable, int numberOfRecordsPerPage)
        {
            double quantity = await queryable.CountAsync();
            double pageQuantity = Math.Ceiling(quantity / numberOfRecordsPerPage);
            httpContext.Response.Headers.Add("NumberOfPages", pageQuantity.ToString());
        }
    }
}
