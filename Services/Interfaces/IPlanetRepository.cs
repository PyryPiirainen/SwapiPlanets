using DomainModels.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Services
{
    public interface IPlanetRepository
    {
        Task<Planet> CreatePlanet(Planet planet);
        Task<IEnumerable<Planet>> GetPlanets();
        Task<Planet> GetPlanet(long id);
        Task<Planet> UpdatePlanet(Planet updatePlanet);
        Task<Planet> DeletePlanet(long id);

        Task<int> SaveChangesAsync();
    }
}