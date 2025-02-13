using HuntFlowWIUT.Web.Models;
using HuntFlowWIUT.Web.Services.Interfaces;

namespace HuntFlowWIUT.Web.Services
{
    public class CountryService : ICountryService
    {
        // In a real application, you might fetch this from a database.
        public Task<List<Country>> GetCountriesAsync()
        {
            var countries = new List<Country>
        {
            new Country { Id = 1, Name = "United States" },
            new Country { Id = 2, Name = "Canada" },
            new Country { Id = 3, Name = "United Kingdom" },
            new Country { Id = 4, Name = "Uzbekistan" },
            // Add additional countries as needed.
        };

            return Task.FromResult(countries);
        }
    }
}
