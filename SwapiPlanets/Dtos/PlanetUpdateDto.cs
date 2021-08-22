using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace SwapiPlanets.Dtos
{
    public class PlanetUpdateDto
    {
        [Required]
        public long? PlanetId { get; set; }
        [Required]
        public string Name { get; set; }
        public List<long> Species { get; set; } = new List<long>();
    }
}
