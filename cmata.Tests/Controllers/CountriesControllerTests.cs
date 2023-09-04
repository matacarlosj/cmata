using Xunit;
using Moq;
using System.Collections.Generic;
using System.Xml.Linq;
using cmata.Services;
using cmata.Models;
using cmata.Controllers;
using Microsoft.AspNetCore.Mvc;

namespace cmata.Tests.Controllers
{
    public class CountriesControllerTests
    {
        [Fact]
        public async Task FilterCountriesByName_ReturnsFilteredCountries()
        {
            // Arrange
            var httpClient = new HttpClient();
            var mockCountryService = new Mock<CountryService>(httpClient);
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();

            mockCountryService.Setup(service => service.FilterCountriesByNameAsync("Sp"))
                .ReturnsAsync(new List<Country>
                {
                new Country { Name = new CountryObjectName { Common = "Spain" } },
                new Country { Name = new CountryObjectName { Common = "Sri Lanka" } }
                });

            var controller = new CountriesController(mockHttpClientFactory.Object, mockCountryService.Object);

            // Act
            var result = await controller.FilterCountriesByName("Sp");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var countries = Assert.IsType<List<Country>>(okResult.Value);
            Assert.Equal(2, countries.Count);
            Assert.Contains(countries, c => c.Name.Common == "Spain");
            Assert.Contains(countries, c => c.Name.Common == "Sri Lanka");
        }
        [Fact]
        public async Task FilterCountriesByPopulation_ReturnsFilteredCountriesByPopulation()
        {
            // Arrange
            var httpClient = new HttpClient();
            var mockCountryService = new Mock<CountryService>(httpClient);
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();

            mockCountryService.Setup(service => service.FilterCountriesByPopulationAsync(20))
                .ReturnsAsync(new List<Country>
                {
                new Country { Name = new CountryObjectName { Common = "Spain" }, Population = 46733038 },
                new Country { Name = new CountryObjectName { Common = "Sri Lanka" }, Population = 21413249 },
                new Country { Name = new CountryObjectName { Common = "Canada" }, Population = 37742154 }
                });

            var controller = new CountriesController(mockHttpClientFactory.Object, mockCountryService.Object);

            // Act
            var result = await controller.FilterCountriesByPopulation(20);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var countries = Assert.IsType<List<Country>>(okResult.Value);
            Assert.Equal(3, countries.Count);
            Assert.Contains(countries, c => c.Name.Common == "Spain");
            Assert.Contains(countries, c => c.Name.Common == "Sri Lanka");
            Assert.Contains(countries, c => c.Name.Common == "Canada");
        }

        [Fact]
        public async Task SortCountriesByName_ReturnsSortedCountries()
        {
            // Arrange
            var httpClient = new HttpClient();
            var mockCountryService = new Mock<CountryService>(httpClient);
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();

            mockCountryService.Setup(service => service.GetCountriesAsync())
                .ReturnsAsync(new List<Country>
                {
                new Country { Name = new CountryObjectName { Common = "Afghanistan" } },
                new Country { Name = new CountryObjectName { Common = "Åland Islands" } },
                new Country { Name = new CountryObjectName { Common = "Albania" } }
                });

            var controller = new CountriesController(mockHttpClientFactory.Object, mockCountryService.Object);

            // Act
            var result = await controller.SortCountriesByName("ascend");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var countries = Assert.IsType<List<Country>>(okResult.Value);
            Assert.Equal(3, countries.Count);
            Assert.Equal("Afghanistan", countries[0].Name.Common);
            Assert.Equal("Åland Islands", countries[1].Name.Common);
            Assert.Equal("Albania", countries[2].Name.Common);
        }
        [Fact]
        public async Task LimitCountries_ReturnsLimitedCountries()
        {
            // Arrange
            var httpClient = new HttpClient();
            var mockCountryService = new Mock<CountryService>(httpClient);
            var mockHttpClientFactory = new Mock<IHttpClientFactory>();

            mockCountryService.Setup(service => service.GetCountriesAsync())
                .ReturnsAsync(new List<Country>
                {
                new Country { Name = new CountryObjectName { Common = "Spain" } },
                new Country { Name = new CountryObjectName { Common = "Sri Lanka" } },
                new Country { Name = new CountryObjectName { Common = "Canada" } }
                });

            var controller = new CountriesController(mockHttpClientFactory.Object, mockCountryService.Object);

            // Act
            var result = await controller.LimitCountries(2);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var countries = Assert.IsType<List<Country>>(okResult.Value);
            Assert.Equal(2, countries.Count);
            Assert.Equal("Spain", countries[0].Name.Common);
            Assert.Equal("Sri Lanka", countries[1].Name.Common);
        }
    }

}
