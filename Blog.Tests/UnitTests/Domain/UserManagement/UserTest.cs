
using Blog.Domain.ContentManagement;
using Blog.Domain.UserInteraction;
using Blog.Domain.UserManagement;

namespace Blog.Tests.UnitTests.Domain.UserManagement
{
    internal class UserTest
    {

        private User user;
        private Post post;


        [SetUp]
        public void SetUp()
        {
            user = new () {Id = new Guid(),Name = "user",Email = "email.com",Password = "password"};
            post = new () { Id = new Guid(), Title = "Post title", Content = "Post content"};
        }

        [Test]
        public void FavoritePost_ShouldAddPostToFavorites_WhenSuccessful()
        {
            user.FavoritePost(post.Id);

            Assert.That(user.Favorites, Is.Not.Null);
            Assert.That(user.Favorites.Count(), Is.EqualTo(1));
            Assert.That(user.Favorites.First().PostId, Is.EqualTo(post.Id));

        }
        
        [Test]
        public void FavoritePost_ShouldThrowException_WhenPostIsAlreadyAFavorite()
        {
            user.FavoritePost(post.Id);

            var call = user.FavoritePost;

            Assert.Throws<Exception>(() => call(post.Id));

        }
        
        [Test]
        public void UnfavoritePost_ShouldRemovePostFromFavorites_WhenSuccessful()
        {
            user.FavoritePost(post.Id);

            Assert.That(user.Favorites, Is.Not.Null);
            Assert.That(user.Favorites.Count(), Is.EqualTo(1));
            
            user.UnfavoritePost(post.Id);

            Assert.That(user.Favorites, Is.Empty);


        }

        [Test]
        public void UnfavoritePost_ShouldThrowException_WhenPostIsntAFavoriteYet()
        {

            var call = user.UnfavoritePost;

            Assert.Throws<Exception>(() => call(post.Id));

        }


    }
}
