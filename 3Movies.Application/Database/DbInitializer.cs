using Dapper;

namespace _2Movies.Application.Database;

public class DbInitializer
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public DbInitializer(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }

    public async Task InitializeAsync()
    {
        using var conn = await _dbConnectionFactory.CreateConnectionAsync();
        await conn.ExecuteAsync("""
                                 create table if not exists movies (
                                     id UUID primary key,
                                     slug TEXT not null,
                                     title TEXT not null,
                                     yearofrelease integer not null);
                                """);
        await conn.ExecuteAsync("""
                                 create unique index concurrently if not exists movies_slug_idx 
                                 on movies 
                                 using btree(slug);
                                """);

        await conn.ExecuteAsync("""
                                 create table if not exists genres (
                                    movieId UUID references movies (Id),
                                    name TEXT not null);
                                """);

    }
}