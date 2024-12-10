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

    public async Task<Review?> Get(int id)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  SELECT
                      a.id AS Id, 
                      a.fk_Vartotojas_id AS userId, 
                      a.fk_Stotele_id AS StopId, 
                      a.tekstas AS Content, 
                      a.ivertinimas AS Rating, 
                      a.data AS Date, 
                      v.vardas AS Name, 
                      v.pavarde AS Surname
                      FROM atsiliepimai a
                      JOIN vartotojai v ON a.fk_Vartotojas_id = v.id
                      WHERE a.id = @id
                  """;
        return await connection.QuerySingleOrDefaultAsync<Review?>(sql, new { id });
    }

    public async Task<IEnumerable<Review>> GetStopReviews(int stopId, int limit, int offset)
    {
        using var connection = _context.CreateConnection();

        var sql = """
                  SELECT 
                      a.id AS Id, 
                      a.fk_Vartotojas_id AS userId, 
                      a.fk_Stotele_id AS StopId, 
                      a.tekstas AS Content, 
                      a.ivertinimas AS Rating, 
                      a.data AS Date, 
                      v.vardas AS Name, 
                      v.pavarde AS Surname
                  FROM atsiliepimai a
                  JOIN vartotojai v ON a.fk_Vartotojas_id = v.id
                  WHERE a.fk_Stotele_id = @stopId
                  ORDER BY a.data DESC
                  LIMIT @limit
                  OFFSET @offset;
                  """;

        return await connection.QueryAsync<Review>(sql, new { stopId, limit, offset });
    }

    public async Task<int> GetStopReviewsCount(int stopId)
    {
        using var connection = _context.CreateConnection();

        var sql = """
                  SELECT COUNT(*)
                  FROM atsiliepimai
                  WHERE fk_Stotele_id = @stopId
                  """;

        return await connection.ExecuteScalarAsync<int>(sql, new { stopId });
    }


    public async Task Insert(Review review)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  INSERT INTO atsiliepimai (fk_Vartotojas_id, fk_Stotele_id, tekstas, ivertinimas, data)
                  VALUES (@UserId, @StopId, @Content, @Rating, @Date)
                  """;
        await connection.ExecuteAsync(sql, review);
    }

    public async Task<ReviewsSummary> GetStopReviewsSummary(int stopId)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  SELECT 
                      SUM(CASE WHEN ivertinimas = 1 THEN 1 ELSE 0 END) AS OneStar,
                      SUM(CASE WHEN ivertinimas = 2 THEN 1 ELSE 0 END) AS TwoStars,
                      SUM(CASE WHEN ivertinimas = 3 THEN 1 ELSE 0 END) AS ThreeStars,
                      SUM(CASE WHEN ivertinimas = 4 THEN 1 ELSE 0 END) AS FourStars,
                      SUM(CASE WHEN ivertinimas = 5 THEN 1 ELSE 0 END) AS FiveStars
                  FROM atsiliepimai 
                  WHERE fk_Stotele_id = @stopId
                  """;
        return await connection.QuerySingleAsync<ReviewsSummary>(sql, new { stopId });
    }

    public async Task Update(Review review)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  UPDATE atsiliepimai
                  SET
                    tekstas = @Content,
                    ivertinimas = @Rating,
                    data = @Date
                  WHERE id = @Id
                  """;
        await connection.ExecuteAsync(sql, review);
    }
}