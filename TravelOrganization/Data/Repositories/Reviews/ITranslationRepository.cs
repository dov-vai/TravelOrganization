using TravelOrganization.Data.Models.Reviews;

namespace TravelOrganization.Data.Repositories.Reviews;

public interface ITranslationRepository
{
    Task<ReviewTranslation?> Get(int reviewId, string targetLanguageCode);
    Task Insert(ReviewTranslation translation);
    Task Delete(int reviewId);
}