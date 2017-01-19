using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

#region Additional Namespaces
using TestMVCLogin.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
#endregion

namespace TestMVCLogin.Controllers
{
    public class UsersController : Controller
    {
        // GET: Users
        public ActionResult Index()
        {
            //Check if user is logged in
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ViewBag.Name = user.Name;

                //Check if the user is an admin
                if (isAdminUser())
                {
                    ViewBag.displayMenu = "Yes";
                }
                //User is not Admin, but is logged in
                else
                {
                    ViewBag.displayMenu = "No";
                }
                return View();
            }
            //User not logged in is goiung back to home page
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        //Method to check if the user is on the Admin
        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                ApplicationDbContext context = new ApplicationDbContext();
                var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                var s = UserManager.GetRoles(user.GetUserId());
                if (s[0].ToString() == "Admin")
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            return false;
        }
    }
}