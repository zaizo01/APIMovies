using MoviesAPI.DTOs;
using System.Linq;

namespace MoviesAPI.Helpers
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginar<T>(this IQueryable<T> queryable, PaginacionDTO paginacionDTO)
        {
            return queryable
                .Skip((paginacionDTO.Page - 1) * paginacionDTO.NumberOfRecordsPerPage)
                .Take(paginacionDTO.NumberOfRecordsPerPage);
        }
    }
}
