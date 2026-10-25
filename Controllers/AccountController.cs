using PetAdoptionPortal.Models;
using Microsoft.AspNetCore.Mvc;

namespace PetAdoptionPortal.Controllers
{
    public class AccountController : Controller
    {
        // Demo login (simple on purpose for a mini project)
        private const string AdminUser = "admin";
        private const string AdminPass = "admin123";

        [HttpGet]
        public IActionResult Login()
        {
            if (!string.IsNullOrEmpty(HttpContext.Session.GetString("AdminName")))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            if (model.Username == AdminUser && model.Password == AdminPass)
            {
                // Save to Session so other pages know we are logged in
                HttpContext.Session.SetString("AdminName", "Staff");
                return RedirectToAction("Index", "Home");
            }

            ModelState.AddModelError("", "Invalid username or password");
            return View(model);
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login");
        }
    }
}
