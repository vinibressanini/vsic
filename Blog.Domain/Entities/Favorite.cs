namespace Blog.Domain.Entities
{
    public class Favorite
    {

        public Guid PostId { get; private init; }
        public Guid UserId { get; private init; }

        // ORM
        public Favorite() { }

        public Favorite(Guid postId, Guid userId)
        {
            PostId = postId;
            UserId = userId;
        }

    }
}
