using Api.Modules;
using Application;
using Infrastructure.Persistence;
using FastEndpoints;
using FastEndpoints.Swagger;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.SetupServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddPersistenceServices(builder.Configuration);

            builder.Services.AddFastEndpoints();
            builder.Services.SwaggerDocument(); 

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwaggerGen();
                app.UseSwaggerUI();
            }

            await app.InitialiseDatabaseAsync();

            app.UseCors();
            app.UseFastEndpoints();

            app.Run();
        }
    }
}

public partial class Program { }