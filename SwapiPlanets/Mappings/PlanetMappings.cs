using DomainModels.Models;
using SwapiPlanets.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwapiPlanets.Mappings
{
    public class PlanetMappings
    {
        public string GetSpeciesSwapiUrl(long speciesId)
        {
            return $"https://swapi.dev/api/species/{speciesId}/";
        }

        public Planet FromDto(PlanetCreateDto dto)
        {
            var planet = new Planet()
            {
                Name = dto.Name
            };

            var planetSpecies = dto.Species
                .Select(s =>
                {
                    var planetSpecies = new PlanetSpecies
                    {
                        Planet = planet,
                        SpeciesId = s,
                        Species = new Species
                        {
                            SpeciesId = s
                        }
                    };
                    planetSpecies.Species.PlanetSpecies.Add(planetSpecies);
                    return planetSpecies;
                })
                .ToList();
            planet.PlanetSpecies = planetSpecies;

            return planet;
        }

        public Planet FromDto(PlanetUpdateDto dto)
        {
            var planet = new Planet()
            {
                PlanetId = dto.PlanetId.Value,
                Name = dto.Name
            };

            var planetSpecies = dto.Species
                .Select(s =>
                {
                    var planetSpecies = new PlanetSpecies
                    {
                        PlanetId = dto.PlanetId.Value,
                        Planet = planet,
                        SpeciesId = s,
                        Species = new Species
                        {
                            SpeciesId = s
                        }
                    };
                    planetSpecies.Species.PlanetSpecies.Add(planetSpecies);
                    return planetSpecies;
                })
                .ToList();
            planet.PlanetSpecies = planetSpecies;

            return planet;
        }

        public PlanetReadDto ToDto(Planet planet)
        {
            var dto = new PlanetReadDto
            {
                PlanetId = planet.PlanetId,
                Name = planet.Name,
                Species = planet.Species
                    .Select(s => GetSpeciesSwapiUrl(s.SpeciesId))
                    .ToList()
            };
            return dto;
        }

        public IEnumerable<PlanetReadDto> ToDto(IEnumerable<Planet> planets)
        {
            return planets.Select(p => ToDto(p));
        }
    }
}
