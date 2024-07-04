using lyricboxd.Data;
using lyricboxd.Models;
using lyricboxd.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

internal class Program
{
    private static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);
        builder.Services.AddSingleton<HttpClient>();
        builder.Services.AddScoped<SpotifyService>();

        // Add services to the container.
        builder.Services.AddControllersWithViews();

        var connectionString = builder.Configuration.GetConnectionString("ConnectionPostgres");
        builder.Services.AddDbContext<LyricboxdDbContext>(options => options.UseNpgsql(connectionString));

        builder.Services.AddIdentity<User, IdentityRole<Guid>>(options =>
        {
            options.Password.RequireDigit = true;
            options.Password.RequiredLength = 6;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = false;
        })
        .AddEntityFrameworkStores<LyricboxdDbContext>()
        .AddDefaultTokenProviders();

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Home/Error");
            app.UseHsts();
        }

        app.UseHttpsRedirection();
        app.UseStaticFiles();

        app.UseRouting();

        app.UseAuthentication();
        app.UseAuthorization();

        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Home}/{action=Index}/{id?}"
        );

        app.Run();
    }
}
