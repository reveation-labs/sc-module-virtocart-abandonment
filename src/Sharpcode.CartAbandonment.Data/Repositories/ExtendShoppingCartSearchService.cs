using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Sharpcode.CartAbandonment.Core.Services;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Core.Model.Search;
using VirtoCommerce.CartModule.Core.Services;
using VirtoCommerce.CartModule.Data.Model;
using VirtoCommerce.CartModule.Data.Repositories;
using VirtoCommerce.Platform.Core.Caching;
using VirtoCommerce.Platform.Core.Common;
using VirtoCommerce.Platform.Core.GenericCrud;
using VirtoCommerce.Platform.Data.GenericCrud;

namespace Sharpcode.CartAbandonment.Data.Repositories
{
    public class ExtendShoppingCartSearchService : SearchService<ShoppingCartSearchCriteria, ShoppingCartSearchResult, ShoppingCart, ShoppingCartEntity>, IExtendShoppingCartSearchService
    {
        public ExtendShoppingCartSearchService(Func<ICartRepository> repositoryFactory, IPlatformMemoryCache platformMemoryCache, IShoppingCartService cartService, IOptions<CrudOptions> crudOptions) :
    base(repositoryFactory, platformMemoryCache, cartService, crudOptions)
        {
        }

        public async Task<ShoppingCartSearchResult> SearchCartReminderAsync(ShoppingCartSearchCriteria criteria)
        {
            return await SearchAsync(criteria);
        }

        protected override IQueryable<ShoppingCartEntity> BuildQuery(IRepository repository, ShoppingCartSearchCriteria criteria)
        {

            var query = ((ICartRepository)repository).ShoppingCarts.Where(x => !x.IsDeleted);

            if (!string.IsNullOrEmpty(criteria.Status))
            {
                query = query.Where(x => x.Status == criteria.Status);
            }

            if (!string.IsNullOrEmpty(criteria.Name))
            {
                query = query.Where(x => x.Name == criteria.Name);
            }

            if (!string.IsNullOrEmpty(criteria.CustomerId))
            {
                query = query.Where(x => x.CustomerId == criteria.CustomerId);
            }

            if (!string.IsNullOrEmpty(criteria.StoreId))
            {
                query = query.Where(x => criteria.StoreId == x.StoreId);
            }

            if (!string.IsNullOrEmpty(criteria.Currency))
            {
                query = query.Where(x => x.Currency == criteria.Currency);
            }

            if (!string.IsNullOrEmpty(criteria.Type))
            {
                query = query.Where(x => x.Type == criteria.Type);
            }

            if (!string.IsNullOrEmpty(criteria.OrganizationId))
            {
                query = query.Where(x => x.OrganizationId == criteria.OrganizationId);
            }

            if (!criteria.CustomerIds.IsNullOrEmpty())
            {
                query = query.Where(x => criteria.CustomerIds.Contains(x.CustomerId));
            }

            if (criteria.CreatedStartDate != null)
            {
                query = query.Where(x => x.CreatedDate >= criteria.CreatedStartDate.Value);
            }

            if (criteria.CreatedEndDate != null)
            {
                query = query.Where(x => x.CreatedDate <= criteria.CreatedEndDate.Value);
            }

            if (criteria.ModifiedStartDate != null)
            {
                query = query.Where(x => x.ModifiedDate >= criteria.ModifiedStartDate.Value);
            }

            if (criteria.ModifiedEndDate != null)
            {
                query = query.Where(x => x.ModifiedDate <= criteria.ModifiedEndDate.Value);
            }
            query = query.Where(x => (x.Items.Count > 0) && ((x.IsAnonymous && x.Shipments.Count>0) || (!x.IsAnonymous)));

            return query;
        }

        protected override IList<SortInfo> BuildSortExpression(ShoppingCartSearchCriteria criteria)
        {
            var sortInfos = criteria.SortInfos;
            if (sortInfos.IsNullOrEmpty())
            {
                sortInfos = new[]
                {
                    new SortInfo
                    {
                        SortColumn = ReflectionUtility.GetPropertyName<ShoppingCartEntity>(x => x.CreatedDate),
                        SortDirection = SortDirection.Descending
                    }
                };
            }

            return sortInfos;
        }
    }
}
