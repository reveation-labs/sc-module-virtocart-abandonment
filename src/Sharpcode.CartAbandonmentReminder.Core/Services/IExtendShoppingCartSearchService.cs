using System.Threading.Tasks;
using VirtoCommerce.CartModule.Core.Model.Search;

namespace Sharpcode.CartAbandonmentReminder.Core.Services
{
    public interface IExtendShoppingCartSearchService
    {
        Task<ShoppingCartSearchResult> SearchCartReminderAsync(ShoppingCartSearchCriteria criteria);
    }
}
