using LogReg_Identity.Areas.Identity.Data;
using LogReg_Identity.Data;
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
            // Set up the in-memory database and Identity services
            //var serviceProvider = new ServiceCollection()
            //    .AddDbContext<ApplicationDbContext>(options =>
            //        options.UseInMemoryDatabase("TestDatabase"))
            //    .AddIdentity<ApplicationUser, IdentityRole>()
            //    .AddEntityFrameworkStores<ApplicationDbContext>()
            //    .AddDefaultTokenProviders()
            //    .BuildServiceProvider();

            //_context = serviceProvider.GetRequiredService<ApplicationDbContext>();
            //_userManager = serviceProvider.GetRequiredService<UserManager<ApplicationUser>>();
            //_roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        }

        [Fact]
        public async Task AssignRoleToUser_ShouldSucceed()
        {
            // Arrange
            var user = new ApplicationUser { UserName = "testuser", Email = "testuser@example.com" };
            await _userManager.CreateAsync(user, "Password123!");

            var role = new IdentityRole("UserRole");
            await _roleManager.CreateAsync(role);

            // Act
            var result = await _userManager.AddToRoleAsync(user, "UserRole");

            // Assert
            Assert.True(result.Succeeded, "Role assignment failed.");
            var roles = await _userManager.GetRolesAsync(user);
            //Assert.Contains("UserRole", roles, "The user does not have the assigned role.");
        }
    }
}
