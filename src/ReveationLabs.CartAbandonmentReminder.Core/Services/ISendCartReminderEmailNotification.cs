using System.Collections.Generic;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.StoreModule.Core.Model;

namespace ReveationLabs.CartAbandonmentReminder.Core.Services
{
    public interface ISendCartReminderEmailNotification
    {
        Task TryToSendCartReminderAsync(List<ShoppingCart> shoppingCarts,Store store);
    }
}
