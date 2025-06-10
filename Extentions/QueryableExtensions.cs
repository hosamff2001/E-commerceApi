using Microsoft.EntityFrameworkCore;

namespace EcommerceApi.Extentions
{
    public static class QueryableExtensions
    {
        public static  IQueryable<T> Paginate<T>(this IQueryable<T> query, int page, int pageSize)
        {
            return  query.Skip((page - 1) * pageSize).Take(pageSize);
        }
    }
}
