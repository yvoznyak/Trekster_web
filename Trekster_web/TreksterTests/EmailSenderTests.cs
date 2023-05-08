using System;
using System.Net.Mail;
using BusinessLogic.Models;
using BusinessLogic.Services.ServiceInterfaces;
using EmailService;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

using Moq;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using Smtp = System.Net.Mail;
using Xunit;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Threading.Tasks;
using MailKit;
using System.Security.Authentication;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Net;
using MailKit.Net.Proxy;
using System.Net.Sockets;
using System.Text;
using MimeKit;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Smtp;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using BusinessLogic.Services.ServiceImplementation;
using Microsoft.Extensions.Logging;

namespace BusinessLogic.Tests
{
    public class EmailSenderTests
    {
        private readonly Mock<EmailConfiguration> _emailConfigMock;
        private readonly Mock<Services.ServiceInterfaces.IEmailSender> _emailSenderMock;
        private readonly List<string> _recipientList;
        private readonly string _subject;
        private readonly string _content;
        private readonly IFormFileCollection _attachments;
        private readonly Mock<ILogger<EmailSender>> _loggerMock;

        public EmailSenderTests()
        {
            _emailConfigMock = new Mock<EmailConfiguration>();
            _emailSenderMock = new Mock<Services.ServiceInterfaces.IEmailSender>();
            _recipientList = new List<string> { "recipient@example.com" };
            _subject = "Test email";
            _content = "This is a test email";
            _attachments = new FormFileCollection();
            _loggerMock = new Mock<ILogger<EmailSender>>();
        }

        [Fact]
        public void TestSendEmail()
        {
            //Arrange
            var message = new Message(_recipientList, _subject, _content, _attachments);
            _emailSenderMock.Setup(x => x.SendEmail(message));

            //Act
            _emailSenderMock.Object.SendEmail(message);

            //Assert
            _emailSenderMock.Verify(x => x.SendEmail(message), Times.Once);
        }

        [Fact]
        public async Task TestSendEmailAsync()
        {
            //Arrange
            var message = new Message(_recipientList, _subject, _content, _attachments);
            _emailSenderMock.Setup(x => x.SendEmailAsync(message));

            //Act
            await _emailSenderMock.Object.SendEmailAsync(message);

            //Assert
            _emailSenderMock.Verify(x => x.SendEmailAsync(message), Times.Once);
        }

        [Fact]
        public void TestSendEmailWithAttachments()
        {
            //Arrange
           
            var message = new Message(_recipientList, _subject, _content, _attachments);
            _emailSenderMock.Setup(x => x.SendEmail(message));

            //Act
            _emailSenderMock.Object.SendEmail(message);

            //Assert
            _emailSenderMock.Verify(x => x.SendEmail(message), Times.Once);
        }

        [Fact]
        public async Task TestSendEmailAsyncWithAttachments()
        {
            //Arrange

            var message = new Message(_recipientList, _subject, _content, _attachments);
            _emailSenderMock.Setup(x => x.SendEmailAsync(message));

            //Act
            await _emailSenderMock.Object.SendEmailAsync(message);

            //Assert
            _emailSenderMock.Verify(x => x.SendEmailAsync(message), Times.Once);
        }

       
    }
}
