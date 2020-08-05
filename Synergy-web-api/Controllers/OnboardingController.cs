using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Synergy.Service.ApiResponse;
using Synergy.Service.Enums;
using Synergy.Service.Interfaces;
using Synergy.Service.ViewModel;
using Synergy_web_api.Base;
using Synergy_web_api.Logging;
using Synergy_web_api.WebHandler;

namespace Synergy_web_api.Controllers
{
    [AllowAnonymous]
    [Route("api/registration")]
    [ApiController]
    public class OnboardingController : BaseController
    {
        private readonly IOnboardingService _onboarding;
        public OnboardingController(IWebHostEnvironment env, IHttpContextAccessor httpContext, IMemoryCache memoryCache, IOnboardingService onboardingService) : base(env, httpContext, memoryCache)
        {
            _onboarding = onboardingService;
        }

        
        [HttpPost("user")]
        [RequestLog("request")]
        [ProducesResponseType((int)HttpStatusCode.OK, StatusCode = (int)HttpStatusCode.OK, Type = typeof(SuccessResponse<string>))]

        public async Task<IActionResult> UserSignOn([FromBody] RegisterUserViewmodel request)
        {
            if (!ModelState.IsValid)
                return BadRequest(RequestResponseFormatter.BadRequestResponse(ModelState, "BadGettingStartedRequest", "InvalidGettingStartedRequest", RootPath));
            // return BadRequest();

            var response = await _onboarding.UserSignOn(request: request);

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