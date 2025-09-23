using Blog.Domain.Entities;
using Blog.Infra.Context;
using Blog.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Blog.Tests.UnitTests.Infra.Context
{
    internal class AddDomainEventInterceptorTest
    {


        private BlogDbContext context;


        [SetUp]
        public void SetUp()
        {

            var fixture = new BlogDbContextFixture();

            context = fixture.context;

        }


        [Test]
        public async Task AddDomainEvents_ShouldAddEntityDomainEvents_WhenSuccessful()
        {

            var user = new User(id: new Guid(), name: "john", email: "test@email.com", password: "secret");

            await context.User.AddAsync(user);
            await context.SaveChangesAsync();

            var domainEvents = await context.DomainEvent.ToListAsync();

            Assert.That(domainEvents, Is.Not.Empty);
            Assert.That(domainEvents.Last().Event,Is.EqualTo("UserCreatedEvent"));


        }


    }
}
