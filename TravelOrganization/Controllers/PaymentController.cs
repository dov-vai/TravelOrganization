using Microsoft.AspNetCore.Components;
using TravelOrganization.Data.Services;

namespace TravelOrganization.Controllers
{
    public class PaymentController
    {
        private readonly NavigationManager _navigationManager;
        public PaymentController(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public void ToCardCreation()
        {
            _navigationManager.NavigateTo("/ccreation");
        }
        public void ToCardDeleteVerification()
        {
            _navigationManager.NavigateTo("/cdverification");
        }
        public void ToCardView()
        {
            _navigationManager.NavigateTo("/cview");
        }
        public void ToOrderCreation()
        {
            _navigationManager.NavigateTo("/ocreation");
        }
        public void ToPayment()
        {
            _navigationManager.NavigateTo("/payment");
        }
        public void ToCart()
        {
            _navigationManager.NavigateTo("/cart");
        }
        public void ToProfile()
        {
            _navigationManager.NavigateTo("/profile");
        }
    }
}
