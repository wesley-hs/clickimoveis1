using click_imoveis.DataSeed;
using click_imoveis.Models;
using click_imoveis.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Serilog;






var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(builder.Configuration)            
            .CreateLogger();

builder.Host.UseSerilog(); 



builder.Services.AddControllers();
// Add services to the container. //
builder.Services.AddControllersWithViews();

builder.Services.AddRazorPages().AddRazorRuntimeCompilation();

builder.Services.AddDbContext<AppDbContext>(options =>
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.Configure<CookiePolicyOptions>(options =>
{
    options.CheckConsentNeeded = context => true;
    options.MinimumSameSitePolicy = SameSiteMode.None;
});

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.AccessDeniedPath = "/Usuarios/AccessDenied/";
        options.LoginPath = "/Usuarios/Login";
    });


// Program.cs (ou Startup.ConfigureServices)
builder.Services.AddTransient<EmailService>();

// No Program.cs, ADICIONE:
builder.Services.AddScoped<EmailService>();


var app = builder.Build();


// Executa o carregamento de dados iniciais caso o banco esteja vazio
await DataSeedImporter.ImportDataAsync(app.Services);



//The following code displays the environment variables and values on application startup, which can be helpful when debugging environment settings:
//foreach (var c in builder.Configuration.AsEnumerable())
//{
//    Console.WriteLine(c.Key + " = " + c.Value);
//}

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


app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

try
{
    Log.Information("Iniciando a aplicação web");
    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "A aplicação falhou ao iniciar");
}
finally
{
    // Garante que todos os logs sejam gravados antes de sair.
    Log.CloseAndFlush();
    
}