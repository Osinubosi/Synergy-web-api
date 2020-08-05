using System;
using System.Threading.Tasks;
using System.Web;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Synergy.Domain.Interfaces;
using Synergy.Domain.ServiceModel;
using Synergy.Repository.Interfaces;
using Synergy.Repository.Models;
using Synergy.Service.ApiResponse;
using Synergy.Service.Constants;
using Synergy.Service.Enums;
using Synergy.Service.Helpers;
using Synergy.Service.Interfaces;
using Synergy.Service.ResponseData;
using Synergy.Service.ViewModel;

namespace Synergy.Service.Implementations
{
  public  class AuthenticationService : BaseService,IAuthenticationService
    {
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;
        public AuthenticationService(IOptions<JwtIssuerOptions> jwtOptions,IJwtFactory jwtFactory,IHostingEnvironment evn, ICryptographyService cryptographyService, IEmailService emailService, IConfiguration config, IDbContext dbContext) : base(evn, cryptographyService, emailService, config, dbContext)
        {
            _jwtFactory = jwtFactory;
            _jwtOptions = jwtOptions.Value;
        }

        public async Task<Response<string>> ActivateCustomerAccount(string encodedString)
        {
            var activateCustomerAccountContext = DbContext.Set<CustomerAccount>();
            try
            {
                var decodeString = HttpUtility.UrlDecode(encodedString);
                var queryString = decodeString.Split("+");

                if (queryString.Length != 3)
                    return new Response<string>
                    {
                        ErrorData = new ErrorResponse<string>
                        {
                            ResponseMessage = "User account can not be active, kindly contact the administrator",
                            ResponseCode = ErrorCode.INVALID_CLIENT_REQUEST,
                            Data = null,
                        }
                    };
                var activationTime = DateTime.Parse(queryString[2]).Date;

                var isActive = DateTime.Now.Date.Subtract(activationTime).Days == 0 ;

                if (!isActive)
                    return new Response<string>
                    {
                        ErrorData = new ErrorResponse<string>
                        {
                            ResponseMessage =
                                "The activation has expire kindly login to get a new activation link to activate your account",
                            ResponseCode = ErrorCode.FAILED_EMAIL_VERIFICATION,
                            Data = null
                        }
                    };
                
                
                var emailAddress = queryString[0];
                var phonenumber = queryString[1];

                var emailverifcation = await activateCustomerAccountContext.FirstOrDefaultAsync(a =>
                    a.EmailAddress == emailAddress && a.PhoneNumber == phonenumber);

                if (emailverifcation == null)
                    return new Response<string>
                    {
                        ErrorData = new ErrorResponse<string>
                        {
                            ResponseCode = ErrorCode.NOT_FOUND,
                            ResponseMessage = "Account does not exist",
                            Data = null
                        }
                    };

                emailverifcation.isEmailVerified = true;
                DbContext.SaveChanges();

                return new Response<string>
                {
                    Status = ResponseStatus.Success,
                    SuccessData = new SuccessResponse<string>
                    {
                        ResponseMessage =
                            "Your Account has been activated kindly procced to the login page to access your account",
                        ResponseCode = SuccessCode.DEFAULT_SUCCESS_CODE,
                        Status = ResponseStatus.Success,
                        Data = null,
                    }
                };

            }
            catch (Exception ex)
            {
                Log.Error(ex, "ActivateCustomerAccount");
                return new Response<string>
                {
                    ErrorData = new ErrorResponse<string>
                    {
                        ResponseCode = ErrorCode.INTERNAL_SERVER_ERROR,
                        ResponseMessage =
                            "Sorry, we are unable to complete this operation at the moment, kindly try again later"
                    }
                };
            }
        }

