using Dapper;
using TravelOrganization.Data.Models.Routes;

namespace TravelOrganization.Data.Repositories.Routes;

public class RouteStopRepository
{
    private readonly DataContext _context;

    public RouteStopRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<RouteStop>> GetAll(int routeId)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  SELECT eil_nr Number, atstumas Distance, laikas Time, fk_Stotele_id StopId, fk_Marsrutas_nr RouteId FROM marsruto_stoteles WHERE fk_Marsrutas_nr = @Id
                  """;
        return await connection.QueryAsync<RouteStop>(sql, new { Id = routeId });
    }

    public async Task Insert(RouteStop routeStop)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  INSERT INTO marsruto_stoteles (eil_nr, atstumas, laikas, fk_Stotele_id, fk_Marsrutas_nr) VALUES (@Number, @Distance, @Time, @StopId, @RouteId)
                  """;
        await connection.ExecuteAsync(sql, routeStop);
    }

    public async Task Delete(int routeId)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  DELETE FROM marsruto_stoteles WHERE fk_Marsrutas_nr = @Id
                  """;
        await connection.ExecuteAsync(sql, new { Id = routeId });
    }
}
