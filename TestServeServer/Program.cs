using TestIServe.Server.Sensors;
using TestIServe.Server.Configurations;
using TestIServe.Server.Sensors.Domain;
using TestIServe.Server.Services;
using TestIServe.Server.Services.WeatherSensorEmulatorService;
using TestIServe.Server.Storage;
using TestIServe.Server.Storage.Commands;
using TestIServe.Server.Storage.Queries;
using TestIServe.Server.Services.SensorValueGenerator;

public static class Program
{

    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddServices()
            .AddSettings(builder.Configuration);

        var app = builder.Build();

        //// Configure the HTTP request pipeline.
        if (!app.Environment.IsDevelopment())
        {
            app.UseExceptionHandler("/Sensors/Error");
            // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
            app.UseHsts();
        }
        else
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.Services.GetService<ISensorsSetup>()?.LoadSensors();
        app.Services.GetService<ISensorDataGenerationService>()?.RunProcessGenerateData();
        app.Services.GetService<ISensorDataAggregationService>()?.RunAggregateProcess();

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();


        app.MapControllerRoute(
            name: "default",
            pattern: "{controller=Sensors}/{action=Index}/{id?}");

        // Configure the HTTP request pipeline.
        app.MapGrpcService<WeatherSensorEmulatorService>();

        app.Run();
    }


    public static IServiceCollection AddServices(this IServiceCollection services)
    {
        services
            .AddImplementations()
            .AddGrpc();

        services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();

        return services;
    }

    public static IServiceCollection AddImplementations(this IServiceCollection serviceCollection)
    {
        serviceCollection
            .AddTransient<ISensorFactory, SensorFactory>()
            .AddSingleton<ISensorRepository, SensorRepository>()
            .AddSingleton<IStorageContext, StorageContext>()
            .AddTransient<ISensorsSetup, SensorsSetup>()
            .AddTransient<ISensorDataAggregationService, SensorDataAggregationService>()
            .AddTransient<ISensorDataGenerationService, SensorDataGenerationService>()
            .AddTransient<ISensorsQueryService, SensorsQueryService>()
            .AddTransient<ISensorsCommandService, SensorsCommandService>()
            .AddSingleton<ISensorValueGenerator, SensorValueGenerator>()

            .AddScoped<WeatherSensorEmulatorService>();

        return serviceCollection;
    }

    public static IServiceCollection AddSettings(this IServiceCollection serviceCollection, ConfigurationManager configurationManager)
    {
        serviceCollection.AddOptions();

        serviceCollection.AddOptions<GenerationEventsServiceOptions>()
            .Bind(configurationManager.GetSection("serviceGenerationEvents"))
            .ValidateDataAnnotations();

        serviceCollection.AddOptions<SensorDataAggregationServiceOptions>()
            .Bind(configurationManager.GetSection("sensorDataAggregationService"))
            .ValidateDataAnnotations();

        return serviceCollection;
    }
}
