using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
        public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if (!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Name",
                    Email = "email@gmail.com",
                    UserName = "email@gmail.com",
                    Address = new Address
                    {
                        FirstName = "Name",
                        LastName = "Surname",
                        Street = "Street",
                        City = "City",
                        State = "State",
                        ZipCode = "12345"
                    }
                };

                await userManager.CreateAsync(user, "P@$$w0rd");
            }
        }
    }
}