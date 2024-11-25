namespace TravelOrganization.Data.Models.Routes;

public class RouteStop {
    public required int Number { get; set; }
    public required double Distance { get; set; }
    public required int Time { get; set; }
    public required int StopId { get; set; }
    public required int RouteId { get; set; }
}