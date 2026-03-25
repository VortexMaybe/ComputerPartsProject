using ComputerParts.Data;
using ComputerParts.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

var builder = WebApplication.CreateBuilder(args);

// 1. Конфигурация на Базата данни
builder.Services.AddDbContext<ComputerPartsContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("ComputerPartsContext") ?? throw new InvalidOperationException("Connection string 'ComputerPartsContext' not found.")));

// 2. Добавяне на Identity (Потребители и Роли)
builder.Services.AddDefaultIdentity<IdentityUser>(options => {
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireDigit = false;
    options.Password.RequiredLength = 3;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase = false;
    options.Password.RequireLowercase = false;
})
.AddRoles<IdentityRole>() // Admin ролята
.AddEntityFrameworkStores<ComputerPartsContext>();

builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages(); // Нужно за вградените страници за Логин

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

// 3. Активиране на Authentication (Кой си ти?) и Authorization (Какво можеш?)
app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=HardwareComponents}/{action=Index}/{id?}");

app.MapRazorPages(); // Регистрира пътищата за Identity

// 4. Seed-ване на данни (Твоите компоненти)
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    SeedData.Initialize(services).GetAwaiter().GetResult();
}

app.Run();