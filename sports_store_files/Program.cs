using Microsoft.AspNetCore.Localization;
using System.Globalization;
using sports_store_files.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// change: *
builder.Services.AddTransient<IProductRepository, EFProductRepository>();
builder.Services.AddScoped<Cart>(sp => SessionCart.GetCart(sp));
builder.Services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
builder.Services.AddTransient<IOrderRepository, EFOrderRepository>();
// added: *
//builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<AppIdentityDbContext>();

// change: *
// Configure the app configuration.
var configuration = new ConfigurationBuilder()
    .SetBasePath(builder.Environment.ContentRootPath)
    .AddJsonFile("appsettings.json")
    .Build();

// Configure the DbContext.
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(configuration["Data:SportStoreProducts:ConnectionString"]));

builder.Services.AddDbContext<AppIdentityDbContext>(options => options.UseSqlServer(configuration["Data:SportStoreIdentity:ConnectionString"]));

builder.Services.AddSession();
builder.Services.AddMemoryCache();

builder.Services.AddIdentity<IdentityUser, IdentityRole>(x => {
    x.User.RequireUniqueEmail = true;
}).AddEntityFrameworkStores<AppIdentityDbContext>().AddDefaultTokenProviders();

builder.Services.AddAuthentication();
builder.Services.Configure<IdentityOptions>(options => {
    options.SignIn.RequireConfirmedAccount = false;
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}
SeedData.EnsurePpulated(app);
IdentitySeedData.EnsurePopulated(app);
app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();
app.UseCookiePolicy();
app.UseSession();

/*app.UseIdentity();*/ // UseAuthentication equivalent to app.UseIdentity() (?)
app.UseAuthentication();

var supportedCultures = new[]
{
    new CultureInfo("en-US")
};

var localizationOptions = new RequestLocalizationOptions {
    DefaultRequestCulture = new RequestCulture("en-US"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
};

app.UseRequestLocalization(localizationOptions);

app.UseEndpoints(endpoints => {
    endpoints.MapControllerRoute(
        name: "categoryPage",
        pattern: "{category}/Page{page:int}",
        defaults: new { controller = "Product", action = "List" }
    );

    endpoints.MapControllerRoute(
        name: "page",
        pattern: "Page{page:int}",
        defaults: new { controller = "Product", action = "List", page = 1 }
    );

    endpoints.MapControllerRoute(
        name: "category",
        pattern: "{category}",
        defaults: new { controller = "Product", action = "List", page = 1 }
    );

    endpoints.MapControllerRoute(
        name: "default",
        pattern: "",
        defaults: new { controller = "Product", action = "List", page = 1 }
    );

    endpoints.MapControllerRoute(
        name: "defaultWithId",
        pattern: "{controller}/{action}/{id?}"
    );
});

app.Run();