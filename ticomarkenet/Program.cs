using Microsoft.EntityFrameworkCore;
using ticomarkenet.Data;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// ------------------------------------
// Servicios (ANTES de builder.Build())
// ------------------------------------
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("AppDbContext")));

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Usuarios/Login";
        options.AccessDeniedPath = "/Usuarios/AccessDenied";
    });


builder.Services.AddAuthorization();
//--------------------------------------------------
builder.Services.AddSession(); // <-- ¡Esto es obligatorio!
//--------------------------------------------------

var app = builder.Build();

// ------------------------------------
// Middleware (DESPUÉS de builder.Build())
// ------------------------------------
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();


//-------------------------------------------------------
app.UseSession(); // <-- ¡También obligatorio!
//-------------------------------------------------------
app.UseAuthentication(); // ? ¡NO TE OLVIDES!
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
