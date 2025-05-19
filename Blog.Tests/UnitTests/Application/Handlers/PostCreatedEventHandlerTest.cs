using Blog.Application.Exceptions;
using Blog.Domain.Events.Post;
using Blog.Infra.Context;
using Blog.Shared.Interfaces;
using Blog.Tests.Fixtures;
using Microsoft.Extensions.Logging;
using Moq;

namespace Blog.Tests.UnitTests.Application.Handlers
{
    public class PostCreatedEventHandlerTest
    {

        private PostCreatedEventHandler handler;
        private BlogDbContext context;
        private Mock<IEmailService> emailService;
        private Mock<ILogger<PostCreatedEventHandler>> logger;
        private PostCreatedEvent @event;

        [SetUp]
        public async Task SetUp()
        {
            
            logger = new Mock<ILogger<PostCreatedEventHandler>>();

            var fixture = new BlogDbContextFixture();

            context = fixture.context;

            @event = new(PostName: "PostCreatedEvent Test", ContentPreview: "Lorem Ipsum Dolor");

        }

        [Test]
        public async Task Handle_ShouldSendEmailToAllSubscribers_WhenSuccessful()
        {

            emailService = new Mock<IEmailService>();

            emailService.Setup(m => m.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>())).Returns(Task.CompletedTask);

            handler = new(emailService.Object, context, logger.Object);

            await handler.Handle(@event);

            emailService.Verify(m => m.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()),Times.Exactly(3));


        }

        [Test]
        public async Task Handle_ShouldEarlyReturn_WhenTheresNoSubscribers()
        {

            context.Subscriber.RemoveRange(context.Subscriber);

            await context.SaveChangesAsync();

            emailService = new Mock<IEmailService>();

            handler = new(emailService.Object, context, logger.Object);

            await handler.Handle(@event);

            emailService.Verify(m => m.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()), Times.Never());

        }

        [Test]
        public async Task Handle_ShouldRethrowException_WhenAnyEmailSendingFails()
        {

            emailService = new Mock<IEmailService>();

            emailService.Setup(m => m.SendAsync(It.IsAny<EmailMessage>(), It.IsAny<CancellationToken>()))
                .ThrowsAsync(new EmailSendException("Error while sending email",new Exception("inner ex")));

            handler = new(emailService.Object, context, logger.Object);

            var call = handler.Handle;

            var ex = Assert.ThrowsAsync<EmailSendException>(() => call(@event));
            Assert.That(ex.Message, Is.EqualTo("Error while sending email"));

        }

    }
}
