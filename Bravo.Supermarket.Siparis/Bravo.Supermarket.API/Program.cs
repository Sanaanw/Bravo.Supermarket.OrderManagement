using Bravo.Supermarket.API.Data;
using Bravo.Supermarket.API.Parametrs;
using Microsoft.EntityFrameworkCore;

namespace Bravo.Supermarket.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);



            // Add services to the container.
            //builder.Services.AddDbContext<AppDbContext>();

            builder.Services.AddControllers();
         //   builder.Services.AddDbContext<AppDbContext>(options =>
         //options.UseSqlServer(builder.Configuration.GetConnectionString("con_string")));



            builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("con_string")));
            builder.Services.AddScoped<sql_operation>();



            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}