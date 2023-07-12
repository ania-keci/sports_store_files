using Microsoft.AspNetCore.Identity;

namespace sports_store_files.Models {
    public class IdentitySeedData {
        private const string adminUser = "Admin";
        private const string adminPassword = "Secret123$";

        public static async void EnsurePopulated(IApplicationBuilder app) {
            using (var scope = app.ApplicationServices.CreateScope()) {
                var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();
                IdentityUser user = await userManager.FindByNameAsync(adminUser);
                if (user == null) {
                    user = new IdentityUser("Admin");
                    await userManager.CreateAsync(user, adminPassword);
                }
            }
        }
    }
}
