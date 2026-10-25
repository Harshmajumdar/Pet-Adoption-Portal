using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace PetAdoptionPortal.Filters
{
    // Put [RequireLogin] on a controller to block it unless the admin is logged in.
    // Login status is remembered in the Session (state management).
    public class RequireLoginAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext context)
        {
            var adminName = context.HttpContext.Session.GetString("AdminName");
            if (string.IsNullOrEmpty(adminName))
            {
                context.Result = new RedirectToActionResult("Login", "Account", null);
            }
        }
    }
}
