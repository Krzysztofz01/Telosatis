using Microsoft.OpenApi.Models;

namespace Telosatis.Api.Configuration;

internal static class OpenApiConfiguration
{
    private const string DocumentTitle = "Telosatis API";
    private const string DocumentVersion = "v1";
    private const string BearerSchemeId = "Bearer";
    
    public static IServiceCollection AddOpenApi(this IServiceCollection services, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
            return services;
            
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(DocumentVersion, new OpenApiInfo
            {
                Title = DocumentTitle,
                Version = DocumentVersion
            });

            var bearerScheme = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                Description = "Paste the access token (without the 'Bearer ' prefix.",
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = BearerSchemeId,
                },
            };

            options.AddSecurityDefinition(BearerSchemeId, bearerScheme);
            
            options.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                [ bearerScheme ] = [],
            });
            
            options.SupportNonNullableReferenceTypes();
            
            var xmlFile = Path.Combine(AppContext.BaseDirectory, $"{typeof(OpenApiConfiguration).Assembly.GetName().Name}.xml");
            if (File.Exists(xmlFile))
                options.IncludeXmlComments(xmlFile);
        });
        
        return services;
    }

    public static WebApplication UseOpenApi(this WebApplication app)
    {
        if (!app.Environment.IsDevelopment())
            return app;
        
        app.UseSwagger();
        app.UseSwaggerUI(options =>
		{
			options.SwaggerEndpoint($"{DocumentVersion}/swagger.json", DocumentTitle);
            options.DocumentTitle = DocumentTitle;
            options.EnablePersistAuthorization();
            options.DisplayRequestDuration();
        });

        return app;
    }
}