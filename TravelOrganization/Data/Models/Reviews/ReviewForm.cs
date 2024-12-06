using System.ComponentModel.DataAnnotations;

namespace TravelOrganization.Data.Models.Reviews;

public class ReviewForm
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int StopId { get; set; }

    [Required(ErrorMessage = "Privaloma pasirinkti įvertinimą.")]
    [Range(1, 5)]
    public int Rating { get; set; }

    [Required(ErrorMessage = "Atsiliepimas negali būti tuščias.")]
    [StringLength(255, ErrorMessage = "Atsiliepimas negali būti ilgesnis nei 255 simboliai.")]
    public string? Content { get; set; }
}