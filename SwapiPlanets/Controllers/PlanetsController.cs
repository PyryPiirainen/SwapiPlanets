using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Services.Services;
using SwapiPlanets.Dtos;
using SwapiPlanets.Mappings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SwapiPlanets.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PlanetsController : ControllerBase
    {
        private readonly ILogger<PlanetsController> _logger;
        private readonly IPlanetRepository _planetRepository;
        private readonly PlanetMappings _mappings;

        public PlanetsController(
            ILogger<PlanetsController> logger,
            IPlanetRepository planetRepository,
            PlanetMappings mappings)
        {
            _logger = logger;
            _planetRepository = planetRepository;
            _mappings = mappings;
        }

        [HttpPost]
        public async Task<ActionResult<PlanetReadDto>> CreatePlanet([FromBody] PlanetCreateDto planetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var planet = _mappings.FromDto(planetDto);
            await _planetRepository.CreatePlanet(planet);
            await _planetRepository.SaveChangesAsync();

            return Ok();
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<PlanetReadDto>>> GetPlanets()
        {
            return Ok(_mappings.ToDto(await _planetRepository.GetPlanets()));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PlanetReadDto>> GetPlanet(long id)
        {
            var dbPlanet = await _planetRepository.GetPlanet(id);
            if (dbPlanet == null)
            {
                return NotFound();
            }

            return Ok(_mappings.ToDto(dbPlanet));
        }

        [HttpPut]
        public async Task<ActionResult<PlanetReadDto>> UpdatePlanet([FromBody] PlanetUpdateDto planetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest();
            }

            var planetUpdate = _mappings.FromDto(planetDto);

            var updatedPlanet = await _planetRepository.UpdatePlanet(planetUpdate);
            if (updatedPlanet == null)
            {
                return NotFound();
            }

            await _planetRepository.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<PlanetReadDto>> DeletePlanet(long id)
        {
            var toRemove = await _planetRepository.DeletePlanet(id);
            if (toRemove == null)
            {
                return NotFound();
            }
            await _planetRepository.SaveChangesAsync();
            return Ok(_mappings.ToDto(toRemove));
        }
    }
}
