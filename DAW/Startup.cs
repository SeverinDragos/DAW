﻿using DAW.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(DAW.Startup))]
namespace DAW
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
            // Se apeleaza o metoda in care se adauga contul de administrator si rolurile aplicatiei              
            CreateAdminUserAndApplicationRoles();
        }

        private void CreateAdminUserAndApplicationRoles()
        {
            ApplicationDbContext context = new ApplicationDbContext();

            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(context));

            var UserManager = new UserManager<ApplicationUser>(new UserStore<ApplicationUser>(context));

            // Se adauga rolurile aplicatiei             
            if (!roleManager.RoleExists("Administrator"))
            {
                // Se adauga rolul de administrator                 
                var role = new IdentityRole();
                role.Name = "Administrator";
                roleManager.Create(role);
                // se adauga utilizatorul administrator                 
                var user = new ApplicationUser();
                user.UserName = "admin@admin.com";
                user.Email = "admin@admin.com";
                var adminCreated = UserManager.Create(user, "Administrator1!");
                if (adminCreated.Succeeded)
                {
                    UserManager.AddToRole(user.Id, "Administrator");
                }
            }

            if (!roleManager.RoleExists("ProjectManager"))
            {
                var role = new IdentityRole();
                role.Name = "ProjectManager";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("User"))
            {
                var role = new IdentityRole();
                role.Name = "User";
                roleManager.Create(role);
            }

            if (!roleManager.RoleExists("TeamMember"))
            {
                var role = new IdentityRole();
                role.Name = "TeamMember";
                roleManager.Create(role);
            }
        }
    }
}
