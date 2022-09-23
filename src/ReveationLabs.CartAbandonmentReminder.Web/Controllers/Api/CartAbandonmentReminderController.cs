using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReveationLabs.CartAbandonmentReminder.Core;

namespace ReveationLabs.CartAbandonmentReminder.Web.Controllers.Api
{
    [Route("api/cart-abandonment-reminder")]
    public class CartAbandonmentReminderController : Controller
    {
        // GET: api/cart-abandonment-reminder
        /// <summary>
        /// Get message
        /// </summary>
        /// <remarks>Return "Hello world!" message</remarks>
        [HttpGet]
        [Route("")]
        [Authorize(ModuleConstants.Security.Permissions.Read)]
        public ActionResult<string> Get()
        {
            return Ok(new { result = "Hello world!" });
        }
    }
}
