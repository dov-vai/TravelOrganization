using Dapper;
using TravelOrganization.Data.Models.Reviews;

namespace TravelOrganization.Data.Repositories.Reviews;

public class ReviewRepository : IReviewRepository
{
    private readonly DataContext _context;

    public ReviewRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Review>> GetAll()
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  SELECT id Id, fk_Vartotojas_id userId, fk_Stotele_id StopId, tekstas Content, ivertinimas Rating, data Date
                  FROM atsiliepimai
                  """;
        return await connection.QueryAsync<Review>(sql);
    }

    public async Task Insert(Review review)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  INSERT INTO atsiliepimai (id, fk_Vartotojas_id, fk_Stotele_id, tekstas, ivertinimas, data)
                  VALUES (@Id, @UserId, @StopId, @Content, @Rating, @Date)
                  """;
        await connection.ExecuteAsync(sql, review);
    }
}