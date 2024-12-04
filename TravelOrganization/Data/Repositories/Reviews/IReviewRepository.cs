using TravelOrganization.Data.Models.Reviews;

namespace TravelOrganization.Data.Repositories.Reviews;

public interface IReviewRepository
{
    Task<IEnumerable<Review>> GetAll();
    Task Insert(Review review);
    Task<IEnumerable<Review>> GetStopReviews(int stopId, int limit, int offset);
}