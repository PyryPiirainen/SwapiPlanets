using Database;
using Database.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Services
{
    public class PlanetRepository : IPlanetRepository
    {
        private SwapiPlanetsDbContext _dbContext;

        public PlanetRepository(SwapiPlanetsDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<Planet> CreatePlanet(Planet planet)
        {
            _dbContext.Planets.Add(planet);

            var species = new List<Species>();
            foreach (var planetSpecies in planet.PlanetSpecies)
            {
                _dbContext.PlanetSpecies.Add(planetSpecies);
                _dbContext.Attach(planetSpecies.Species); //otherwise EF tries to ad pre-existing ones
                species.Add(planetSpecies.Species);
            }

            List<Species> existingSpecies = await GetPreExistingSpecies(species);
            IEnumerable<Species> missingSpecies = GetMissingSpecies(species, existingSpecies);
            foreach (var nonExisting in missingSpecies)
            {
                _dbContext.Species.Add(nonExisting);
            }

            return planet;
        }


        public async Task<IEnumerable<Planet>> GetPlanets()
        {
            return await _dbContext
                .Planets
                .Include(p => p.PlanetSpecies)
                .ThenInclude(ps => ps.Species)
                .ToListAsync();
        }

        public async Task<Planet> GetPlanet(long id)
        {
            return await _dbContext
                .Planets
                .Where(p => p.PlanetId == id)
                .Include(p => p.PlanetSpecies)
                .ThenInclude(ps => ps.Species)
                .SingleOrDefaultAsync();
        }

        public async Task<Planet> UpdatePlanet(Planet updatePlanet)
        {
            var existingPlanet = await GetPlanet(updatePlanet.PlanetId);
            if (existingPlanet == null)
            {
                return null;
            }

            existingPlanet.Name = updatePlanet.Name;

            var unneededSpecies = GetUnneededSpecies(updatePlanet.Species, existingPlanet.Species);
            RemoveSpeciesFromPlanet(existingPlanet, unneededSpecies);

            AddMissingSpeciesToPlanet(updatePlanet, existingPlanet);

            await AddMissingSpeciesToDb(updatePlanet);

            return existingPlanet;
        }

        private void AddMissingSpeciesToPlanet(Planet updatePlanet, Planet existingPlanet)
        {
            var missingSpecies = GetMissingSpecies(updatePlanet.Species, existingPlanet.Species);
            var missingPlanetSpecies = updatePlanet.PlanetSpecies
                .Where(ps => missingSpecies.Select(ms => ms.SpeciesId).Contains(ps.SpeciesId))
                .ToList();
            foreach (var missing in missingPlanetSpecies)
            {
                var planetSpecies = new PlanetSpecies
                {
                    SpeciesId = missing.SpeciesId,
                    PlanetId = missing.PlanetId
                };
                planetSpecies.Planet = existingPlanet;
                _dbContext.PlanetSpecies.Add(planetSpecies);
            }
        }

        public async Task<Planet> DeletePlanet(long id)
        {
            var planet = await GetPlanet(id);
            if (planet != null)
            {
                _dbContext
                    .Planets
                    .Remove(planet);
            }
            return planet;
        }

        public Task<int> SaveChangesAsync()
        {
            return _dbContext.SaveChangesAsync();
        }


        /// <summary>
        /// Check which species are not in DB yet, but should be.
        /// </summary>
        /// <param name="speciesNeeded"></param>
        /// <returns></returns>
        private async Task<List<Species>> GetPreExistingSpecies(IEnumerable<Species> speciesNeeded)
        {
            return await _dbContext.Species
                .Where(dbSpecies => speciesNeeded.Select(s => s.SpeciesId).Contains(dbSpecies.SpeciesId))
                .ToListAsync();
        }

        private IEnumerable<Species> GetMissingSpecies(IEnumerable<Species> neededSpecies, IEnumerable<Species> existingSpecies)
        {
            return neededSpecies.Where(s => !existingSpecies.Select(e => e.SpeciesId).Contains(s.SpeciesId));
        }

        private IEnumerable<Species> GetUnneededSpecies(IEnumerable<Species> neededSpecies, IEnumerable<Species> existingSpecies)
        {
            return existingSpecies.Where(es => !neededSpecies.Select(ns => ns.SpeciesId).Contains(es.SpeciesId));
        }

        private async Task AddMissingSpeciesToDb(Planet updatePlanet)
        {
            var preExistingSpeciesDb = await GetPreExistingSpecies(updatePlanet.Species);
            var missingSpeciesDb = GetMissingSpecies(updatePlanet.Species, preExistingSpeciesDb);
            foreach (var missingDb in missingSpeciesDb)
            {
                _dbContext.Species.Add(new Species { SpeciesId = missingDb.SpeciesId });
            }
        }

        private void RemoveSpeciesFromPlanet(Planet existingPlanet, IEnumerable<Species> speciesToRemove)
        {
            var speciesIdsToRemove = speciesToRemove.Select(s => s.SpeciesId);
            var planetSpeciesToRemove = existingPlanet
                            .PlanetSpecies.Where(ps => speciesIdsToRemove.Contains(ps.SpeciesId))
                            .ToList();
            foreach (var planetSpecies in planetSpeciesToRemove)
            {
                planetSpecies.Planet.PlanetSpecies.Remove(planetSpecies);
                planetSpecies.Species.PlanetSpecies.Remove(planetSpecies);
                _dbContext.PlanetSpecies.Remove(planetSpecies);
            }
        }
    }
}
