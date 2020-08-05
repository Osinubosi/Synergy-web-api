using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Synergy.Domain.Interfaces;
using Synergy.Repository.Interfaces;
using Synergy.Repository.Models;
using Synergy.Service.ApiResponse;
using Synergy.Service.Interfaces;
using Synergy.Service.ViewModel;
using System;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore;
using Synergy.Domain.ServiceModel;
using Synergy.Service.Constants;
using Synergy.Service.Helpers;

namespace Synergy.Service.Implementations
{
    public class OnboardingService : BaseService, IOnboardingService
    {
        
        
        public OnboardingService(IHostingEnvironment evn, ICryptographyService cryptographyService, IEmailService emailService, IConfiguration config, IDbContext dbContext) : base(evn, cryptographyService, emailService, config, dbContext)
        {
        }

        public async Task<Response<string>> UserSignOn(RegisterUserViewmodel request)
        {
            var customweAccountContext = DbContext.Set<CustomerAccount>();
            try
            {
                bool isUserExist = await customweAccountContext.AnyAsync(a => a.PhoneNumber.Equals(request.PhoneNumber) || a.EmailAddress.Equals(request.EmailAddress.ToLower()));

                if (isUserExist)
                    return new Response<string>
                    {
                        Status = Enums.ResponseStatus.Conflict,
                        ErrorData = new ErrorResponse<string>
                        {
                            Data = $"Either {request.EmailAddress} or {request.PhoneNumber} already exist"

                        }
                    };

                HashDetail encryptPassword = CryptographyService.GenerateHash(request.Password);
                await customweAccountContext.AddAsync(new CustomerAccount
                {
                    CountryId = request.CountryId,
                    DailingCode = request.DailingCode,
                    EmailAddress = request.EmailAddress.ToLower(),
                    FirstName = request.Firstname,
                    LastName = request.Lastname,
                    Password = encryptPassword.HashedValue,
                    PasswordKey = encryptPassword.Salt,
                    isEmailVerified = false,
                    PhoneNumber = request.PhoneNumber,
                    HowDoyouKnow = request.HowDoyouKnowUs
                });

                DbContext.SaveChanges();

                var baseUrl = GetBaseUrl("BaseEndpoint");
                var verify = GetBaseUrl("verifyUser");
                var encodeData = $"{request.EmailAddress}+{request.PhoneNumber}+{DateTime.Now.Date}";

                var maiilRequest = new EmailRequest
                {
                    Body = EmailFormatter.ConfirmEmail("",$"{baseUrl}{verify}?id={HttpUtility.UrlEncode(encodeData)}",DateTime.Now.Year.ToString(), ContentPath),
                    Subject = "Kindly verify your email address",
                    To = request.EmailAddress
                    
                };

                EmailService.SendEmail(maiilRequest);
                return new Response<string>
                {
                    Status = Enums.ResponseStatus.Success,
                    SuccessData = new SuccessResponse<string>
                    {
                        Data = "Account Created successfully, kindly check your email to verify your account",
                        ResponseCode = SuccessCode.DEFAULT_SUCCESS_CODE,
                        ResponseMessage = "",
                    }
                };
            }
            catch (Exception ex)
            {

                Log.Error(ex, "SendSMSVericationService");
                return new Response<string>
                {
                    Status = Enums.ResponseStatus.ServerError,
                    ErrorData = new ErrorResponse<string>
                    {
                        Data = "Error retrieving data, try again",
                        ResponseCode = ErrorCode.INTERNAL_SERVER_ERROR,
                        ResponseMessage = "",
                    }
                };
            }
        }
    }

}
