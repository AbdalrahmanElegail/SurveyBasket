using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace SurveyBasket;

public static class DependencyInjection
{
    public static IServiceCollection AddDependencies(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddControllers();

        services.AddCors(options => 
            options.AddDefaultPolicy(builder => 
                builder
                    .AllowAnyMethod()  
                    .AllowAnyHeader()
                    .WithOrigins(configuration.GetSection("AllowedOrigins").Get<string[]>()!)
            )
        );

        services.AddAuthConfigurations(configuration);

        services.AddDb(configuration);

        services
            .AddOpenApi()
            .AddMapsterConfigurations()
            .AddFluentValidationConfigurations();


        //services.AddServices();
        services.AddScoped<IPollService, PollService>();
        services.AddScoped<IAuthService, AuthService>();


        return services;
    }








    //public static IServiceCollection AddServices(this IServiceCollection services)
    //{
    //    services.AddScoped<IPollService, PollService>();
    //    services.AddScoped<IAuthService, AuthService>();

    //    return services;
    //}


    private static IServiceCollection AddDb(this IServiceCollection services, IConfiguration configuration)
    {
        var ConnectionString = configuration.GetConnectionString("DefaultConnection") ??
            throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseSqlServer(ConnectionString));

        return services;
    }

    private static IServiceCollection AddMapsterConfigurations(this IServiceCollection services)
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

    private static IServiceCollection AddFluentValidationConfigurations(this IServiceCollection services)
    {
        services
            .AddFluentValidationAutoValidation()
            .AddValidatorsFromAssemblyContaining<PollRequestValidator>();

        return services;
    }
    private static IServiceCollection AddAuthConfigurations(this IServiceCollection services, IConfiguration configuration)
    {
        services
            .AddIdentity<ApplicationUser, IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>();


        services.AddSingleton<IJwtProvider, JwtProvider>();
     
        //services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        services.AddOptions<JwtOptions>()
            .BindConfiguration(JwtOptions.SectionName)
            .ValidateDataAnnotations()
            .ValidateOnStart();

        var JwtSettings = configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>();

        services
        .AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.SaveToken = true;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                ValidateIssuer = true,
                ValidateAudience = true,
                ValidateLifetime = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtSettings?.Key!)),
                ValidIssuer = JwtSettings?.Issuer,
                ValidAudience = JwtSettings?.Audience
            };
        });
        return services;
    }
}
