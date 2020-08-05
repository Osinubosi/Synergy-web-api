using Synergy.Service.ViewModel;
using System;
using System.Collections.Generic;
using System.Text;

namespace Synergy.Service.Interfaces
{
    public interface ILoggingService
    {
        void LogRequestData(RequestLoggingViewModel request);

        void LogResponseData(ResponseLoggingViewModel response);
    }
}
