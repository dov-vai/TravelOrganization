using Microsoft.AspNetCore.Components;
using TravelOrganization.Data.Models.Reviews;
using TravelOrganization.Data.Services;

namespace TravelOrganization.Controllers;

public class ReviewController
{
    private readonly NavigationManager _navigationManager;
    private readonly ReviewService _reviewService;

    public ReviewController(ReviewService reviewService, NavigationManager navigationManager)
    {
        _reviewService = reviewService;
        _navigationManager = navigationManager;
    }

    public async Task<IEnumerable<Review>> GetReviews(int stopId, int page, int pageSize)
    {
        return await _reviewService.GetStopReviews(stopId, page, pageSize);
    }

    public async Task<int> GetStopReviewPages(int stopId, int pageSize)
    {
        return await _reviewService.GetStopReviewsPages(stopId, pageSize);
    }

    public void AddReview()
    {
        _navigationManager.NavigateTo("/ReviewForm/1");
    }

    public void DeleteReview()
    {
        _navigationManager.NavigateTo("/ReviewDelete");
    }

    public void EditReview()
    {
        _navigationManager.NavigateTo("/ReviewEdit");
    }

    public async Task SubmitReview(ReviewForm reviewForm)
    {
        await _reviewService.AddReview(reviewForm);
        _navigationManager.NavigateTo("/ReviewSuccess/1");
    }
}