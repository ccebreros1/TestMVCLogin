﻿using System;
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
                //Create instance of user
                var user = User.Identity;
                ViewBag.Name = user.Name;

                //Check if the user is an admin
                if (isAdminUser())
                {
                    ViewBag.displayMenu = "Yes";
                }
                //The registration is not approved yet
                else if (isUnregistered())
                {
                    ViewBag.displayMenu = "NA";
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
                ViewBag.displayMenu = "You are not an user";
                return View();
            }
        }
        //GET: UnregisteredUsers
        public ActionResult unregisteredUsers()
        {
            using (var context = new ApplicationDbContext())
            {
                //Get the role ID for unregistered Users
                var role = (from r in context.Roles where r.Name.Contains("UnregisteredUser") select r).FirstOrDefault();
                //List all the unregistered Users
                var users = context.Users.Where(x => x.Roles.Select(y => y.RoleId).Contains(role.Id)).ToList();

                //Get the unregistered user info 
                var unregisteredUsers = users.Select(user => new UserGroupViewModel
                {
                    Username = user.UserName,
                    Email = user.Email,
                    RoleName = "UnregisteredUser"
                }).ToList();
                //Create a model
                var model = new GroupedUserViewModel { Users = unregisteredUsers };
                //Return the model
                return View(model);
            }
        }

        //Approve a user
        public ActionResult approveUser(string userName)
        {
            using (var context = new ApplicationDbContext())
            {
                //Get the role ID for unregistered Users
                var role = (from r in context.Roles where r.Name.Contains("UnregisteredUser") select r).FirstOrDefault();
                //List all the unregistered Users
                var user = context.Users.Where(x => x.UserName == userName).FirstOrDefault();

                var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                userManager.AddToRole(user.Id, "Member");
                userManager.RemoveFromRole(user.Id, "UnregisteredUser");
                context.SaveChanges();
                //Return the view
                return RedirectToAction("UnregisteredUsers", "Users");
            }
        }

        //Method to check if the user is on the Admin
        public Boolean isAdminUser()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                using (var context = new ApplicationDbContext())
                {
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
            }
            return false;
        }

        //Method to check if the user is not registered
        public Boolean isUnregistered()
        {
            if (User.Identity.IsAuthenticated)
            {
                var user = User.Identity;
                using (var context = new ApplicationDbContext())
                {
                    var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));
                    var s = UserManager.GetRoles(user.GetUserId());
                    if (s[0].ToString() == "UnregisteredUser")
                    {
                        return true;
                    }
                    else
                    {
                        return false;
                    }
                }
            }
            return false;
        }
    }
}