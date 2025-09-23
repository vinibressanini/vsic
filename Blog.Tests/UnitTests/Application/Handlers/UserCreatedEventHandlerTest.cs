using Blog.Application.Exceptions;
using Blog.Application.Handlers;
using Blog.Application.Models;
using Blog.Domain.Events.User;
using Blog.Infra.Context;
using Blog.Shared.Interfaces;
using Blog.Tests.Fixtures;
using Moq;

namespace Blog.Tests.UnitTests.Application.Handlers
{
    public class UserCreatedEventHandlerTest
    {

        private Mock<IEmailService> emailService;
        private Mock<IEmailTemplateRenderer<WelcomeEmailModel>> renderer;
        private BlogDbContext context;
        private UserCreatedEventHandler handler;
        private UserCreatedEvent @event;

        [SetUp]
        public void SetUp()
        {
            renderer = new Mock<IEmailTemplateRenderer<WelcomeEmailModel>>();
            emailService = new Mock<IEmailService>();
            var fixture = new BlogDbContextFixture();
            context = fixture.context;
            @event = new UserCreatedEvent(Username: "User", Email: "user@email.com");

        }

        [Test]
        public async Task Handle_ShouldSendEmail_WhenSuccessfull()
        {

            

            renderer.Setup(r => r.RenderEmailTemplate(It.IsAny<WelcomeEmailModel>())).ReturnsAsync("html template");
            emailService.Setup(e => e.SendAsync(It.IsAny<EmailMessage>(),It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            handler = new UserCreatedEventHandler(emailService.Object, renderer.Object, context);

            await handler.Handle(@event);

            renderer.Verify(r => r.RenderEmailTemplate(It.IsAny<WelcomeEmailModel>()), Times.Once);
            emailService.Verify(e => e.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()),Times.Once);

        }

        [Test]
        public async Task Handle_ShouldRethrowException_WhenAnyEmailSendingFails()
        {

            emailService = new Mock<IEmailService>();

            emailService.Setup(m => m.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new EmailSendException("Error while sending email", new Exception("inner ex")));

            handler = new UserCreatedEventHandler(emailService.Object, renderer.Object, context);


            var call = handler.Handle;

            var ex = Assert.ThrowsAsync<EmailSendException>(() => call(@event));
            Assert.That(ex.Message, Is.EqualTo("Error while sending email"));

        }
    }
}
