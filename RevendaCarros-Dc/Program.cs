
using Microsoft.EntityFrameworkCore;
using RevendaCarros_Dc.Domain.Context;
using RevendaCarros_Dc.Infra.Repositories;
using RevendaCarros_Dc.Infra.Repositories.Interfaces;
using System.Reflection;

namespace RevendaCarros_Dc
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var connectionString = builder.Configuration.GetConnectionString("DefaultConncection");
 

            builder.Services.AddControllers();

            builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

            builder.Services.AddDbContext<RevendaCarrosDbContext>(options =>
            {
                options.UseLazyLoadingProxies()
                   .UseMySql
                (
                    connectionString: connectionString,
                    serverVersion: ServerVersion.AutoDetect(connectionString)
                );
            });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opitions => 
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opitions.IncludeXmlComments(xmlPath);
            } );

            builder.Services.AddScoped<IRevendaRepository, RevendaRepository>();

            var app = builder.Build();

           
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();

            app.UseCors(options => options.WithOrigins("http://localhost:4200").AllowAnyMethod().AllowAnyHeader());

            app.UseAuthorization();


            app.MapControllers();
          
            app.Run();
        }
    }
}
