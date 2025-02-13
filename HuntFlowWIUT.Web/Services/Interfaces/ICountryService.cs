using HuntFlowWIUT.Web.Models;

namespace HuntFlowWIUT.Web.Services.Interfaces
{
    public interface ICountryService
    {
        Task<List<Country>> GetCountriesAsync();
    }
}
