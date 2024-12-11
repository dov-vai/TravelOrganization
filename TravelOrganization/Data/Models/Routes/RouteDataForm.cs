using System.ComponentModel.DataAnnotations;

namespace TravelOrganization.Data.Models.Routes;

public class RouteDataForm {
    public int Id { get; set; }

    [Required(ErrorMessage = "Pavadinimas negali būti tuščias.")]
    public string Name { get; set; } = null!;

    public double Rate { get; set; }
}
