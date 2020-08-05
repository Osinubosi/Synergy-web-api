using Synergy.Domain.ServiceModel;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Synergy.Domain.Interfaces
{
    public interface IEmailService
    {
        Task<int> SendEmail(EmailRequest request);
    }
}
