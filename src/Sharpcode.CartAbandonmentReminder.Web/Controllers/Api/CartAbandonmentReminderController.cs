using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Sharpcode.CartAbandonmentReminder.Core.Services;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Core.Model.Search;

namespace Sharpcode.CartAbandonmentReminder.Web.Controllers.Api
{
    [Route("api/cart-abandonment")]
    public class CartAbandonmentReminderController : Controller
    {
        private readonly IExtendShoppingCartSearchService _cartSearchService;

        public CartAbandonmentReminderController(IExtendShoppingCartSearchService cartSearchService)
        {
            _cartSearchService = cartSearchService;
        }
        // Post: api/cart-abandonment
        /// <summary>
        /// Get carts
        /// </summary>
        /// <remarks>Return ShoppingCartSearchResult</remarks>
        [HttpPost]
        [Route("search")]
        public async Task<ActionResult<ShoppingCartSearchResult>> GetCart([FromBody] ShoppingCartSearchCriteria shoppingCartSearchCriteria)
        {
            var response = CartResponseGroup.Full;
            shoppingCartSearchCriteria.ResponseGroup = response.ToString();
            var shoppingCarts = await _cartSearchService.SearchCartReminderAsync(shoppingCartSearchCriteria);

            return shoppingCarts;
        }
    }
}