        public async Task<Response<AuthenticationResponse>> LoginUserAsyn(LoginViewModel loginViewModel)
        {
            var loginUserContext = DbContext.Set<CustomerAccount>().Include(a => a.Country);
            try
            {
                var customerAccount =
                    await loginUserContext.FirstOrDefaultAsync(a => a.EmailAddress == loginViewModel.UserName);

                if (customerAccount == null)
                    return new Response<AuthenticationResponse>
                    {
                        Status = ResponseStatus.NotFound,
                        ErrorData = new ErrorResponse<AuthenticationResponse>
                        {
                            Data = null,
                            ResponseCode = ErrorCode.NOT_FOUND,
                            ResponseMessage = "Account does not exist, kindly user the get started to sign on"
                        }
                    };

                bool isPassword = CryptographyService.ValidateHash(loginViewModel.Password, customerAccount.PasswordKey, customerAccount.Password);
                if (!isPassword)
                    return new Response<AuthenticationResponse>
                    {
                        Status = ResponseStatus.Unauthorized, 
                        ErrorData = new ErrorResponse<AuthenticationResponse>
                        {
                            ResponseCode = ErrorCode.UNAUTHORIZED_USER_ACCESS,
                            ResponseMessage = "Either email address or password ins invalid",
                            Data = null

                        }
                    };

                if (!customerAccount.isEmailVerified)
                {
                    var baseUrl = GetBaseUrl("BaseEndpoint");
                    var verify = GetBaseUrl("verifyUser");
                    var encodeData = $"{customerAccount.EmailAddress} + {customerAccount.PhoneNumber}+{DateTime.Now.Date}";

                    var maiilRequest = new EmailRequest
                    {
                        Body = EmailFormatter.ConfirmEmail("", $"{baseUrl}{verify}?id={HttpUtility.UrlEncode(encodeData)}", DateTime.Now.Year.ToString(), ContentPath),
                        Subject = "Kindly verify your email address",
                        To = customerAccount.EmailAddress

                    };

                    EmailService.SendEmail(maiilRequest);

                    return new Response<AuthenticationResponse>
                    {
                        ErrorData = new ErrorResponse<AuthenticationResponse>
                        {
                            ResponseMessage =
                                "Account is in-active kindly check your email for activation link to activate your account",
                            ResponseCode = ErrorCode.UNAUTHORIZED_USER_ACCESS,
                            Data = null

                        }
                    };
                }

                var identity = _jwtFactory.GenerateClaimsIdentity(loginViewModel.UserName, customerAccount.Id.ToString());
                var jwtToken = await Domain.DomainUtility.Utility.GenerateJwt(identity, _jwtFactory, customerAccount.EmailAddress, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.None });

                var authToken = JsonConvert.DeserializeObject<TokenModel>(jwtToken);

                var data = new AuthenticationResponse
                {
                    Token = authToken,
                    CountryDetailLite = new CountryDetailLite
                    {
                        CountryCode = customerAccount.Country.CountryShortCode,
                        CountryId = customerAccount.CountryId,
                        CountryName = customerAccount.Country.CountryName,
                        DailingCode = customerAccount.Country.DailingCode
                    },
                    UserDetail = new UserDetail
                    {
                        EmailAddress = loginViewModel.UserName,
                        Status = !customerAccount.isEmailVerified ? "Inactive" : "Active",
                        FirstName = customerAccount.FirstName,
                        LastName = customerAccount.LastName,
                        Selfie = "",
                        UserId = customerAccount.Id.ToString()
                    }
                };

                return new Response<AuthenticationResponse>
                {
                    Status = ResponseStatus.Success,
                    SuccessData = new SuccessResponse<AuthenticationResponse>
                    {
                        ResponseCode = SuccessCode.DEFAULT_SUCCESS_CODE,
                        ResponseMessage = "User Login successfully",
                        Status = ResponseStatus.Success,
                        Data = data
                    }
                };
            }
            catch (Exception ex)
            {
                Log.Error(ex, "LoginUserAsyn");
                return new Response<AuthenticationResponse>
                {
                    ErrorData = new ErrorResponse<AuthenticationResponse>
                    {
                        ResponseCode = ErrorCode.INTERNAL_SERVER_ERROR,
                        ResponseMessage =
                            "Sorry, we are unable to complete this operation at the moment, kindly try again later"
                    }
                };
            }


        }
    }
}
