namespace TravelOrganization.Controllers;

using Microsoft.AspNetCore.Components;
using TravelOrganization.Data.Services;
using TravelOrganization.Data.Models.Routes;

public class RouteController
{
    private readonly NavigationManager _navigationManager;
    private readonly RouteService _routeService;

    public RouteController(NavigationManager navigationManager, RouteService routeService) {
        _navigationManager = navigationManager;
        _routeService = routeService;
    }

    public async Task<IEnumerable<Stop>> GetStops() {
        return await _routeService.GetStops();
    }

    public async Task AddStop(StopForm form) {
        await _routeService.AddStop(form);
    }

    public async Task UpdateStop(StopForm form) {
        await _routeService.UpdateStop(form);
    }

    public async Task DeleteStop(int id) {
        await _routeService.DeleteStop(id);
    }

    public async Task<IEnumerable<Route>> GetRoutes() {
        return await _routeService.GetRoutes();
    }

    public async Task<Route> GetRoute(int routeId) {
        return await _routeService.GetRoute(routeId);
    }

    public async Task AddRoute(RouteDataForm routeForm, IList<Stop> routeStops) {
        await _routeService.AddRoute(routeForm, routeStops);

        _navigationManager.NavigateTo("/routes");
    }

    public async Task UpdateRoute(RouteDataForm routeForm, IList<Stop> routeStops) {
        await _routeService.UpdateRoute(routeForm, routeStops);

        _navigationManager.NavigateTo("/routes");
    }

    public void AddRoute() {
        _navigationManager.NavigateTo("/routes/add");
    }

    public void EditRoute(int routeId) {
        _navigationManager.NavigateTo($"/routes/{routeId}/edit");
    }

    public void DeleteRoute(int routeId) {
        _navigationManager.NavigateTo($"/routes/{routeId}/delete");
    }

    public async Task ConfirmDeleteRoute(int routeId) {
        await _routeService.DeleteRoute(routeId);

        _navigationManager.NavigateTo("/routes");
    }

    public void CancelDeleteRoute() {
        _navigationManager.NavigateTo("/routes");
    }

    public async Task<IEnumerable<RouteStop>> GetRouteStops(Route route) {
        return await _routeService.GetRouteStops(route.Id);
    }
}
