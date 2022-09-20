using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Core.Model.Search;

namespace VirtoCommerce.CartAbandonmentReminder.Core.Services
{
    public interface ISendCartReminderEmailNotification
    {
        Task TryToSendCartReminderAsync(List<ShoppingCart> shoppingCarts);
    }
}
