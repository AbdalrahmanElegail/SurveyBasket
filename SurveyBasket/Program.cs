using Serilog;

namespace SurveyBasket;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddDependencies(builder.Configuration);

        builder.Host.UseSerilog((context, configuration) =>
            configuration.ReadFrom.Configuration(context.Configuration)
        );

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            app.MapScalarApiReference();
        }

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.UseCors();

        app.UseAuthorization();

        app.MapControllers();

        app.UseExceptionHandler();

        app.Run();
    }
}
