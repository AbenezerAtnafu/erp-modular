using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ERP;
using ERP.Areas.Identity.Data;
using Microsoft.Extensions.FileProviders;
using ERP.Interface;
using ERP.Service;
using Minio;
using Minio.AspNetCore;
using Microsoft.Extensions.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);
var connectionString = builder.Configuration.GetConnectionString("UserDbContextConnection") ?? throw new InvalidOperationException("Connection string 'UserDbContextConnection' not found.");

builder.Services.AddDbContext<UserDbContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddDbContext<employee_context>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<UserDbContext>();
builder.Services.AddScoped<ICacheService, CacheService>();
builder.Services.AddIdentityCore<User>().AddUserManager<UserManager<User>>();

// Add services to the container.b
//builder.Services.AddControllersWithViews();
builder.Services.ConfigureApplicationCookie(options =>
{
    // Cookie settings
    options.Cookie.HttpOnly = true;
    options.ExpireTimeSpan = TimeSpan.FromMinutes(30000);
    options.LoginPath = "/Identity/Account/Login";
    options.AccessDeniedPath = "/Identity/Account/AccessDenied";
    options.SlidingExpiration = true;

});

string syspath = "";
builder.Services.AddSingleton<IFileProvider>(
        new PhysicalFileProvider(
            Path.Combine(Directory.GetCurrentDirectory(), syspath)));



// ...



builder.Services.AddControllers();
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();



var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication(); ;

app.UseAuthorization();

app.MapRazorPages();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;
var userManager = services.GetRequiredService<UserManager<User>>();
var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();


await SeedRoles.SeedRolesAsync(userManager, roleManager);
await SeedRoles.SeedSuperAdminAsync(userManager, roleManager);

//await Task.Delay(Timeout.Infinite);
app.Run();