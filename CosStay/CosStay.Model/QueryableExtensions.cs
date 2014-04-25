using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.Entity;
using System.Linq.Expressions;

namespace CosStay.Model
{
    public static class CosStayQueryableExtensions
    {
        public async static Task<List<T>> ToAsyncList<T>(this IQueryable<T> queryable)
        {
            return await queryable.ToListAsync();
        }

        public static IQueryable<T> IncludePaths<T, TProperty>(this IQueryable<T> queryable, params Expression<Func<T, TProperty>>[] paths)
        {
            foreach (var path in paths)
                queryable = queryable.Include(path);
            return queryable;
        }


    }
}
