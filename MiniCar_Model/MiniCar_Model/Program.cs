using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.EntityFrameworkCore;
using MiniCar_Model.Models;
using MiniCar_Model.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Begin Nhat
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));
//End Nhat
builder.Services.AddScoped<CartService>();

builder.Services.AddSession(options => {
  options.IdleTimeout = TimeSpan.FromHours(2);
  options.Cookie.HttpOnly = true;
  options.Cookie.IsEssential = true;
});


builder.Services.AddSession(options =>
{
  options.IdleTimeout = TimeSpan.FromHours(2);
  options.Cookie.HttpOnly = true;
  options.Cookie.IsEssential = true;
});

var app = builder.Build();


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment()) {
  app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseSession();
//app.UseAuthorization();



//-------Tri Trong Trang

app.MapControllerRoute(
    name: "Areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

//-------

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
