using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReveationLabs.CartAbandonmentReminder.Core;
using ReveationLabs.CartAbandonmentReminder.Core.Services;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Core.Model.Search;
using VirtoCommerce.Platform.Core.GenericCrud;

namespace ReveationLabs.CartAbandonmentReminder.Web.Controllers.Api
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
            var dateTime = DateTime.Now;
            var startDateTime = dateTime.AddDays(-3000);
            var endDateTime = dateTime.AddDays(0);
            shoppingCartSearchCriteria.ResponseGroup = response.ToString();
            shoppingCartSearchCriteria.CreatedStartDate = startDateTime;
            shoppingCartSearchCriteria.CreatedEndDate = endDateTime;
            var shoppingCarts = await _cartSearchService.SearchCartAsync(shoppingCartSearchCriteria);

            return shoppingCarts;
        }
    }
}
