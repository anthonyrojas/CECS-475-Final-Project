using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using MVCMusicStore.Models;

namespace MVCMusicStore.Controllers
{
    public class AccountController : Controller
    {
        AccountEntities accountDB = new AccountEntities();
        private void MigrateShoppingCart(string UserName)
        {
            // Associate shopping cart items with logged-in user
            var cart = ShoppingCart.GetCart(this.HttpContext);

            cart.MigrateCart(UserName);
            Session[ShoppingCart.CartSessionKey] = UserName;
        }

        public ActionResult Index()
        {
            using (accountDB)
            {
                return View(accountDB.UserAccounts.ToList());
            }
        }

        //
        // GET: /Account/Register

        public ActionResult Register()
        {
            return View();
        }

        //
        // POST: /Account/Register

        [HttpPost]
        public ActionResult Register(UserAccount model)
        {
            if (ModelState.IsValid)
            {
                using (accountDB)
                {
                    accountDB.UserAccounts.Add(model);
                    accountDB.SaveChanges();
                }
                ModelState.Clear();
                ViewBag.Message = String.Format("{0} {1} successfully registered!", 
                    model.FirstName, model.LastName );
            }

            // If we got this far, something failed, redisplay form
            return View();
        }

        //Login
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(UserAccount model)
        {
            using (accountDB)
            {
                try
                {
                    var user = accountDB.UserAccounts.Single(u => u.Username == model.Username
                        && u.Password == model.Password);
                    if (user != null)
                    {
                        Session["UserId"] = user.UserId.ToString();
                        Session["Username"] = user.Username.ToString();
                        return RedirectToAction("LoggedIn");
                    }
                    else
                    {
                        ModelState.AddModelError("", "Username or password is wrong");
                    }
                }
                catch
                {
                    ModelState.AddModelError("", "Username or password is wrong");
                }
            }
            return View();
        }

        public ActionResult LoggedIn()
        {
            if (Session["UserId"] != null)
            {
                return View();
            }
            else
            {
                return View("SessionError");
            }
        }

        #region Status Codes
        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion
    }
}
