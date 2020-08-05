using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Synergy.Service.Interfaces;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Synergy.Service.ApiResponse;
using Synergy.Service.Enums;
using Synergy.Service.ViewModel;
using Synergy_web_api.Base;
using Synergy_web_api.WebHandler;

namespace Synergy_web_api.Controllers
{
    [AllowAnonymous]
    [Route("api/v1/authenticate")]
    [ApiController]
    public class AuthenticationController : BaseController
    {
        private readonly IAuthenticationService _authenticationService;
        public AuthenticationController(IWebHostEnvironment env, IHttpContextAccessor httpContext, IMemoryCache memoryCache, IAuthenticationService authentication) : base(env, httpContext, memoryCache)
        {
            _authenticationService = authentication;
        }



        [HttpPost("login")]
        [ProducesResponseType((int)HttpStatusCode.OK, StatusCode = (int)HttpStatusCode.OK, Type = typeof(SuccessResponse<string>))]
        public async Task<IActionResult> Login(LoginViewModel login)

        {
            if (!ModelState.IsValid)
                return BadRequest(RequestResponseFormatter.BadRequestResponse(ModelState, "BadLoginRequest",
                    "InvalidLoginRequest", RootPath));


            var response = await _authenticationService.LoginUserAsyn(login);

            if (response.Status == ResponseStatus.Unauthorized)
                return Unauthorized(response.ErrorData);

            if (response.Status.Equals(ResponseStatus.BadRequest))
                return BadRequest(response.ErrorData);

            if (response.Status.Equals(ResponseStatus.Conflict))
                return Conflict(response.ErrorData);

            if (response.Status.Equals(ResponseStatus.ServerError))
                return StatusCode(500, response.ErrorData);


            return Ok(response.SuccessData);

           
        }
    }
}