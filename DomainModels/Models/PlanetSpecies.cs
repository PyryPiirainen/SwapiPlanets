using System;
using System.Collections.Generic;
using System.Text;

namespace DomainModels.Models
{
    public class PlanetSpecies   
    {
        public long PlanetId { get; set; }
        public long SpeciesId { get; set; }
        
        public Planet Planet { get; set; }
        public Species Species { get; set; }
    }
}
