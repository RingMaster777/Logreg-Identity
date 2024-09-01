using LogReg_Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Globalization;

namespace LogReg_Identity.Data;

public class ApplicationDbContext : IdentityDbContext<IdentityUser>
{
    public DbSet<ApplicationUser> ApplicationUsers { get; set; }
    public DbSet<PermissionModel> Permissions { get; set; }
    public DbSet<RolePermissionModel> RolePermissions { get; set; }

    public DbSet<NoteModel> Notes { get; set; }

    public DbSet<MenuModel> Menus { get; set; }

    public DbSet<MenuPermissionModel> MenuPermissions { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Seed Permissions
        var permissions = new List<PermissionModel>
        {
            new PermissionModel { PermissionId = 1, PermissionName = "POST" },
            new PermissionModel { PermissionId = 2, PermissionName = "GET" },
            new PermissionModel { PermissionId = 3, PermissionName = "PATCH" },
            new PermissionModel { PermissionId = 4, PermissionName = "DELETE" },
        };
        builder.Entity<PermissionModel>().HasData(permissions);



        ////Seed RolePermissions
        var rolePermissions = new List<RolePermissionModel>
        {
            new RolePermissionModel {RpId=1 , RoleId = "436d0dc3-12ea-4690-8164-219ff00789a4", PermissionId = 1 }, // Admin -> Create
            new RolePermissionModel {RpId=2 , RoleId = "436d0dc3-12ea-4690-8164-219ff00789a4", PermissionId = 2 }, // Admin -> Read
            new RolePermissionModel {RpId=3,  RoleId = "436d0dc3-12ea-4690-8164-219ff00789a4", PermissionId = 3 }, // Admin -> Update
            new RolePermissionModel {RpId=4,  RoleId = "436d0dc3-12ea-4690-8164-219ff00789a4", PermissionId = 4 }, // Admin -> Delete
            new RolePermissionModel {RpId=5,  RoleId = "681b047a-2255-4262-ae12-95a311c498b9", PermissionId = 2 }, // User -> Read
            new RolePermissionModel {RpId=6,  RoleId = "681b047a-2255-4262-ae12-95a311c498b9", PermissionId = 1 }, // User -> Create
        };
        builder.Entity<RolePermissionModel>().HasData(rolePermissions);


    }
}
