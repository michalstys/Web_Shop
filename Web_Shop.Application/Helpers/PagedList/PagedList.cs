using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sieve.Models;
using Sieve.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Web_Shop.Application.Helpers.PagedList
{
    public class PagedList<T, TOut> : BasePagedList<T, TOut>
    {

        private PagedList()
        {
        }

        public async static Task<PagedList<T, TOut>> CreateAsync(IQueryable<T> superset,
                                                                 ISieveProcessor sieveProcessor,
                                                                 IOptions<SieveOptions> sieveOptions,
                                                                 SieveModel model,
                                                                 Func<T, TOut> formatterCallback)
        {
            var pagedList = new PagedList<T, TOut>();

            if (model.Page == null || model.Page < 1)
                model.Page = 1;
            if (model.PageSize == null || model.PageSize < 1)
                model.PageSize = sieveOptions.Value.DefaultPageSize;

            if (model.PageSize > sieveOptions.Value.MaxPageSize)
                model.PageSize = sieveOptions.Value.MaxPageSize;

            var filteredAndSortedData = sieveProcessor.Apply(model, superset, applyPagination: false);
            var filteredSortedAndPaginatedData = sieveProcessor.Apply(model, superset);

            pagedList.TotalItemCount = filteredAndSortedData.Count();
            pagedList.PageSize = (int)model.PageSize;
            pagedList.PageNumber = (int)model.Page;

            pagedList.PageCount = (int)Math.Ceiling((double)pagedList.TotalItemCount / pagedList.PageSize);
            pagedList.HasPreviousPage = pagedList.PageNumber > 1;
            pagedList.HasNextPage = pagedList.PageNumber < pagedList.PageCount;
            pagedList.IsFirstPage = pagedList.PageNumber == 1;
            pagedList.IsLastPage = pagedList.PageNumber >= pagedList.PageCount;
            pagedList.FirstItemOnPage = (pagedList.PageNumber - 1) * pagedList.PageSize + 1;
            var numberOfLastItemOnPage = pagedList.FirstItemOnPage + pagedList.PageSize - 1;
            pagedList.LastItemOnPage = numberOfLastItemOnPage > pagedList.TotalItemCount
                ? pagedList.TotalItemCount
                : numberOfLastItemOnPage;

            pagedList.PageData = await filteredSortedAndPaginatedData.Select(x => formatterCallback(x)).ToListAsync();

            return pagedList;
        }
    }
}
