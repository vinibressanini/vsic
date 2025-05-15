using Blog.Application.Exceptions;
using Blog.Infra.Configs;
using Blog.Infra.Services.Notification;
using Blog.Shared.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Moq;
using System.Net.Sockets;
using SmtpStatusCode = MailKit.Net.Smtp.SmtpStatusCode;

namespace Blog.Tests.UnitTests.Infra.Services
{
    public class EmailServiceTest
    {


        private Mock<ISMTPClientWrapper> smtpClient;
        private IEmailService emailService;
        private EmailMessage message;
        private SmtpConfiguration smtpConfiguration;
        private IOptions<SmtpConfiguration> options;


        [SetUp]
        public void SetUp()
        {
            smtpConfiguration = new() {Server = "fake.smtp.com",Username = "sender@email.com", Password = "password", Port = 576};
            options = Options.Create(smtpConfiguration);

            message = new(To: "receiver@email.com", Subject: "Test Email", Body: "Test");
        }

        [Test]
        public async Task SendAsync_ShouldThrowNoException_WhenSuccessful()
        {


            smtpClient = new Mock<ISMTPClientWrapper>();

            smtpClient.Setup(c => c.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            smtpClient.Setup(c => c.SupportsAuthentication).Returns(true);
            smtpClient.Setup(c => c.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            smtpClient.Setup(c => c.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            smtpClient.Setup(c => c.DisconnectAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);


            emailService = new MailKitEmailService(options,smtpClient.Object);

            await emailService.SendAsync(message);

            smtpClient.Verify(m => m.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()),Times.Once);
            smtpClient.Verify(m => m.SupportsAuthentication,Times.Once);
            smtpClient.Verify(m => m.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            smtpClient.Verify(m => m.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()), Times.Once);
            smtpClient.Verify(m => m.DisconnectAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()),Times.Once);


        }

        [Test]
        public async Task SendAsync_ShouldThrowEmailSendException_WhenServerConnectionFails()
        {

            smtpClient = new Mock<ISMTPClientWrapper>();

            smtpClient.Setup(c => c.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).ThrowsAsync(new SocketException(400,"Unknown SMTP server"));

            emailService = new MailKitEmailService(options, smtpClient.Object);

            var call = emailService.SendAsync;

            var ex = Assert.ThrowsAsync<EmailSendException>(() => call(message));

            Assert.That(ex.Message, Does.Contain("Error while connecting to SMTP server. Check the host address"));
            Assert.That(ex.InnerException, Is.TypeOf<SocketException>());

            smtpClient.Verify(m => m.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task SendAsync_ShouldNotTryToAuth_WhenServerDoesntSupportAuth()
        {

            smtpClient = new Mock<ISMTPClientWrapper>();

            smtpClient.Setup(c => c.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            smtpClient.Setup(c => c.SupportsAuthentication).Returns(false);
            smtpClient.Setup(c => c.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            smtpClient.Setup(c => c.DisconnectAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            emailService = new MailKitEmailService(options, smtpClient.Object);

            await emailService.SendAsync(message);

            smtpClient.Verify(m => m.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            smtpClient.Verify(m => m.SupportsAuthentication, Times.Once);
            smtpClient.Verify(m => m.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Never);
            smtpClient.Verify(m => m.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()), Times.Once);
            smtpClient.Verify(m => m.DisconnectAsync(It.IsAny<bool>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task SendAsync_ShouldThrowEmailSendException_WhenAuthFails()
        {

            smtpClient = new Mock<ISMTPClientWrapper>();

            smtpClient.Setup(c => c.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            smtpClient.Setup(c => c.SupportsAuthentication).Returns(true);
            smtpClient.Setup(c => c.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).ThrowsAsync(new AuthenticationException("Error while authenticating"));

            emailService = new MailKitEmailService(options, smtpClient.Object);

            var call = emailService.SendAsync;

            var ex = Assert.ThrowsAsync<EmailSendException>(() => call(message));

            Assert.That(ex.Message, Does.Contain("Error while authenticating. Wrong credentials"));
            Assert.That(ex.InnerException, Is.TypeOf<AuthenticationException>());

            smtpClient.Verify(m => m.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            smtpClient.Verify(m => m.SupportsAuthentication, Times.Once);
            smtpClient.Verify(m => m.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
        }

        [Test]
        public async Task SendAsync_ShouldThrowEmailSendException_WhenSendAsyncFails()
        {

            smtpClient = new Mock<ISMTPClientWrapper>();

            smtpClient.Setup(c => c.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            smtpClient.Setup(c => c.SupportsAuthentication).Returns(true);
            smtpClient.Setup(c => c.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);
            smtpClient.Setup(c => c.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>())).ThrowsAsync(new SmtpCommandException(SmtpErrorCode.SenderNotAccepted,SmtpStatusCode.MailboxUnavailable ,"Error while sending email"));

            emailService = new MailKitEmailService(options, smtpClient.Object);

            var call = emailService.SendAsync;

            var ex = Assert.ThrowsAsync<EmailSendException>(() => call(message));

            Assert.That(ex.Message, Does.Contain($"Error [{SmtpErrorCode.SenderNotAccepted}] while sending message"));
            Assert.That(ex.InnerException, Is.TypeOf<SmtpCommandException>());

            smtpClient.Verify(m => m.ConnectAsync(It.IsAny<string>(), It.IsAny<int>(), It.IsAny<CancellationToken>()), Times.Once);
            smtpClient.Verify(m => m.SupportsAuthentication, Times.Once);
            smtpClient.Verify(m => m.AuthenticateAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<CancellationToken>()), Times.Once);
            smtpClient.Verify(m => m.SendAsync(It.IsAny<EmailMessage>(),It.IsAny<CancellationToken>()), Times.Once);
        }

    }
}
