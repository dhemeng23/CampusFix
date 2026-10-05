using Microsoft.EntityFrameworkCore;
using campusfix.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<campusfix.Data.CampusFixContext>(options =>
    options.UseSqlite("Data Source=campusfix.db"));

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddSession();

// Admin login session
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseRouting();

// Enable sessions
app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.Run();