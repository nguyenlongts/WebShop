using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using WebShop.Areas.Customer.Repositories.Implement;
using WebShop.Areas.Customer.Repositories.Interface;
using WebShop.Data;
using WebShop.Identity;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<WebShopContext>(options =>	options.UseSqlServer(builder.Configuration.GetConnectionString("String")));

builder.Services.AddScoped<ICartRepository,CartRepository>();
builder.Services.AddScoped<IProductRepository,ProductRepository>();

builder.Services.AddAuthentication("CustomerCookie") 
		.AddCookie("AdminCookie", options =>
		{
			options.Cookie.Name = "AdminCookie";
			options.LoginPath = "/Admin-Login";
			options.AccessDeniedPath = "/Admin-AccessDenied";
			options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
		})
		.AddCookie("CustomerCookie", options =>
		{
			options.Cookie.Name = "CustomerCookie";
			options.LoginPath = "/Login"; 
			options.AccessDeniedPath = "/AccessDenied";
			options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
		});




var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();

app.UseAuthorization();

app.UseEndpoints(endpoints =>
{
	endpoints.MapAreaControllerRoute(
		name: "customer",
		areaName: "Customer",
		pattern: "Customer/{controller=Home}/{action=Index}/{id?}");

	endpoints.MapAreaControllerRoute(
		name: "admin",
		areaName: "Admin",
		pattern: "Admin/{controller=Admin}/{action=Index}/{id?}");


});


app.Run();
