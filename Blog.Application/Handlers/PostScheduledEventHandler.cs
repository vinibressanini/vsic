using Blog.Domain.Events.Post;
using Blog.Infra.Context;
using Blog.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Handlers
{
    public class PostScheduledEventHandler : IDomainEventHandler<PostScheduledEvent>
    {
        private readonly BlogDbContext _context;
        public PostScheduledEventHandler(BlogDbContext context)
        {
            _context = context;
        }

        public async Task Handle(PostScheduledEvent @event)
        {

            try {
                var post = await _context.Post.FirstOrDefaultAsync(p => p.Id == @event.PostId);

                post!.Publish();

                await _context.SaveChangesAsync();
            } catch (Exception )
            {
                throw;
            }


        }

    }
}
