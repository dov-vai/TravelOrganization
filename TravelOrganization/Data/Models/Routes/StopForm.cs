using System.ComponentModel.DataAnnotations;

namespace TravelOrganization.Data.Models.Routes;

public class StopForm {
    public int Id { get; set; }

    [Required(ErrorMessage = "Pavadinimas negali būti tuščias.")]
    public string Name { get; set; } = null!;

    [Required]
    [Range(-90, 90, ErrorMessage = "Ilguma turi būti intervale nuo -90° iki 90°")]
    public double Longitude { get; set; }

    [Required]
    [Range(-90, 90, ErrorMessage = "Platuma turi būti turi būti intervale nuo -90° iki 90°")]
    public double Latitude { get; set; }

    public double Rating { get; set; }
}
