using Synergy.Service.ApiResponse;
using Synergy.Service.ResponseData;
using Synergy.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.Service.Interfaces
{
    public interface ISynergySettings
    {
        Task<Response<string>> AddCountry(CountryViewModel request);
        Task<Response<List<CountryData>>> GetAllCountry();
        Task<Response<string>> GetHowYouHearAboutUs();
    }
}
