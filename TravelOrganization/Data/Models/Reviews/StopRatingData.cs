namespace TravelOrganization.Data.Models.Reviews;

public class StopRatingData
{
    public string Star { get; set; }
    public int Count { get; set; }
    
    public StopRatingData(string star, int count)
    {
        Star = star;
        Count = count;
    }
    
    public static List<StopRatingData> GetRatingData(ReviewsSummary summary)
    {
        return new List<StopRatingData>
        {
            new("1 žvaigždė", summary.OneStar),
            new("2 žvaigždės", summary.TwoStars),
            new("3 žvaigždės", summary.ThreeStars),
            new("4 žvaigždės", summary.FourStars),
            new("5 žvaigždės", summary.FiveStars)
        };
    }
}