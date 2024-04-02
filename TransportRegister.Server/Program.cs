using Microsoft.EntityFrameworkCore;
using TransportRegister.Server.Data;
using TransportRegister.Server.Seeds;

namespace TransportRegister.Server
{
    public class Program
    {
        public static void Main(string[] args)
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

            var app = builder.Build();
            
            // For seed data use cmd: dotnet run seed
            if (args.Length > 0 && args[0] == "seed")
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                DbSeeder.SeedAll(dbContext);
                return;
            }
            // For database cleanup use cmd: dotnet run clear-db
            if (args.Length > 0 && args[0] == "clear-db")
            {
                using var scope = app.Services.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<AppDbContext>();
                DbCleaner.ClearAllData(dbContext);
                return;
            }

            app.UseDefaultFiles();
            app.UseStaticFiles();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.MapFallbackToFile("/index.html");
    
            
            app.Run();
        }
    }
}
