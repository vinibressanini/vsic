using Blog.Application.Handlers;
using Blog.Domain.Events;
using Blog.Domain.Events.Post;
using Blog.Infra.Context;
using Blog.Shared.Interfaces;
using Blog.Tests.Fixtures;
using Blog.Worker;
using Hangfire;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Moq;

namespace Blog.Tests.UnitTests.Worker
{
    public class JobHandlerTest
    {

        private BlogDbContext context;
        private Mock<IServiceScope> scope;
        private Mock<IServiceProvider> serviceProvider;
        private Mock<IServiceScopeFactory> scopeFactory;
        private Mock<ILogger<JobHandler>> logger;
        private Mock<IDomainEventHandler<PostCreatedEvent>> postEventHandler;
        private Mock<IDomainEventHandler<PostScheduledEvent>> postScheduledEventHandler;
        private Mock<IBackgroundJobClient> backgroundJob;
        private JobHandler jobHandler;


        [SetUp]
        public void SetUp()
        {
            var fixture = new BlogDbContextFixture();
            context = fixture.context;

            logger = new Mock<ILogger<JobHandler>>();

            backgroundJob = new Mock<IBackgroundJobClient>();

        }

        private void InitializeServiceScope()
        {
            serviceProvider = new Mock<IServiceProvider>();
            
            serviceProvider.Setup(sp => sp.GetService(typeof(IDomainEventHandler<PostCreatedEvent>)))

               .Returns(postEventHandler.Object);
            
            serviceProvider.Setup(sp => sp.GetService(typeof(IDomainEventHandler<PostScheduledEvent>)))
               .Returns(postScheduledEventHandler.Object);


            scope = new Mock<IServiceScope>();
            scope.Setup(s => s.ServiceProvider).Returns(serviceProvider.Object);

            scopeFactory = new Mock<IServiceScopeFactory>();
            scopeFactory.Setup(f => f.CreateScope()).Returns(scope.Object);
        }

        [Test]
        public async Task Handle_ShouldHandleAllPendingDomainEvents_WhenSuccessful()
        {
            postEventHandler = new Mock<IDomainEventHandler<PostCreatedEvent>>();
            postEventHandler.Setup(h => h.Handle(It.IsAny<PostCreatedEvent>()))
                .Returns(Task.CompletedTask);

            InitializeServiceScope();

            jobHandler = new JobHandler(context, scopeFactory.Object, backgroundJob.Object, logger.Object);

            await jobHandler.Handle();

            postEventHandler.Verify(h => h.Handle(It.IsAny<PostCreatedEvent>()), Times.AtLeastOnce);


            var domainEvents = await context.DomainEvent.ToListAsync();

            Assert.That(domainEvents, Is.Not.Empty);
            Assert.That(domainEvents.Any(d => d.Status == DomainEventStatus.Pending),Is.False);


        }

        [Test]
        public async Task Handle_ShouldMarkFailedEventsAsFailed_WhenExceptionIsThrown()
        {
            postEventHandler = new Mock<IDomainEventHandler<PostCreatedEvent>>();
            postEventHandler.Setup(h => h.Handle(It.IsAny<PostCreatedEvent>()))
                .ThrowsAsync(new Exception("Something Happened"));

            InitializeServiceScope();

            jobHandler = new JobHandler(context, scopeFactory.Object, backgroundJob.Object, logger.Object);

            await jobHandler.Handle();

            var domainEvents = await context.DomainEvent.Where(de => de.Event == "PostCreatedEvent").ToListAsync();

            Assert.That(domainEvents, Is.Not.Empty);
            Assert.That(domainEvents.All(de => de.Status == DomainEventStatus.Failed), Is.True);

            postEventHandler.Verify(h => h.Handle(It.IsAny<PostCreatedEvent>()),Times.Exactly(domainEvents.Count));

        }

        [Test]
        public async Task Handle_ShouldContinue_WhenEventHandlerExceptionIsThrown()
        {

            postEventHandler = new Mock<IDomainEventHandler<PostCreatedEvent>>();
            postEventHandler.Setup(h => h.Handle(It.IsAny<PostCreatedEvent>()))
                .ThrowsAsync(new Exception("Something Happened"));

            postScheduledEventHandler = new Mock<IDomainEventHandler<PostScheduledEvent>>();
            postScheduledEventHandler.Setup(h => h.Handle(It.IsAny<PostScheduledEvent>())).Returns(Task.CompletedTask);

            InitializeServiceScope();

            jobHandler = new JobHandler(context, scopeFactory.Object, backgroundJob.Object,logger.Object);

            await jobHandler.Handle();

            var postCreatedEvents = await context.DomainEvent.Where(de => de.Event == "PostCreatedEvent").ToListAsync();
            var postScheduledEvents = await context.DomainEvent.Where(de => de.Event == "PostScheduledEvent").ToListAsync();

            Assert.That(postCreatedEvents, Is.Not.Empty);
            Assert.That(postScheduledEvents, Is.Not.Empty);

            Assert.That(postCreatedEvents.All(de => de.Status == DomainEventStatus.Failed), Is.True);
            Assert.That(postScheduledEvents.All(de => de.Status == DomainEventStatus.Processed), Is.True);

        }


    }
}
