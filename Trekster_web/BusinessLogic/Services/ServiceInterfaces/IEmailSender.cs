using BusinessLogic.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLogic.Services.ServiceInterfaces
{
    public interface IEmailSender
    {
        void SendEmail(Message message);

        Task SendEmailAsync(Message message);
    }
}
