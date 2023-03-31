using NewsBlog.Data;
using Microsoft.EntityFrameworkCore;
using NewsBlog.Models;
using Microsoft.AspNetCore.Identity;
using NewsBlog.Utilities;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("DatabaseConnection");

builder.Services.AddDbContext<AppDbContext>(
    options => options.UseSqlServer(connectionString));

builder.Services.AddIdentity<User, IdentityRole>()
    .AddDefaultTokenProviders()
    .AddEntityFrameworkStores<AppDbContext>();

builder.Services.AddScoped<IDataInitializer, DataInitializer>();

var app = builder.Build();
DataSeeding();

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

app.UseAuthorization();

// If there is area it will go on area if not go on default
app.MapControllerRoute(
    name: "area",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");



app.Run();

void DataSeeding()
{
    using(var scope = app.Services.CreateScope())
    {
        var DataInitialize = scope.ServiceProvider.GetRequiredService<IDataInitializer>();
        DataInitialize.Initialize();
    }
}