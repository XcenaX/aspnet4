using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace WebApplication2.Models
{
    public class BithAuthorize : AuthorizeAttribute
    {
        public int Age { get; set; }
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                return false;
            }

            var claims = httpContext.User.Identity as ClaimsIdentity;
            var yearStr = claims.FindFirstValue("BithYear");

            var currYear = DateTime.UtcNow.Year;

            int userYear;
            if (!int.TryParse(yearStr, out userYear))
            {
                return false;
            }

            if (currYear - userYear < Age)
            {
                return false;
            }
 
            return base.AuthorizeCore(httpContext);
        }
    }
}