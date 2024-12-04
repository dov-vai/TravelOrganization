using TravelOrganization.Data.Models.Reviews;
using TravelOrganization.Data.Repositories.Reviews;

namespace TravelOrganization.Data.Services;

public class ReviewService
{
    IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }


    public async Task<IEnumerable<Review>> GetStopReviews(int stopId, int page, int pageSize)
    {
        int offset = (page - 1) * pageSize;
        return await _reviewRepository.GetStopReviews(stopId, pageSize, offset);
    }
    
}