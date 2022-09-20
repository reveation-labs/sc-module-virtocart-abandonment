using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.NotificationsModule.Core.Model;

namespace VirtoCommerce.CartAbandonmentReminder.Data.Notifications
{
    public class CartReminderEmailNotification : EmailNotification
    {
        public CartReminderEmailNotification() : base(nameof(CartReminderEmailNotification))
        {

        }

        public ShoppingCart ShoppingCart { get; set; }
    }
}
