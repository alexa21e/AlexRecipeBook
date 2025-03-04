﻿using AlexRecipeBook.ApplicationServices;
using AlexRecipeBook.ApplicationServices.Abstractions;
using AlexRecipeBook.DataAccess.Abstractions;
using AlexRecipeBook.DataAccess;
using Microsoft.OpenApi.Models;
using Neo4j.Driver;

namespace AlexRecipeBookAPI.Extensions
{
    public static class ApplicationServicesExtensions
    {
        public static IServiceCollection AddApplicationServices(this IServiceCollection services,
            IConfiguration config)
        {
            services.Configure<ApplicationSettings>(config.GetSection("ApplicationSettings"));

            var settings = new ApplicationSettings();
            config.GetSection("ApplicationSettings").Bind(settings);

            services.AddSingleton(GraphDatabase.Driver(settings.Neo4jConnection, AuthTokens.Basic(settings.Neo4jUser, settings.Neo4jPassword)));

            services.AddScoped<INeo4JDataAccess, Neo4JDataAccess>();

            services.AddCors(options =>
            {
                options.AddPolicy(name: "AllowFrontEnd",
                    policy =>
                    {
                        policy.WithOrigins("https://localhost:4200")
                            .AllowAnyHeader()
                            .AllowAnyMethod();
                    });
            });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "CulinaryRecipes", Version = "v1" });
            });

            services.AddScoped<IRecipeRepository, RecipeRepository>();
            services.AddScoped<IRecipeService, RecipeService>();
            services.AddScoped<IIngredientRepository, IngredientRepository>();
            services.AddScoped<IIngredientService, IngredientService>();
            services.AddScoped<IAuthorRepository, AuthorRepository>();
            services.AddScoped<IAuthorService, AuthorService>();

            return services;
        }
    }
}
