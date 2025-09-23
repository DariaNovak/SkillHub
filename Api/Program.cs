using Api.Modules;
using Application;
using Infrastructure.Persistence;

namespace Api
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.SetupServices();
            builder.Services.AddApplicationServices();
            builder.Services.AddPersistenceServices();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            await app.InitialiseDatabaseAsync();

            app.UseCors();
            app.MapControllers();

            app.Run();
        }
    }
}
