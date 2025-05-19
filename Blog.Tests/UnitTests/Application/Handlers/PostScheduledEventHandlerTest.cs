
using Blog.Application.Handlers;
using Blog.Domain.Entities;
using Blog.Domain.Events.Post;
using Blog.Infra.Context;
using Blog.Tests.Fixtures;
using Microsoft.EntityFrameworkCore;

namespace Blog.Tests.UnitTests.Application.Handlers
{
    public class PostScheduledEventHandlerTest 
    {

        private BlogDbContext context;
        private PostScheduledEvent @event;
        private PostScheduledEventHandler handler;

        [SetUp]
        public async Task SetUp()
        {
            var fixure = new BlogDbContextFixture();

            context = fixure.context;

            var post = await context.Post.FirstAsync(p => p.PublishAt != null);

            handler = new(context);

            @event = new(post.Id, post.PublishAt!.Value);
        }

        
        [Test]
        public async Task Handle_ShouldChangePostStatusToActive_WhenSuccessful()
        {

            var post = await context.Post.FirstAsync(p => p.Id == @event.PostId);

            Assert.That(post.Status, Is.EqualTo(PostStatus.Inactive));

            await handler.Handle(@event);

            Assert.That(post.Status, Is.EqualTo(PostStatus.Active));


        }

        

    }
}
