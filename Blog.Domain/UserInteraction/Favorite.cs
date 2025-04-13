namespace Blog.Domain.UserInteraction
{
    public class Favorite
    {

        public Guid PostId { get; set; }
        public Guid UserId { get; set; }

        // ORM
        public Favorite() { }

        public Favorite(Guid postId, Guid userId)
        {
            PostId = postId;
            UserId = userId;
        }

    }
}
