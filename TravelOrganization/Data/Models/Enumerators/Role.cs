using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using TravelOrganization.Data.Models.Account;

namespace TravelOrganization.Data.Models.Enumerators
{
    public class Role
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
    }
}
