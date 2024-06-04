
//using ecomerceWithAngularAnd_Api.Data;

using AutoMapper;
using Core.Helpers;
using Infrastructur.Data;
using Infrastructur.repository;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Core
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container. builder.Services.AddDbContext<Context>(Options =>
            builder.Services.AddControllers();
            builder.Services.AddDbContext<storeContext>(Options =>
            {
                Options.UseSqlServer(builder.Configuration.GetConnectionString("cs"));
            }
            );

            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
            builder.Services.AddAutoMapper(typeof(MapperProfile));
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            DataSeddingAsync();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();


            async Task DataSeddingAsync()
            {
               
               using(var scope=app.Services.CreateScope())
                {
                    var services=scope.ServiceProvider;
                    var loggerFactory=services.GetRequiredService<ILoggerFactory>();
                    try
                    {
                        var context = services.GetRequiredService<storeContext>();
                        await context.Database.MigrateAsync();
                         DataSeedContext.Dataseeding(context,loggerFactory);
                    }
                    catch (Exception ex)
                    {
                        var logger=loggerFactory.CreateLogger<Program>();
                        logger.LogError(ex, "log error happened in program migration");
                    }

                }
            }
        }
    }
}
