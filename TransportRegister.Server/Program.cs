using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using System.Globalization;
using TransportRegister.Server.Configurations;
using TransportRegister.Server.Data;
using TransportRegister.Server.Models;
using TransportRegister.Server.Repositories;
using TransportRegister.Server.Repositories.Implementations;
using TransportRegister.Server.Seeds;

namespace TransportRegister.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            // Set default czech datetime format
            Thread.CurrentThread.CurrentCulture = new CultureInfo("cs-CZ");

            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // MSSQL database
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // User authentication
            builder.Services.AddIdentity<User, IdentityRole>(IdentityConfiguration.ConfigureIdentityOptions)
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders()
                .AddRoles<IdentityRole>();

            // Repositories
            builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
            builder.Services.AddScoped<ITheftRepository, TheftRepository>();
            builder.Services.AddScoped<IOffenceRepository, OffenceRepository>();
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IPersonRepository, PersonRepository>();

            // Cookies settings
            builder.Services.ConfigureApplicationCookie(options =>
            {
                options.LoginPath = "/login";
                options.Cookie.HttpOnly = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.ExpireTimeSpan = TimeSpan.FromDays(2);
                options.SlidingExpiration = true;
                options.Cookie.SameSite = SameSiteMode.None;
                options.Cookie.SecurePolicy = CookieSecurePolicy.Always;
            });

            builder.Services.AddQuartzConfiguration();

            var app = builder.Build();

            // Enable to apply changes to db in Azure hosting
            //await using (AppDbContext context = app.Services.CreateScope().ServiceProvider.GetService<AppDbContext>())
            //{
            //    await context.Database.EnsureDeletedAsync();
            //    await context.Database.MigrateAsync();
            //    await DbSeeder.SeedAll(app.Services);
            //}

            // For seed data use cmd: dotnet run seed
            if (args.Length > 0)
            {
                if (args[0] == "seed")
                {
                    // For seed data use cmd: dotnet run seed
                    await DbSeeder.SeedAll(app.Services);
                    return;
                }
                else if (args[0] == "delete-db")
                {
                    // For database delete use cmd: dotnet run delete-db
                    using var scope = app.Services.CreateScope();
                    var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                    await DbCleaner.DeleteEntireDb(dbContext);
                    return;
                }
            }

            // Set Cors
            app.UseCors(corsBuilder =>
            {
                corsBuilder.WithOrigins(
                        "https://localhost:5173",
                        "http://localhost:5173")
                    .AllowAnyMethod()
                    .AllowAnyHeader()
                    .AllowCredentials();
            });
            app.UseCors("AllowSpecificOrigin");

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            //else
            //{
            //    app.UseExceptionHandler("/error");
            //    app.UseHsts();
            //}

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
