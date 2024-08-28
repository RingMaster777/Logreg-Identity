using LogReg_Identity.Data;
using LogReg_Identity.Middlewares;
using LogReg_Identity.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Security.Claims;

namespace LogReg_Identity_Test
{
    public class PermissionMiddlewareTests
    {
        [Fact]
        public async Task InvokeAsync_UserHasPermission_AllowsRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.RolePermissions.Add(new RolePermissionModel
                {
                    RoleId = "1",
                    PermissionId = 1,
                    Role = new IdentityRole { Name = "Admin" },
                    Permission = new PermissionModel { PermissionId = 1, PermissionName = "GET" }
                });
                dbContext.SaveChanges();
            }

            var middleware = new PermissionMiddleware((innerHttpContext) => Task.CompletedTask);

            var context = new DefaultHttpContext();
            context.Request.Method = "GET";
            context.Request.Path = "/Home/Index";
            context.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.NameIdentifier, "user-id")
            }));

            var user = new ApplicationUser { Id = "user-id", UserName = "testuser" };

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string> { "Admin" });

            using (var dbContext = new ApplicationDbContext(options))
            {
                // Act
                await middleware.InvokeAsync(context, userManagerMock.Object, dbContext);

                // Assert
                Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
            }
        }




        [Fact]
        public async Task InvokeAsync_UserLacksPermission_DeniesRequest()
        {
            // Arrange
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDatabase")
                .Options;

            using (var dbContext = new ApplicationDbContext(options))
            {
                dbContext.RolePermissions.Add(new RolePermissionModel
                {
                    RoleId = "1",
                    PermissionId = 1,
                    Role = new IdentityRole { Name = "Admin" },
                    Permission = new PermissionModel { PermissionId = 1, PermissionName = "GET" }
                });
                dbContext.SaveChanges();
            }



            var middleware = new PermissionMiddleware((innerHttpContext) => Task.CompletedTask);

            var context = new DefaultHttpContext();
            context.Request.Method = "GET";
            context.Request.Path = "/Home/Index";
            context.User = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
        new Claim(ClaimTypes.NameIdentifier, "user-id")
            }));

            var user = new ApplicationUser { Id = "user-id", UserName = "testuser" };

            var userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);
            userManagerMock.Setup(um => um.GetUserAsync(It.IsAny<ClaimsPrincipal>())).ReturnsAsync(user);
            userManagerMock.Setup(um => um.GetRolesAsync(user)).ReturnsAsync(new List<string> { "User" });

            using (var dbContext = new ApplicationDbContext(options))
            {
                // Act
                await middleware.InvokeAsync(context, userManagerMock.Object, dbContext);

                // Assert
                Assert.Equal(StatusCodes.Status403Forbidden, context.Response.StatusCode);
            }
        }


    //    [Fact]
    //    public async Task InvokeAsync_UserIsNull_AllowsRequest()
    //    {
    //        // Arrange
    //        var middleware = new PermissionMiddleware((innerHttpContext) => Task.CompletedTask);

    //        var context = new DefaultHttpContext();
    //        context.Request.Method = "GET";
    //        context.Request.Path = "/Home/Index";
    //        context.User = null; // Simulate no user

    //        var userManagerMock = new Mock<UserManager<ApplicationUser>>(
    //            Mock.Of<IUserStore<ApplicationUser>>(), null, null, null, null, null, null, null, null);

    //        var rolePermissions = new List<RolePermission>
    //{
    //    new RolePermission { RoleId = "1", PermissionId = 1, Role = new IdentityRole { Name = "Admin" }, Permission = new Permission { PermissionId = 1, PermissionName = "GET" } }
    //}.AsQueryable();

            

    //        var mockRolePermissionsSet = new Mock<DbSet<RolePermission>>();
    //        mockRolePermissionsSet.As<IQueryable<RolePermission>>().Setup(m => m.Provider).Returns(rolePermissions.Provider);
    //        mockRolePermissionsSet.As<IQueryable<RolePermission>>().Setup(m => m.Expression).Returns(rolePermissions.Expression);
    //        mockRolePermissionsSet.As<IQueryable<RolePermission>>().Setup(m => m.ElementType).Returns(rolePermissions.ElementType);
    //        mockRolePermissionsSet.As<IQueryable<RolePermission>>().Setup(m => m.GetEnumerator()).Returns(rolePermissions.GetEnumerator());

    //        var dbContextMock = new Mock<ApplicationDbContext>();
    //        dbContextMock.Setup(db => db.RolePermissions).Returns(mockRolePermissionsSet.Object);

    //        // Act
    //        await middleware.InvokeAsync(context, userManagerMock.Object, dbContextMock.Object);

    //        // Assert
    //        Assert.Equal(StatusCodes.Status200OK, context.Response.StatusCode);
    //    }

    }


}
