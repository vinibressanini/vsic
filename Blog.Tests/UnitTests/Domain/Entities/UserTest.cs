using Blog.Domain.Entities;
using Blog.Domain.Events.User;
using NuGet.Frameworks;
using System.Xml.Linq;

namespace Blog.Tests.UnitTests.Domain.Entities
{
    internal class UserTest
    {

        private User user;
        private Post post;


        [SetUp]
        public void SetUp()
        {
            user = new() { Id = new Guid(), Name = "user", Email = "email.com", Password = "password" };
            post = new() { Id = new Guid(), Title = "Post title", Content = "Post content" };
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

        [Test]
        public void UserCreation_ShouldRaiseADomainEvent_WhenSuccessful()
        {

            User newUser = new User(id: new Guid(), name : "User", email : "myemail.com", password : "password");

            var dEvent = newUser.GetDomainEvents();

            Assert.That(dEvent, Is.Not.Empty);
            Assert.That(dEvent.First(), Is.Not.Null);
            Assert.That(dEvent.First(), Is.TypeOf<UserCreatedEvent>());


        }


    }
}
