using Microsoft.Owin;
using Owin;

#region Additional Namespaces
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using TestMVCLogin.Models;
#endregion

[assembly: OwinStartupAttribute(typeof(TestMVCLogin.Startup))]
namespace TestMVCLogin
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        // In this method we will create default User roles and Admin user for login
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Admin"))
            {

                // first we create Admin rool   
                var role = new IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                //Here we create a web master who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "WebMaster";
                user.Email = "WebMaster@ectv.ca";

                string userPWD = "Pa$$word1";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Admin");

                }
            }

            #region Create Startup Roles
            // creating Creating Employee role    
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);
            }

            // creating Creating Member role    
            if (!roleManager.RoleExists("Member"))
            {
                var role = new IdentityRole();
                role.Name = "Member";
                roleManager.Create(role);
            }

            // creating Creating Volunteer role  
            if (!roleManager.RoleExists("Volunteer"))
            {
                var role = new IdentityRole();
                role.Name = "Volunteer";
                roleManager.Create(role);
            }
            #endregion
        }
    }
}
