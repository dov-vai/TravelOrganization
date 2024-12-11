using Dapper;
using TravelOrganization.Data.Models.Routes;

namespace TravelOrganization.Data.Repositories.Routes;

public class StopRepository
{
    private readonly DataContext _context;

    public StopRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Stop>> GetAll()
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  SELECT id Id, pavadinimas Name, vieta_pl Latitude, vieta_dn Longitude, ivertinimas 
                  Rating FROM stoteles
                  """;
        return await connection.QueryAsync<Stop>(sql);
    }

    public async Task Insert(Stop stop)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  INSERT INTO stoteles (pavadinimas, vieta_pl, vieta_dn, ivertinimas)
                  VALUES (@Name, @Latitude, @Longitude, @Rating)
                  """;
        await connection.ExecuteAsync(sql, stop);
    }

    public async Task Update(Stop stop)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  UPDATE stoteles
                  SET pavadinimas = @Name, vieta_pl = @Latitude, vieta_dn = @Longitude, ivertinimas = @Rating
                  WHERE id = @Id
                  """;
        await connection.ExecuteAsync(sql, stop);
    }

    public async Task Delete(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  DELETE FROM stoteles WHERE id = @Id
                  """;
        await connection.ExecuteAsync(sql, new {Id = id});
    }
}
