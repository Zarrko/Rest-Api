using _2Movies.Application.Database;
using _2Movies.Application.Models;
using Dapper;

namespace _2Movies.Application.Repositories;

public class MovieRepository : IMovieRepository
{
    private readonly IDbConnectionFactory _dbConnectionFactory;

    public MovieRepository(IDbConnectionFactory dbConnectionFactory)
    {
        _dbConnectionFactory = dbConnectionFactory;
    }
    
    public async Task<bool> CreateAsync(Movie movie, CancellationToken token = default)
    {
        using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = conn.BeginTransaction();

        var result = await conn.ExecuteAsync(new CommandDefinition("""
                                                                   insert into movies (id, slug, title, yearofrelease)
                                                                   values (@Id, @Slug, @Title, @YearOfRelease)
                                                                   """, movie, cancellationToken: token));

        if (result > 0)
        {
            foreach (var genre in movie.Genres)
            {
                await conn.ExecuteAsync(new CommandDefinition("""
                                                              insert into genres (movieId, name)
                                                              values (@MovieId, @Name)
                                                              """, new { MovieId = movie.Id, Name = genre } , cancellationToken: token));

            }
        }
        
        transaction.Commit();
        return  result > 0;
    }

    public async Task<Movie?> GetByIdAsync(Guid id, CancellationToken token = default)
    {
        using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);
        var movie = await conn.QuerySingleOrDefaultAsync<Movie>(new CommandDefinition(
            """
            select * from movies where id = @id
            """, new { id } , cancellationToken: token
        ));

        if (movie == null)
        {
            return null;
        }

        var genres = await conn.QueryAsync<string>(new CommandDefinition(
            """
            select name from genres where movieid = @id
            """, new { id }, cancellationToken: token
        ));

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
    }

    public async Task<Movie?> GetBySlugAsync(string slug, CancellationToken token = default)
    {
        using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);
        var movie = await conn.QuerySingleOrDefaultAsync<Movie>(new CommandDefinition(
            """
            select * from movies where slug = @slug
            """, new { slug }, cancellationToken: token
        ));

        if (movie == null)
        {
            return null;
        }

        var genres = await conn.QueryAsync<string>(new CommandDefinition(
            """
            select name from genres where movieid = @id
            """, new { id = movie.Id}, cancellationToken: token
        ));

        foreach (var genre in genres)
        {
            movie.Genres.Add(genre);
        }

        return movie;
    }

    public async Task<IEnumerable<Movie>> GetAllAsync(CancellationToken token = default)
    {
        using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);
        var result = await conn.QueryAsync(new CommandDefinition(
            """
            select m.*, string_agg(g.name, ',') as genres
            from movies m left join genres g on m.id = g.movieid
            group by id
            """ , cancellationToken: token
        ) );

        return result.Select(x => new Movie
        {
            Id = x.id,
            Title = x.title,
            YearOfRelease = x.yearofrelease,
            Genres = Enumerable.ToList(x.genres.Split(','))
        });
    }

    public async Task<bool> UpdateAsync(Movie movie, CancellationToken token = default)
    {
        using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = conn.BeginTransaction();

        await conn.ExecuteAsync(new CommandDefinition("""
                                                       delete from genres where movieid = @id
                                                      """, new { id = movie.Id } , cancellationToken: token));
        
        foreach (var genre in movie.Genres)
        {
            await conn.ExecuteAsync(new CommandDefinition("""
                                                          insert into genres (movieId, name)
                                                          values (@MovieId, @Name)
                                                          """, new { MovieId = movie.Id, Name = genre } , cancellationToken: token));

        }

        var result = await conn.ExecuteAsync(new CommandDefinition("""
                                                                   update movies set slug = @Slug, title = @Title, yearofrelease = @YearOfRelease
                                                                   where id = @Id
                                                                   """, movie , cancellationToken: token));
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> DeleteByIdAsync(Guid id, CancellationToken token = default)
    {
        using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);
        using var transaction = conn.BeginTransaction();

        await conn.ExecuteAsync(new CommandDefinition("""
                                                       delete from genres where movieid = @id
                                                      """, new { id }));
        var result = await conn.ExecuteAsync(new CommandDefinition("""
                                                              delete from movies where id = @id
                                                             """, new { id } , cancellationToken: token));
        
        transaction.Commit();
        return result > 0;
    }

    public async Task<bool> ExistsByIdAsync(Guid id, CancellationToken token = default)
    {
        using var conn = await _dbConnectionFactory.CreateConnectionAsync(token);
        return await conn.ExecuteScalarAsync<bool>(new CommandDefinition(
            """
            select count(1) from movies where id = @id
            """, new { id } , cancellationToken: token
        ));
    }
}