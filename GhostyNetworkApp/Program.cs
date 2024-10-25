using GhostyNetwork.Core.Application.Helpers;
using GhostyNetwork.Core.Application.ServiceRegistration;
using GhostyNetwork.Core.Domain.Entities;
using GhostyNetwork.Infrastructure.Persistence;
using GhostyNetwork.Infrastructure.Persistence.Contexts;
using GhostyNetwork.Infrastructure.Shared;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

// connection string DB
var connecString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationContext>(options => options.UseSqlServer(connecString));

// Registro de los Repositorios (Infraestructura)
builder.Services.AddPersistenceInfrastructure(builder.Configuration);

// Registro de los Servicios (Aplicación)
builder.Services.AddAplicationLayer(builder.Configuration);

// Registro de la Infraestructura compartida (Servicios adicionales como IEmailService)
builder.Services.AddSharedInfrastructure(builder.Configuration);

// Registro de IPasswordHasher
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();


// confi coookies
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";
        options.LogoutPath = "/User/Logout";
        options.ExpireTimeSpan = TimeSpan.FromMinutes(30);
        options.SlidingExpiration = true;
        options.AccessDeniedPath = "/User/AccessDenied"; // home antes
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

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=User}/{action=Login}/{id?}");

app.Run();
