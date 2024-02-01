using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.NotificationsModule.Core.Model;

namespace Sharpcode.CartAbandonment.Data.Notifications
{
    public class CartReminderEmailNotification : EmailNotification
    {
        public CartReminderEmailNotification() : base(nameof(CartReminderEmailNotification))
        {

        }

        public ShoppingCart ShoppingCart { get; set; }
    }
}
