using System.Threading.Tasks;
using homework_59_aruuke_maratova.Models;
using Microsoft.AspNetCore.Identity;

namespace homework_59_aruuke_maratova.Utils
{
    public class AdminInitializer
    {
        public static async Task SeedAdminUser(RoleManager<IdentityRole> _roleManager, UserManager<User> _userManager)
        {
            string adminEmail = "admin@admin.com";
            string adminPassword = "password";
            var roles = new[] { "admin", "user" };

            foreach (var role in roles)
            {
                if (await _roleManager.FindByNameAsync(role) is null)
                    await _roleManager.CreateAsync(new IdentityRole(role));
            }

            if (await _userManager.FindByNameAsync(adminEmail) == null)
            {
                User admin = new User
                {
                    Email = adminEmail,
                    UserName = adminEmail
                };
                IdentityResult result = await _userManager.CreateAsync(admin, adminPassword);
                if (result.Succeeded)
                    await _userManager.AddToRoleAsync(admin, "admin");
            }
        }
    }
}