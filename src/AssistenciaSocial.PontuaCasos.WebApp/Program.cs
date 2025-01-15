using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using AssistenciaSocial.PontuaCasos.WebApp.Models;
using System.Globalization;
using Microsoft.AspNetCore.Localization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews(options => options.SuppressImplicitRequiredAttributeForNonNullableReferenceTypes = true);

builder.Services.AddDbContext<PontuaCasosContext>(options =>
    options.UseSqlServer($"Server={builder.Configuration["DatabaseServer"]};Database={builder.Configuration["DatabaseName"]};User Id={builder.Configuration["DatabaseUser"]};Password={builder.Configuration["DatabasePassword"]};Encrypt=True;TrustServerCertificate=True;"));

builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<Usuario>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireDigit = false;
    options.Password.RequireUppercase = false;
})
    .AddRoles<IdentityRole>()
    .AddEntityFrameworkStores<PontuaCasosContext>();

if (builder.Configuration["Authentication:Google:ClientId"] != null && builder.Configuration["Authentication:Google:ClientSecret"] != null)
{
    builder.Services.AddAuthentication().AddGoogle(googleOptions =>
    {
        googleOptions.ClientId = builder.Configuration["Authentication:Google:ClientId"] ?? "";
        googleOptions.ClientSecret = builder.Configuration["Authentication:Google:ClientSecret"] ?? "";
    });
}

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

// Definindo a cultura padrão: pt-BR
var supportedCultures = new[] { new CultureInfo("pt-BR") };
app.UseRequestLocalization(new RequestLocalizationOptions
{
    DefaultRequestCulture = new RequestCulture(culture: "pt-BR", uiCulture: "pt-BR"),
    SupportedCultures = supportedCultures,
    SupportedUICultures = supportedCultures
});

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

/// Data Seeding dos perfis base da aplicação
using (var scope = app.Services.CreateScope())
{
    var configuration = scope.ServiceProvider.GetRequiredService<IConfiguration>();
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<Usuario>>();

    var roles = new[] { "Administradores", "Usuários", "Gestores" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    // Seed admin user
    var adminEmail = configuration["AdminUser"] ?? "";
    var adminPassword = configuration["AdminPassword"] ?? "";

    if (await userManager.FindByEmailAsync(adminEmail) == null)
    {
        var adminUser = new Usuario { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
        var result = await userManager.CreateAsync(adminUser, adminPassword);

        if (result.Succeeded)
        {
            await userManager.AddToRoleAsync(adminUser, "Administradores");
        }
    }

}

app.Run();
