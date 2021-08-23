using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModels.Models
{
    public class Species
    {
        public long SpeciesId { get; set; }
        public List<PlanetSpecies> PlanetSpecies { get; set; } = new List<PlanetSpecies>();
    }
}
