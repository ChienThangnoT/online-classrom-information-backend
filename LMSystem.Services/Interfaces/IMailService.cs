using LMSystem.Repository.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LMSystem.Services.Interfaces
{
    public interface IMailService
    {
        public Task SendEmailAsync(EmailRequest emailRequest);
        public Task SendConFirmEmailAsync(EmailRequest emailRequest);
    }
}
