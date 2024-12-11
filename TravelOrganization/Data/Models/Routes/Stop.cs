namespace TravelOrganization.Data.Models.Routes;

public class Stop {
    public int Id { get; set; }
    public required string Name { get; set; }
    public required double Longitude { get; set; }
    public required double Latitude { get; set; }
    public required double Rating { get; set; }
}
