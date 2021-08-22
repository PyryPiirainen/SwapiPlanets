using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwapiPlanets.Dtos
{
    public class PlanetReadDto
    {
        public long PlanetId { get; set; }
        public string Name { get; set; }
        public List<string> Species { get; set; } = new List<string>();
    }
}
