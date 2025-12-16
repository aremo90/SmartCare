using LinkO.Domin.Contract;
using LinkO.Domin.Models.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LinkO.Persistence.DataSeed
{
    public class IdentityDataIni : IDataInitilizer
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _role;
        private readonly ILogger<IdentityDataIni> _logger;

        public IdentityDataIni(UserManager<ApplicationUser> userManager , RoleManager<IdentityRole> role , ILogger<IdentityDataIni> logger)
        {
            _userManager = userManager;
            _role = role;
            _logger = logger;
        }

        public async Task InitilizeAsync()
        {
            try
            {
                if (!_role.Roles.Any())
                {
                    await _role.CreateAsync(new IdentityRole("Admin"));
                    await _role.CreateAsync(new IdentityRole("User"));
                }
                if (!_userManager.Users.Any())
                {
                    var user01 = new ApplicationUser
                    {
                        FirstName = "LinkO",
                        LastName = "Admin",
                        UserName = "linkoadmin",
                        Email = "LinkOAdmin@example.com",
                        Gender = Domin.Models.Enum.Gender.Male,
                        DateOfBirth = new DateOnly(1990, 1, 1),
                        PhoneNumber = "01120976601"

                    };
                    await _userManager.CreateAsync(user01, "Aa@123456");

                    await _userManager.AddToRoleAsync(user01, "Admin");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"An error occurred while seeding identity data {ex.Message}.");
            }
        }
    }
}
