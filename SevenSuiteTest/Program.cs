using DbService;
using DbService.Service.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<Context>(options =>    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddTransient<DbServiceContext>();
builder.Services.AddScoped<ISeveClieInterface, SeveClieService>();
builder.Services.AddScoped<IEstadoCivilInterface, EstadoCivilService>();
builder.Services.AddScoped<IUserInterface, UserService>();
builder.Services.AddLogging();
builder.Services.AddAuthentication("MyCookieAuth")
            .AddCookie("MyCookieAuth", options =>
            {
                options.Cookie.Name = "MyCookieAuth";
                options.LoginPath = "/Account/Login";
            });

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

using (var scope = app.Services.CreateScope())
{
    var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();
    try
    {
        var dbContext = scope.ServiceProvider.GetRequiredService<Context>();
        dbContext.Database.Migrate();

        var dbService = scope.ServiceProvider.GetRequiredService<DbServiceContext>();
        dbService.Initialize();

        logger.LogInformation("Migraciones aplicadas y base de datos inicializada.");
    }
    catch (Exception ex)
    {
        logger.LogError(ex, "Ocurrió un error aplicando las migraciones.");
    }
}

app.Run();
