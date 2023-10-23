using _2Movies.Application.Database;
using _2Movies.Application.Repositories;
using _2Movies.Application.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace _2Movies.Application;

public static class ApplicationServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection serviceCollection)
    {
        serviceCollection.AddSingleton<IMovieRepository, MovieRepository>();
        serviceCollection.AddSingleton<IMovieService, MovieService>();
        serviceCollection.AddValidatorsFromAssemblyContaining<IApplicationMarker>(ServiceLifetime.Singleton);
        return serviceCollection;
    }

    public static IServiceCollection AddDatabase(this IServiceCollection serviceCollection, string connectionString)
    {
        serviceCollection.AddSingleton<IDbConnectionFactory>(_ => new NpgsqlConnectionFactory(connectionString));
        serviceCollection.AddSingleton<DbInitializer>();
        return serviceCollection;
    }
}