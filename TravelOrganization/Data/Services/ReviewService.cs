using TravelOrganization.Data.Models.Reviews;
using TravelOrganization.Data.Repositories.Reviews;

namespace TravelOrganization.Data.Services;

public class ReviewService
{
    private readonly IReviewRepository _reviewRepository;
    private readonly ITranslationRepository _translationRepository;

    public ReviewService(IReviewRepository reviewRepository, ITranslationRepository translationRepository)
    {
        _reviewRepository = reviewRepository;
        _translationRepository = translationRepository;
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

    public async Task<ReviewForm?> GetReviewForm(int reviewId)
    {
        var review = await _reviewRepository.Get(reviewId);

        if (review == null)
            return null;
        
        return new ReviewForm
        {
            Id = review.Id!.Value,
            UserId = review.UserId,
            StopId = review.StopId,
            Rating = review.Rating,
            Content = review.Content,
        };
    }

    public async Task UpdateReview(ReviewForm reviewForm)
    {
        var review = await _reviewRepository.Get(reviewForm.Id);
        
        // ???
        if (review == null)
            return;
        
        await _reviewRepository.Update(new()
        {
            Id = reviewForm.Id,
            UserId = reviewForm.UserId,
            Rating = reviewForm.Rating,
            Content = reviewForm.Content!,
            Date = DateTime.Now,
            StopId = reviewForm.StopId
        });
        
        // delete translations if the content was updated
        if (reviewForm.Content != review.Content)
            await _translationRepository.Delete(reviewForm.Id);
    }
}