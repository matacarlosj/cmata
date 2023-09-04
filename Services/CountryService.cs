namespace cmata.Services
{
    using cmata.Models;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.Metrics;
    using System.Linq;
    using System.Net.Http;
    using System.Text.Json;
    using System.Threading.Tasks;

    public class CountryService
    {
        private readonly HttpClient _httpClient;

        public CountryService(HttpClient httpClient)
        {
            _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task<List<Country>> FilterCountriesByNameAsync(string search)
        {
            var countries = await GetCountriesAsync();

            return countries
                .Where(c => c.Name.Common.Contains(search, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        private async Task<List<Country>> GetCountriesAsync()
        {
            var response = await _httpClient.GetAsync("https://restcountries.com/v3.1/all");
            response.EnsureSuccessStatusCode();

            using var responseStream = await response.Content.ReadAsStreamAsync();
            var countries = await JsonSerializer.DeserializeAsync<List<Country>>(responseStream);
            return countries ?? new List<Country>();
        }
    }

}
