using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Owin;
using Mixr.Models;
using Owin;

[assembly: OwinStartupAttribute(typeof(Mixr.Startup))]
namespace Mixr
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            CreateRolesAndUsers();
        }

        // Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddMvc();

            services.AddAuthorization(options =>
            {
                options.AddPolicy("RequireAdministratorRole", policy => policy.RequireRole("Administrator"));
            });
        }

        // In this method we will create default User roles and Admin user for login   
        private void CreateRolesAndUsers()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));
            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));


            // In Startup iam creating first Admin Role and creating a default Admin User    
            if (!roleManager.RoleExists("Administrator"))
            {

                // first we create Admin role 
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Administrator";
                roleManager.Create(role);


                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "angus";
                user.Email = "angus.miller@gmail.com";
                user.EmailConfirmed = true;

                string userPWD = "Budgie28";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result1 = UserManager.AddToRole(user.Id, "Administrator");

                }
            }


            // In Startup iam creating first User Role and creating a default User    
            if (!roleManager.RoleExists("User"))
            {

                // first we create Admin role 
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "User";
                roleManager.Create(role);


                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "miller";
                user.Email = "angus.miller12@gmail.com";
                user.EmailConfirmed = true;

                string userPWD = "Budgie28";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result = UserManager.AddToRole(user.Id, "User");

                }
            }


            // creating Creating Manager role    
            if (!roleManager.RoleExists("Manager"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Manager";
                roleManager.Create(role);


                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "kevin";
                user.Email = "angus.miller11@gmail.com";
                user.EmailConfirmed = true;

                string userPWD = "Budgie28";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result = UserManager.AddToRole(user.Id, "Manager");

                }

            }

            // creating Creating Employee role    
            if (!roleManager.RoleExists("Employee"))
            {
                var role = new Microsoft.AspNet.Identity.EntityFramework.IdentityRole();
                role.Name = "Employee";
                roleManager.Create(role);

                //Here we create a Admin super user who will maintain the website                  

                var user = new ApplicationUser();
                user.UserName = "peter";
                user.Email = "peter.miller11@gmail.com";
                user.EmailConfirmed = true;

                string userPWD = "Peter28";

                var chkUser = UserManager.Create(user, userPWD);

                //Add default User to Role Admin   
                if (chkUser.Succeeded)
                {
                    var result = UserManager.AddToRole(user.Id, "Employee");
                }

            }
        }
    }
}
