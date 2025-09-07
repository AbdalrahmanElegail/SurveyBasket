namespace SurveyBasket;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddDb(configuration);

        services
            .AddOpenApi()
            .AddMapsterConfigurations()
            .AddFluentValidationConfigurations();

        services.AddScoped<IPollService, PollService>();

        return services;
    }




    public static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
    {
        var ConnectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(ConnectionString));

        return services;
    }

    public static IServiceCollection AddMapsterConfigurations(this IServiceCollection services)
    {

        ////////////1st way///////
        //TypeAdapterConfig.GlobalSettings.Scan(assemblies: typeof(MappingConfigurations).Assembly);

        ////////////2nd way///////
        var mappingConfig = TypeAdapterConfig.GlobalSettings;
        mappingConfig.Scan(assemblies: typeof(MappingConfigurations).Assembly);
        services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        ////////////3rd way///////
        //var mappingConfig = TypeAdapterConfig.GlobalSettings;
        //mappingConfig.Scan(Assembly.GetExecutingAssembly());
        //services.AddSingleton<IMapper>(new Mapper(mappingConfig));

        return services;
    }

    public static IServiceCollection AddFluentValidationConfigurations(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<PollRequestValidator>();

        return services;
    }
}
