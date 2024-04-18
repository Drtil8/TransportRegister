using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Text.Json.Serialization;
using TransportRegister.Server.Configurations;
using TransportRegister.Server.Data;
using TransportRegister.Server.DTOs._Convertors;
using TransportRegister.Server.Models;
using TransportRegister.Server.Repositories;
using TransportRegister.Server.Repositories.VehicleRepository;
using TransportRegister.Server.Seeds;

namespace TransportRegister.Server
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            // Add in-memory database
            //builder.Services.AddDbContext<AppDbContext>(options => 
            //options.UseInMemoryDatabase("TransportRegisterDb"));

            // MSSQL database
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // User authentication
            builder.Services.AddIdentity<User, IdentityRole>(IdentityConfiguration.ConfigureIdentityOptions)
                .AddEntityFrameworkStores<AppDbContext>()
                .AddDefaultTokenProviders();
            
            // Repositories
            builder.Services.AddScoped<IVehicleRepository, VehicleRepository>();
            
            // Convertors
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.Converters.Add(new VehicleDtoConverter());
                // Preventing cyclic dependencies
                options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.Preserve;
            });
            
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

            var app = builder.Build();

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
                    DbCleaner.DeleteEntireDb(dbContext);
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

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthentication();
            app.UseAuthorization();
            app.UseCors("AllowSpecificOrigin");

            app.MapControllers();

            app.MapFallbackToFile("/index.html");

            app.Run();
        }
    }
}
