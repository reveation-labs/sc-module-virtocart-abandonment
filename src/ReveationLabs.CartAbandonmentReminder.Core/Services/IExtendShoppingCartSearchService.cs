using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Core.Model.Search;

namespace ReveationLabs.CartAbandonmentReminder.Core.Services
{
    public interface IExtendShoppingCartSearchService
    {
        Task<ShoppingCartSearchResult> SearchCartAsync(ShoppingCartSearchCriteria criteria);
    }
}
