using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;
using TheatreCMS.Models;

[assembly: OwinStartupAttribute(typeof(TheatreCMS.Startup))]
namespace TheatreCMS
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            createRolesandUsers();
        }

        //create method for default roles and Admin users for login
        private void createRolesandUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var userManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            //create Admin roll on strartup if none exist
            if (!roleManager.RoleExists("Admin"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Admin";
                roleManager.Create(role);

                var user = new ApplicationUser();
                user.UserName = "test";
                user.FirstName = "Queen";
                user.LastName = "LaSneefa";
                user.Email = "test@gmail.com";
                user.StreetAddress = "420 Beefsteak Ln";
                user.City = "Atlantis";
                user.State = "OR";
                user.ZipCode = "97212";

                string userPWD = "Passw0rd!";

                var chkUser = userManager.Create(user, userPWD);
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "Admin");
                }
            }

            //Creating Member role
            if (!roleManager.RoleExists("Member"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Member";
                roleManager.Create(role);

                var user = new ApplicationUser()
                {
                    UserName = "memberTest",
                    FirstName = "God",
                    LastName = "Skrilla",
                    Email = "member.test@gmail.com",
                    StreetAddress = "314 Pi Ct",
                    City = "Detroit",
                    //State could be Enum later
                    State = "FL",
                    ZipCode = "12345"
                };

                string userPWD = "Ih@ve12cats";

                var chkUser = userManager.Create(user, userPWD);
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "Member");
                }
            }

            //Creating Subscriber role
            if (!roleManager.RoleExists("Subscriber"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Subscriber";
                roleManager.Create(role);

                var user = new ApplicationUser()
                {
                    UserName = "subscriberTest",
                    FirstName = "Pamela",
                    LastName = "Flanderson",
                    Email = "subscriber.test@gmail.com",
                    StreetAddress = "100 100th St",
                    City = "Shire",
                    State = "MA",
                    ZipCode = "54321"
                };

                string userPWD = "100100St!";

                var chkUser = userManager.Create(user, userPWD);
                if (chkUser.Succeeded)
                {
                    var result1 = userManager.AddToRole(user.Id, "Subscriber");
                }
            }
        }
    }
}
