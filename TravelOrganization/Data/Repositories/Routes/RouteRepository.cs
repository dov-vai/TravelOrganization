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
                  SELECT id Id, pavadinimas Name, tarifas Rate FROM marsrutai
                  """;
        return await connection.QueryAsync<Models.Routes.Route>(sql);
    }

    public async Task Insert(Models.Routes.Route route)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  INSERT INTO marsrutai (nr, pavadinimas, tarifas) VALUES (@Id, @Name, @Rate)
                  """;
        await connection.ExecuteAsync(sql, route);
    }

    public async Task Delete(Models.Routes.Route route)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  DELETE FROM marsrutai WHERE id = @Id
                  """;
        await connection.ExecuteAsync(sql, route);
    }
}