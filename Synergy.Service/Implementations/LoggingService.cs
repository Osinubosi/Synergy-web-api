using System;
using NLog;
using Synergy.Repository.Database;
using Synergy.Repository.Interfaces;
using Synergy.Repository.Models;
using Synergy.Service.Interfaces;
using Synergy.Service.ViewModel;

namespace Synergy.Service.Implementations
{
    public class LoggingService : ILoggingService
    {
        public static Logger Log = LogManager.GetCurrentClassLogger();

        public void LogRequestData(RequestLoggingViewModel request)
        {
            IDbContext dbContext = new SynergyDbContext();
            var requestLog = dbContext.Set<ClientRequestLog>();
            try
            {
                requestLog.Add(new ClientRequestLog
                {
                    ClientId = request.ClientId,
                    Payload = request.Payload,
                    RequestMethod = request.RequestMethod,
                    Url = request.Url,
                    RequestReferencId = request.RequestReferencId,
                    RequestUniqueRefernceId = request.RequestUniqueRefernceId,
                    DateLogged = DateTime.UtcNow
                });

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                Log.Error(ex, "LogRequestData");
            }
        }

        public void LogResponseData(ResponseLoggingViewModel response)
        {
            IDbContext dbContext = new SynergyDbContext();
            var responseLog = dbContext.Set<ClientResponseLog>();
            try
            {
                responseLog.Add(new ClientResponseLog
                {
                    Payload = response.Payload,
                    RequestUniqueRefernceId = response.RequestUniqueRefernceId,
                    HttpStatusCode = response.StatusCode,
                    SynergyStatusCode = response.CadawadaStatusCode,
                    DateLogged = DateTime.UtcNow
                });

                dbContext.SaveChanges();
            }
            catch (Exception ex)
            {

                Log.Error(ex, "LogResponseData");
            }
        }
    }
}
