using Microsoft.AspNetCore.Mvc;
using UserContext;

namespace DeliverITUser.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var aUser = Models.User.GetUserDetailsFromCookie(HttpContext);
            if (aUser == null) Models.User.SetAnonymousCookie(HttpContext);
            return View(aUser);
        }

        public IActionResult Login()
        {
            var aUser = Models.User.GetUserDetailsFromCookie(HttpContext);
            return View(aUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login([Bind("UserName")] ClientToken myToken)
        {
            Models.User.LogUserIn(HttpContext, myToken.UserName);
            return RedirectToAction("Index");
        }

        public IActionResult Logout()
        {
            var aUser = Models.User.GetUserDetailsFromCookie(HttpContext);
            return View(aUser);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult LogoutUser()
        {
            Models.User.Logout(HttpContext);
            return RedirectToAction("Index");
        }
        public IActionResult Error()
        {
            return View();
        }
    }
}
