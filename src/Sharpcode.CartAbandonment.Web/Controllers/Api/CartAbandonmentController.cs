using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Sharpcode.CartAbandonment.Core;
using Sharpcode.CartAbandonment.Core.Services;
using VirtoCommerce.CartModule.Core.Model;
using VirtoCommerce.CartModule.Core.Model.Search;

namespace Sharpcode.CartAbandonment.Web.Controllers.Api
{
    [Route("api/cart-abandonment")]
    public class CartAbandonmentController : Controller
    {
        private readonly IExtendShoppingCartSearchService _cartSearchService;

        public CartAbandonmentController(IExtendShoppingCartSearchService cartSearchService)
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
        [Authorize(ModuleConstants.Security.Permissions.Access)]
        public async Task<ActionResult<ShoppingCartSearchResult>> GetCart([FromBody] ShoppingCartSearchCriteria shoppingCartSearchCriteria)
        {
            var response = CartResponseGroup.Full;
            shoppingCartSearchCriteria.ResponseGroup = response.ToString();
            var shoppingCarts = await _cartSearchService.SearchCartReminderAsync(shoppingCartSearchCriteria);

            return shoppingCarts;
        }
    }
}
