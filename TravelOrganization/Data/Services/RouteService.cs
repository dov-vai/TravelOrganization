using TravelOrganization.Data.Models.Routes;
using TravelOrganization.Data.Repositories.Routes;

namespace TravelOrganization.Data.Services;

public class RouteService
{
    private readonly RouteRepository _routeRepository;
    private readonly RouteStopRepository _routeStopRepository;
    private readonly StopRepository _stopRepository;

    public RouteService(RouteRepository routeRepository, RouteStopRepository routeStopRepository, StopRepository stopRepository) {
        _routeRepository = routeRepository;
        _routeStopRepository = routeStopRepository;
        _stopRepository = stopRepository;
    }

    public async Task<IEnumerable<Stop>> GetStops() {
        return await _stopRepository.GetAll();
    }

    public async Task AddStop(StopForm form) {
        await _stopRepository.Insert(new Stop {
            Name = form.Name,
            Longitude = form.Longitude,
            Latitude = form.Latitude,
            Rating = form.Rating
        });
    }

    public async Task UpdateStop(StopForm form) {
        await _stopRepository.Update(new Stop {
            Id = form.Id,
            Name = form.Name,
            Longitude = form.Longitude,
            Latitude = form.Latitude,
            Rating = form.Rating
        });
    }

    public async Task DeleteStop(int id) {
        await _stopRepository.Delete(id);
    }

    public async Task<IEnumerable<Models.Routes.Route>> GetRoutes() {
        return await _routeRepository.GetAll();
    }

    public async Task<Models.Routes.Route> GetRoute(int routeId) {
        return await _routeRepository.Get(routeId);
    }

    public async Task<IEnumerable<RouteStop>> GetRouteStops(int routeId) {
        return await _routeStopRepository.GetAll(routeId);
    }

    private double Deg2Rad(double degrees) {
        return degrees * (Math.PI/180);
    }

    private double Distance(double lat1, double lon1, double lat2, double lon2) {
        var R = 6371;
        var dLat = Deg2Rad(lat2 - lat1);
        var dLon = Deg2Rad(lon2 - lon1);

        var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                Math.Cos(Deg2Rad(lat1)) * Math.Cos(Deg2Rad(lat2)) *
                Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
        var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1-a));

        return  R * c;
    }

    public async Task AddRouteStops(int routeId, IList<Stop> stops) {
        for (int i = 0; i < stops.Count(); i++) {
            Stop stop = stops[i];

            double distance = 0;
            int time = 0;

            if (i + 1 < stops.Count()) {
                var nextStop = stops[i + 1];

                distance = Distance(stop.Latitude, stop.Longitude, nextStop.Latitude, nextStop.Longitude);
                time = (int)(distance / 100 * 3600);
            }

            await _routeStopRepository.Insert(new RouteStop {
                RouteId = routeId,
                StopId = stop.Id,
                Number = i,
                Distance = distance,
                Time = time
            });
        }
    }

    public async Task AddRoute(RouteDataForm routeForm, IList<Stop> routeStops) {
        var route = new Models.Routes.Route {
            Name = routeForm.Name,
            Rate = routeForm.Rate
        };

        route.Id = await _routeRepository.Insert(route);

        await AddRouteStops(route.Id, routeStops);
    }

    public async Task UpdateRoute(RouteDataForm routeForm, IList<Stop> routeStops) {
        await _routeRepository.Update(new Models.Routes.Route {
            Id = routeForm.Id,
            Name = routeForm.Name,
            Rate = routeForm.Rate
        });

        await _routeStopRepository.Delete(routeForm.Id);

        await AddRouteStops(routeForm.Id, routeStops);
    }

    public async Task DeleteRoute(int routeId) {
        await _routeStopRepository.Delete(routeId);
        await _routeRepository.Delete(routeId);
    }
}
