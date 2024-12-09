using TravelOrganization.Data.Models.Reviews;
using TravelOrganization.Data.Repositories.Reviews;

namespace TravelOrganization.Data.Services;

public class ReviewService
{
    private readonly IReviewRepository _reviewRepository;

    public ReviewService(IReviewRepository reviewRepository)
    {
        _reviewRepository = reviewRepository;
    }

    public async Task<IEnumerable<Review>> GetStopReviews(int stopId, int page, int pageSize)
    {
        var offset = (page - 1) * pageSize;
        return await _reviewRepository.GetStopReviews(stopId, pageSize, offset);
    }

    public async Task<int> GetStopReviewsPages(int stopId, int pageSize)
    {
        var count = await _reviewRepository.GetStopReviewsCount(stopId);
        return (count + pageSize - 1) / pageSize;
    }

    public async Task AddReview(ReviewForm reviewForm)
    {
        await _reviewRepository.Insert(new Review
        {
            UserId = reviewForm.UserId,
            Rating = reviewForm.Rating,
            Date = DateTime.Now,
            StopId = reviewForm.StopId,
            Content = reviewForm.Content!
        });
    }

    public async Task<ReviewsSummary?> GetReviewsSummary(int stopId)
    {
        // it always returns a value because the sums would default to 0
        // but if they're all 0, just return null, because no meaningful data was found
        var summary = await _reviewRepository.GetStopReviewsSummary(stopId);
        if (summary is { OneStar: 0, TwoStars: 0, ThreeStars: 0, FourStars: 0, FiveStars: 0 })
        {
            return null;
        }

        return summary;
    }
}