using DomainModels.Models;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace SwapiPlanets.Clients
{
    public class SwapiClient
    {
        private IHttpClientFactory _clientFactory;

        public SwapiClient(IHttpClientFactory clientFactory)
        {
            _clientFactory = clientFactory;
        }

        public async Task<bool> IsValidSpecies(IEnumerable<Species> species)
        {
            var bag = new ConcurrentBag<bool>();
            var tasks = species.Select(async item =>
            {
                bag.Add(await IsValidSpecies(item));
            });
            await Task.WhenAll(tasks);
            return bag.All(x => x);
        }

        public async Task<bool> IsValidSpecies(Species species)
        {
            HttpClient client = _clientFactory.CreateClient(Constants.Swapi);
            var request = new HttpRequestMessage(HttpMethod.Get, $"species/{species.SpeciesId}");
            var response = await client.SendAsync(request);
            
            if (response.StatusCode == HttpStatusCode.OK)
            {
                return true;
            }
            else if (response.StatusCode == HttpStatusCode.NotFound)
            {
                return false;
            }
            throw new InvalidOperationException("Failed to validate existence of entered species id.");
            
        }
    }
}
