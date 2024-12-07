namespace TravelOrganization.Data.Models.Reviews;

public class ReviewTranslation
{
    public int? Id { get; set; }
    public int? ReviewId { get; set; }
    public string? LanguageCode { get; set; }
    public string? Content { get; set; }
}