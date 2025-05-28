using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using LandingPage.Data;

var builder = WebApplication.CreateBuilder(args);

// Set EPPlus license context for non-commercial use (EPPlus v8+)

OfficeOpenXml.ExcelPackage.License.SetNonCommercialPersonal("Maghreb Steel");



// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// ✅ Corrected connection string name to match appsettings.json
builder.Services.AddDbContext<STMprocessDB>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("STMprocessDB")));

// ✅ Cookie-based authentication
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Home/Login"; // Redirect here if not authenticated
        options.AccessDeniedPath = "/Home/AccessDenied";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
    });

var app = builder.Build();

// ✅ Error handling & HTTPS
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// ✅ Disable browser caching after logout
app.Use(async (context, next) =>
{
    context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
    context.Response.Headers["Pragma"] = "no-cache";
    context.Response.Headers["Expires"] = "0";
    await next();
});

// ✅ Add authentication/authorization middleware
app.UseAuthentication();
app.UseAuthorization();

// ✅ Default route configuration
app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
