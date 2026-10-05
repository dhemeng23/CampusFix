using Microsoft.EntityFrameworkCore;
using campusfix.Data;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<CampusFixContext>(options =>
    options.UseSqlite("Data Source=campusfix.db"));

builder.Services.AddControllersWithViews();

builder.Services.AddSession();

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

// app.UseHttpsRedirection();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<CampusFixContext>();
    db.Database.Migrate();
}
app.Run();