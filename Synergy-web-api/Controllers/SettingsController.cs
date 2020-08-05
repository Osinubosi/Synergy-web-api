using System.Collections.Generic;
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
using Synergy.Service.ResponseData;
using Synergy.Service.ViewModel;
using Synergy_web_api.Base;
using Synergy_web_api.Logging;
using Synergy_web_api.WebHandler;

namespace Synergy_web_api.Controllers
{
    [Route("api/settings")]
    [ApiController]
    [AllowAnonymous]
    public class SettingsController : BaseController
    {
        private readonly ISynergySettings _synergySettings;
        public SettingsController(IWebHostEnvironment env, IHttpContextAccessor httpContext, IMemoryCache memoryCache, ISynergySettings synergy) : base(env, httpContext, memoryCache)
        {
            _synergySettings = synergy;
        }


        [HttpPost("admin/country")]
        [RequestLog("request")]
        [ProducesResponseType((int)HttpStatusCode.OK, StatusCode = (int)HttpStatusCode.OK, Type = typeof(SuccessResponse<string>))]

        public async Task<IActionResult> AddCountry([FromBody] CountryViewModel request)
        {

            if (!ModelState.IsValid)
                return BadRequest(RequestResponseFormatter.BadRequestResponse(ModelState, "BadGettingStartedRequest", "InvalidGettingStartedRequest", RootPath));
            // return BadRequest();

            var response = await _synergySettings.AddCountry(request: request);

            if (response.Status.Equals(ResponseStatus.BadRequest))
                return BadRequest(response.ErrorData);

            if (response.Status.Equals(ResponseStatus.Conflict))
                return Conflict(response.ErrorData);

            if (response.Status.Equals(ResponseStatus.ServerError))
                return StatusCode(500, response.ErrorData);


            return Ok(response.SuccessData);
        }

        [HttpGet("admin/country")]
        [RequestLog("request")]
        [ProducesResponseType((int)HttpStatusCode.OK, StatusCode = (int)HttpStatusCode.OK, Type = typeof(SuccessResponse<List<CountryData>>))]

        public async Task<IActionResult> GetCountry()
        {

            var response = await _synergySettings.GetAllCountry();

            if (response.Status.Equals(ResponseStatus.BadRequest))
                return BadRequest(response.ErrorData);

            if (response.Status.Equals(ResponseStatus.Conflict))
                return Conflict(response.ErrorData);

            if (response.Status.Equals(ResponseStatus.ServerError))
                return StatusCode(500, response.ErrorData);


            return Ok(response.SuccessData);
        }

        [HttpGet("admin/tell-us-how")]
        [ProducesResponseType((int) HttpStatusCode.OK, StatusCode = (int) HttpStatusCode.OK,
            Type = typeof(SuccessResponse<string>))]
        public async Task<IActionResult> HowDoHearAboutUs()
        {
            var response = await _synergySettings.GetHowYouHearAboutUs();

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