using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_Shop.Application.Helpers.PagedList;

namespace Web_Shop.Application.Extensions
{
    public static class PagedListExtensions
    {
        public static async Task<IPagedList<T, TOut>> ToPagedListAsync<T, TOut>(this IQueryable<T> superset,
                                                                                ISieveProcessor sieveProcessor,
                                                                                IOptions<SieveOptions> sieveOptions,
                                                                                SieveModel model,
                                                                                Func<T, TOut> formatterCallback)
        {
            return await PagedList<T, TOut>.CreateAsync(superset, sieveProcessor, sieveOptions, model, formatterCallback);
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> superset, int numberOfPages)
        {
            return superset
                .Select((item, index) => new { index, item })
                .GroupBy(x => x.index % numberOfPages)
                .Select(x => x.Select(y => y.item));
        }

        public static IEnumerable<IEnumerable<T>> Partition<T>(this IEnumerable<T> superset, int pageSize)
        {
            if (superset.Count() < pageSize)
                yield return superset;
            else
            {
                var numberOfPages = Math.Ceiling(superset.Count() / (double)pageSize);
                for (var i = 0; i < numberOfPages; i++)
                    yield return superset.Skip(pageSize * i).Take(pageSize);
            }
        }
    }
}
