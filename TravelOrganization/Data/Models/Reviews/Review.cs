namespace TravelOrganization.Data.Models.Reviews;

public class Review
{
    public int? Id { get; set; }
    public int UserId { get; set; }
    public string Name { get; set; }
    public string Surname { get; set; }
    public int StopId { get; set; }
    public int Rating { get; set; }
    public string Content { get; set; }
    public DateTime Date { get; set; }
}