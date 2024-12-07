using Dapper;
using TravelOrganization.Data.Models.Reviews;

namespace TravelOrganization.Data.Repositories.Reviews;

public class TranslationRepository : ITranslationRepository
{
    private readonly DataContext _context;

    public TranslationRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<ReviewTranslation?> Get(int reviewId, string targetLanguageCode)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                  SELECT 
                      id AS Id,
                      fk_Atsiliepimas_id AS ReviewId,
                      kalbos_kodas AS LanguageCode,
                      tekstas AS Content
                  FROM vertimai
                  WHERE fk_Atsiliepimas_id = @reviewId AND kalbos_kodas == @targetLanguageCode
                  """;
        return await connection.QuerySingleOrDefaultAsync<ReviewTranslation?>(sql,
            new { reviewId, targetLanguageCode });
    }

    public async Task Insert(ReviewTranslation translation)
    {
        using var connection = _context.CreateConnection();
        var sql = """
                    INSERT INTO vertimai (fk_Atsiliepimas_id, kalbos_kodas, tekstas)
                    VALUES (@ReviewId, @LanguageCode, @Content)
                  """;
        await connection.ExecuteAsync(sql, translation);
    }
}