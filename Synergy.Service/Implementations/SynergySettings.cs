using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Synergy.Domain.Interfaces;
using Synergy.Repository.Interfaces;
using Synergy.Repository.Models;
using Synergy.Service.Interfaces;
using Synergy.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Synergy.Service.ApiResponse;
using Synergy.Service.ResponseData;
using Synergy.Service.Constants;
using Synergy.Service.Enums;

namespace Synergy.Service.Implementations
{
    public class SynergySettings : BaseService, ISynergySettings
    {
        public SynergySettings(IHostingEnvironment evn, ICryptographyService cryptographyService, IEmailService emailService, IConfiguration config, IDbContext dbContext) : base(evn, cryptographyService, emailService, config, dbContext)
        {
        }

        public async Task<Response<string>> AddCountry(CountryViewModel request)
        {
            var countryContext = DbContext.Set<Country>();
            try
            {

                await countryContext.AddAsync(new Country
                {
                    CountryName = request.CountryName,
                    CountryShortCode = request.CountryCode,
                    DailingCode = request.DailingCode
                });

                DbContext.SaveChanges();

                return new Response<string>
                {
                    Status = Enums.ResponseStatus.Success,
                    SuccessData = new SuccessResponse<string>
                    {
                        Data = "A new country have been added!!!",
                        ResponseCode = SuccessCode.DEFAULT_SUCCESS_CODE,
                        ResponseMessage = "Successful"
                    }
                };
            }
            catch (Exception  ex)
            {

                Log.Error(ex,"");
                return new Response<string>
                {
                    Status = Enums.ResponseStatus.Conflict,
                    ErrorData = new ErrorResponse<string>
                    {
                        Data = null,
                        ResponseCode = ErrorCode.UNIQUE_IDENTITY_VIOLATION,
                        ResponseMessage = ErrorCode.UNIQUE_IDENTITY_VIOLATION
                    }
                };
            }
        }

        public async Task<Response<List<CountryData>>> GetAllCountry()
        {
            var countryContext = DbContext.Set<Country>();
            var countries = new List<CountryData>();
            try
            {
                var countriesQuery = await countryContext.ToListAsync();
                if (countriesQuery == null)
                    return new Response<List<CountryData>>
                    {
                        Status = Enums.ResponseStatus.Conflict,
                        ErrorData = new ErrorResponse<List<CountryData>>
                        {
                            Data = null,
                            ResponseCode = ErrorCode.NOT_FOUND,
                            ResponseMessage = "Can not get country at the moment try again"
                        }
                    };

                foreach (var item in countriesQuery)
                {
                    countries.Add(new CountryData
                    {
                        CountryName = item.CountryName,
                        DailingCode = item.DailingCode,
                        Id = item.CountryId,
                        ShortCode = item.CountryShortCode
                    });
                }

                return new Response<List<CountryData>>
                {
                    Status = Enums.ResponseStatus.Success,
                    SuccessData = new SuccessResponse<List<CountryData>>
                    {
                        Data = countries,
                        ResponseCode = SuccessCode.DEFAULT_SUCCESS_CODE,
                        ResponseMessage = "Request successful"
                    }
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetAllCountry");
                return new Response<List<CountryData>>
                {
                    Status = Enums.ResponseStatus.Conflict,
                    ErrorData = new ErrorResponse<List<CountryData>>
                    {
                        Data = null,
                        ResponseCode = ErrorCode.NOT_FOUND,
                        ResponseMessage = "Can not get country at the moment try again"
                    }
                };
            }
        }

        public async Task<Response<string>> GetHowYouHearAboutUs()
        {
            try
            {
                var enumList = Enum.GetNames(typeof(InformationMetaDataEnum)).ToList();

                if (enumList.Count > 0)
                    return new Response<string>
                    {
                        Status = ResponseStatus.Success,
                        SuccessData = new SuccessResponse<string>
                        {
                            ResponseCode = SuccessCode.DEFAULT_SUCCESS_CODE,
                            Status = ResponseStatus.Success,
                            Data = JsonConvert.SerializeObject(enumList),
                            ResponseMessage = "Data request successful"
                        }
                    };
                return new Response<string>
                {
                    Status = ResponseStatus.NotFound,
                    ErrorData = new ErrorResponse<string>
                    {
                      ResponseCode = ErrorCode.NOT_FOUND,
                      ResponseMessage = "Request Not Found",
                      Data = null
                    }
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "GetAllCountry");
                return new Response<string>
                {
                    Status = Enums.ResponseStatus.Conflict,
                    ErrorData = new ErrorResponse<string>
                    {
                        Data = null,
                        ResponseCode = ErrorCode.NOT_FOUND,
                        ResponseMessage = "Can not get country at the moment try again"
                    }
                };
            }
        }
    }
}
