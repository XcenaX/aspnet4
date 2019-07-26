using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using WebApplication2.Models;
using Microsoft.AspNet.Identity.EntityFramework;

namespace WebApplication2.Controllers
{
    public class AccountController : Controller
    {
        private AppUserManager UserManager => HttpContext.GetOwinContext().GetUserManager<AppUserManager>();
        private AppRolesManager RolesManager => HttpContext.GetOwinContext().GetUserManager<AppRolesManager>();
        private IAuthenticationManager AuthenticationManager => HttpContext.GetOwinContext().Authentication;

        [BithAuthorize(Age = 44)]
        public ActionResult Index()
        {
            var claims = User.Identity as ClaimsIdentity;
            ViewBag.ClaimValue = claims.FindFirstValue("BithYear");
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Login(LoginModel loginModel)
        {
            
            if (ModelState.IsValid)
            {
                var user = await UserManager.FindAsync(loginModel.Username, loginModel.Password);
                if (user == null)
                {
                    return new RedirectResult("/Account/Registration");
                }

                ClaimsIdentity claims = await UserManager.CreateIdentityAsync(user, DefaultAuthenticationTypes.ApplicationCookie);
                AuthenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = true,
                }, claims);

                return new RedirectResult("/Products/Index");
            }
            else
            {
                return View(loginModel);
            }
        }

        [HttpGet]
        public ActionResult Login()
        {
            if (!RolesManager.RoleExists("user"))
            {
                RolesManager.Create(new IdentityRole("user"));
            }
            
            return View();
        }

        [HttpGet]
        public ActionResult Registration()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Registration(RegistrationModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser { UserName = model.Username, Email = model.Email, BithYear = model.BirthYear };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    var claim = new Claim("BithYear", model.BirthYear.ToString());
                    UserManager.AddClaim(user.Id, claim);
                    UserManager.AddToRole(user.Id, "user");
                    return new RedirectResult("/Products/Index");
                }
                else
                {
                    return View(model);
                }

            }

            return View(model);
        }
    }
}