using cmata.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace cmata.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly CountryService _countryService;

        public CountriesController(IHttpClientFactory httpClientFactory, CountryService countryService)
        {
            _httpClientFactory = httpClientFactory;
            _countryService = countryService;
        }

        [HttpGet]
        public async Task<IActionResult> GetCountryData(
            [FromQuery] string? param1 = null,
            [FromQuery] int param2 = 0,
            [FromQuery] string? param3 = null,
            [FromQuery] string? param4 = null)
        {
            try
            {
                // Create a dictionary to hold the query parameters.
                var queryParams = new Dictionary<string, string>();

                // Add parameters to the dictionary only if they have values.
                if (!string.IsNullOrEmpty(param1))
                {
                    queryParams.Add("param1", param1);
                }

                if (param2 != 0)
                {
                    queryParams.Add("param2", param2.ToString());
                }

                if (!string.IsNullOrEmpty(param3))
                {
                    queryParams.Add("param3", param3);
                }

                if (!string.IsNullOrEmpty(param4))
                {
                    queryParams.Add("param4", param4);
                }

                // Construct the query string from the dictionary.
                string query = string.Join("&", queryParams.Select(kv => $"{kv.Key}={kv.Value}"));

                // Create an HttpClient instance using the HttpClientFactory.
                var httpClient = _httpClientFactory.CreateClient();

                // Make a request to the REST Countries API.
                var response = await httpClient.GetAsync($"https://restcountries.com/v3.1/all?{query}");

                // Check if the request was successful.
                if (response.IsSuccessStatusCode)
                {
                    // Deserialize the response JSON to a variable/object.
                    var responseData = await response.Content.ReadFromJsonAsync<object>();

                    // You can now work with the responseData object.

                    return Ok(responseData);
                }
                else
                {
                    // Handle the error if the request was not successful.
                    return StatusCode((int)response.StatusCode, "Failed to retrieve data from the REST Countries API.");
                }
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during the request.
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpGet("filterCountriesByName")]
        public async Task<IActionResult> FilterCountriesByName([FromQuery] string search)
        {
            if (string.IsNullOrWhiteSpace(search))
            {
                return BadRequest("Search query is required.");
            }

            var filteredCountries = await _countryService.FilterCountriesByNameAsync(search);

            return Ok(filteredCountries);
        }

        [HttpGet("filterCountriesByPopulation")]
        public async Task<IActionResult> FilterCountriesByPopulation([FromQuery] long populationThreshold)
        {
            if (populationThreshold <= 0)
            {
                return BadRequest("Population threshold must be a positive number.");
            }

            var filteredCountries = await _countryService.FilterCountriesByPopulationAsync(populationThreshold);

            return Ok(filteredCountries);
        }

        [HttpGet("sortCountriesByName")]
        public async Task<IActionResult> SortCountriesByName([FromQuery] string sortOrder)
        {
            if (string.IsNullOrWhiteSpace(sortOrder) || (sortOrder != "ascend" && sortOrder != "descend"))
            {
                return BadRequest("Sort order must be 'ascend' or 'descend'.");
            }

            var countries = await _countryService.GetCountriesAsync();

            if (sortOrder == "ascend")
                countries = countries.OrderBy(c => c.Name.Common).ToList();
            else
                countries = countries.OrderByDescending(c => c.Name.Common).ToList();

            return Ok(countries);
        }

        [HttpGet("limitCountries")]
        public async Task<IActionResult> LimitCountries([FromQuery] int recordLimit)
        {
            if (recordLimit <= 0)
            {
                return BadRequest("Record limit must be a positive number.");
            }

            var countries = await _countryService.GetCountriesAsync();
            var limitedCountries = countries.Take(recordLimit).ToList();

            return Ok(limitedCountries);
        }

        [HttpGet("filterSortPaginate")]
        public async Task<IActionResult> FilterSortPaginateCountries([FromQuery] string search, [FromQuery] string sortOrder, [FromQuery] int page, [FromQuery] int pageSize)
        {
            var countries = await _countryService.GetCountriesAsync();

            // Apply filtering by name
            if (!string.IsNullOrWhiteSpace(search))
            {
                countries = countries.Where(c => c.Name.Common.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            // Apply sorting by name
            if (!string.IsNullOrWhiteSpace(sortOrder))
            {
                if (sortOrder == "ascend")
                {
                    countries = countries.OrderBy(c => c.Name.Common).ToList();
                }
                else if (sortOrder == "descend")
                {
                    countries = countries.OrderByDescending(c => c.Name.Common).ToList();
                }
            }

            // Apply pagination
            var totalCount = countries.Count;
            var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

            var paginatedCountries = countries.Skip((page - 1) * pageSize).Take(pageSize).ToList();

            var result = new
            {
                TotalCount = totalCount,
                TotalPages = totalPages,
                Page = page,
                PageSize = pageSize,
                Data = paginatedCountries
            };

            return Ok(result);
        }

    }
}
