using LogReg_Identity.Data;
using LogReg_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LogReg_Identity_Test
{
    public class UserRoleAssignmentTests
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;

        public UserRoleAssignmentTests()
        {
            // Set up the in-memory database and identity services
            var serviceCollection = new ServiceCollection();

            // Add logging services
            serviceCollection.AddLogging();

            // Add DbContext and Identity services
            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                options.UseInMemoryDatabase("testdatabase"))
                .AddIdentity<ApplicationUser, IdentityRole>(options =>
                {
                    // Configure Identity options if needed
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireDigit = false;
                    options.Password.RequireUppercase = false;
                    options.Password.RequireLowercase = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>()
                .AddDefaultTokenProviders();

            // Build the service provider
            var serviceProvider = serviceCollection.BuildServiceProvider();

            // Get services
            _context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            _userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            _roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        }

        [Fact]
        public async Task AssignRoleToUser_ShouldSucceed()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "testuser", Email = "testuser@example.com" };
            var createUserResult = await _userManager.CreateAsync(user, "Password123!");
            Assert.True(createUserResult.Succeeded, "User creation failed.");

            var role = new IdentityRole("Member");
            var createRoleResult = await _roleManager.CreateAsync(role);
            Assert.True(createRoleResult.Succeeded, "Role creation failed.");

            // Act
            var addRoleResult = await _userManager.AddToRoleAsync(user, "Member");

            // Assert
            Assert.True(addRoleResult.Succeeded, "Role assignment failed.");

            var roles = await _userManager.GetRolesAsync(user);
            Assert.Contains("Member", roles);
        }
    }
}
