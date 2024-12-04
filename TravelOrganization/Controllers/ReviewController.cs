using Microsoft.AspNetCore.Components;
using TravelOrganization.Data.Models.Reviews;
using TravelOrganization.Data.Services;

namespace TravelOrganization.Controllers;

public class ReviewController
{
    ReviewService _reviewService;
    NavigationManager _navigationManager;

    public ReviewController(ReviewService reviewService, NavigationManager navigationManager)
    {
        _reviewService = reviewService;
        _navigationManager = navigationManager;
    }

    public async Task<IEnumerable<Review>> GetReviews(int stopId, int page, int pageSize)
    {
        return await _reviewService.GetStopReviews(stopId, page, pageSize);
    }
    
    public void AddReview()
    {
        _navigationManager.NavigateTo("/ReviewForm");
    }
    
    public void DeleteReview()
    {
        _navigationManager.NavigateTo("/ReviewDelete");
    }

    public void EditReview()
    {
        _navigationManager.NavigateTo("/ReviewEdit");
    }
    
    
}