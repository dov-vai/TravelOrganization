namespace TravelOrganization.Data.Models.Routes;

public class Route {
    public int Id { get; set; }
    public required string Name { get; set; }
    public required double Rate { get; set; }
}
