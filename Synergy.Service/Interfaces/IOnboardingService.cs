using Synergy.Service.ApiResponse;
using Synergy.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.Service.Interfaces
{
    public interface IOnboardingService
    {
        Task<Response<string>> UserSignOn(RegisterUserViewmodel request);
    }
}
