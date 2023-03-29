using hidayah_collage.DataContext;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace hidayah_collage.Models.Roles
{
    public static class RolesData
    {
        //private static readonly string[] Roles = new string[] { "Administrator", "Editor", "Subscriber" };

        public static async Task SeedRoles(IServiceProvider serviceProvider)
        {
            //var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            //var userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();

            using (var serviceScope = serviceProvider.GetRequiredService<IServiceScopeFactory>().CreateScope())
            {
                var dbContext = serviceScope.ServiceProvider.GetService<AppDbContext>();
                var userManager = serviceScope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();
                var configuration = serviceScope.ServiceProvider.GetService<IConfiguration>();
                
                var Role = (await dbContext.System.AsNoTracking().Where(x => x.Type == "DEFAULT_ROLES").FirstOrDefaultAsync()).Value_Txt;
                string[] Roles = Role.Split(',').ToArray<string>();

                //if (dbContext.Database.GetPendingMigrations().Any())
                if (!dbContext.UserRoles.Any())
                {
                    //await dbContext.Database.MigrateAsync();
                    
                    var roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                    
                    /*
                    foreach (var role in configuration.GetSection("Roles").Get<List<string>>())
                    {
                        var nameRole = await roleManager.FindByNameAsync(role);
                        var result = await roleManager.DeleteAsync(nameRole);
                    }*/
                    //foreach (var role in configuration.GetSection("Roles").Get<List<string>>())
                    foreach (var role in Roles)
                    {
                        if (!await roleManager.RoleExistsAsync(role))
                        {
                            await roleManager.CreateAsync(new IdentityRole(role));
                        }
                    }                    
                }
                //Here you could create a super user who will maintain the web app
                var SuperUser = new ApplicationUser
                {
                    UserName = configuration["SuperAdmin:UserName"],
                    //UserName = configuration.GetValue<string>("SuperAdmin:UserName"),
                    Email = configuration.GetValue<string>("SuperAdmin:Email"),
                    //UserName = configuration.GetSection("SuperAdmin").GetSection("UserName").Value,
                    //Email = configuration.GetSection("SuperAdmin").GetSection("Email").Value,
                    FirstName = "Super Admin"
                };
                //string password = configuration["SuperAdmin:Password"];
                string password = configuration.GetSection("SuperAdmin").GetSection("Password").Value;
                var user = await userManager.FindByEmailAsync(SuperUser.Email);

                if (user == null)
                {
                    var createSuperUser = await userManager.CreateAsync(SuperUser, password);
                    if (createSuperUser.Succeeded)
                    {
                        await userManager.AddToRoleAsync(SuperUser, "SuperAdmin");
                    }
                }
            }
        }
    }
}
