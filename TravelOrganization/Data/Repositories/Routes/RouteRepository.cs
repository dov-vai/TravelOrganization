using Dapper;
using TravelOrganization.Data.Models.Routes;

namespace TravelOrganization.Data.Repositories.Routes;

public class RouteRepository
{
    private readonly DataContext _context;

    public RouteRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Models.Routes.Route>> GetAll()
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  SELECT nr Id, pavadinimas Name, tarifas Rate FROM marsrutai
                  """;
        return await connection.QueryAsync<Models.Routes.Route>(sql);
    }

    public async Task<Models.Routes.Route> Get(int routeId)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  SELECT nr Id, pavadinimas Name, tarifas Rate FROM marsrutai
                  WHERE nr = @Id
                  """;
        return await connection.QuerySingleAsync<Models.Routes.Route>(sql, new { Id = routeId });
    }

    public async Task<int> Insert(Models.Routes.Route route)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  INSERT INTO marsrutai (pavadinimas, tarifas) VALUES (@Name, @Rate);
                  SELECT last_insert_rowid();
                  """;
        return await connection.QuerySingleAsync<int>(sql, route);
    }

    public async Task<int> Update(Models.Routes.Route route)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  UPDATE marsrutai
                  SET pavadinimas = @Name, tarifas = @Rate WHERE nr = @Id
                  """;
        return await connection.ExecuteAsync(sql, route);
    }

    public async Task Delete(int routeId)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  DELETE FROM marsrutai WHERE nr = @Id
                  """;
        await connection.ExecuteAsync(sql, new { Id = routeId });
    }
}
