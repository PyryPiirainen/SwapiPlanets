using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DomainModels.Models
{
    public class Planet
    {
        public long PlanetId { get; set; }
        public string Name { get; set; }

        public List<PlanetSpecies> PlanetSpecies { get; set; } = new List<PlanetSpecies>();

        public IEnumerable<Species> Species => PlanetSpecies?.Select(s => s.Species);
    }
}
