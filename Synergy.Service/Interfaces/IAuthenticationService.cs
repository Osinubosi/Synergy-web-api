using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Synergy.Service.ApiResponse;
using Synergy.Service.ResponseData;
using Synergy.Service.ViewModel;

namespace Synergy.Service.Interfaces
{
    public interface IAuthenticationService
    {
        Task<Response<AuthenticationResponse>> LoginUserAsyn(LoginViewModel loginViewModel);

        Task<Response<string>> ActivateCustomerAccount(string encodedString);
    }
}
