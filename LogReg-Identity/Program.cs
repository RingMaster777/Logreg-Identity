using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using LogReg_Identity.Data;
using Serilog;
using LogReg_Identity.Middlewares;
using LogReg_Identity.Models;
using LogReg_Identity.Repository.IRepository;
using LogReg_Identity.Repository;
var builder = WebApplication.CreateBuilder(args);



// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();



var connectionString = builder.Configuration.GetConnectionString("ApplicationDbContextConnection") ?? throw new InvalidOperationException("Connection string 'ApplicationDbContextConnection' not found.");

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

builder.Services.AddDefaultIdentity<ApplicationUser>(options => options.SignIn.RequireConfirmedAccount = false)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>();


builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped<LogReg_Identity.Repository.IRepository.IUserRepository, LogReg_Identity.Repository.UserRepository>();
builder.Services.AddScoped<LogReg_Identity.Services.IUserService, LogReg_Identity.Services.UserService>();
builder.Services.AddScoped<LogReg_Identity.Services.INoteService, LogReg_Identity.Services.NoteService>();
builder.Services.AddScoped<LogReg_Identity.Services.IMenuService, LogReg_Identity.Services.MenuService>();

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages();

//builder.Logging.SetMinimumLevel(LogLevel.Debug);




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
else
{
    app.UseDeveloperExceptionPage();
}

// Register the custom exception handling middleware
app.UseMiddleware<PermissionMiddleware>();
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.MapRazorPages();


// Apply migrations and seed data
using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
    try
    {
        // Apply any pending migrations
        await dbContext.Database.MigrateAsync();
        Log.Information("Database migrated successfully");
    }
    catch (Exception ex)
    {
        Log.Error(ex, "An error occurred while migrating the database");
    }

    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

    var roles = new[] { "Admin", "Member" };
    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
            Log.Information($"Role '{role}' created successfully");
        }
    }
}

app.Run();
