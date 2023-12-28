using MOLS.Shared;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .WriteTo.Console().CreateLogger();

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddMvc().AddMvcModules(builder.Services, builder.Configuration);
builder.Services.AddControllersWithViews();;

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

app.UseMvcModulesStaticFiles(app.Services.GetRequiredService<ILoggerFactory>(), builder.Configuration);
// app.UseMvc(routes =>
// {
//     // Add route for area handling
//     routes.UseMvcModulesRoute();
//     routes.MapRoute(name: "default", template: "{controller=Home}/{action=Index}/{id?}");
// });


app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();