using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.NotificationsModule.Core.Model;

namespace Sharpcode.CartAbandonment.Data.Notifications
{
    public class CartAbandonmentEmailNotification : EmailNotification
    {
        public CartAbandonmentEmailNotification() : base(nameof(CartAbandonmentEmailNotification))
        {

        }

        public ShoppingCart ShoppingCart { get; set; }
    }
}
