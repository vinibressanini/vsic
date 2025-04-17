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
            user = new User(id: new Guid(), name: "User", email: "email.com", password: "password");
            post = new Post(id: new Guid(), title: "Post Title", content: "Post Content");
        }

        [Test]
        public void FavoritePost_ShouldAddPostToFavorites_WhenSuccessful()
        {
            user.FavoritePost(post);

            Assert.That(user.Favorites, Is.Not.Null);
            Assert.That(user.Favorites.Count(), Is.EqualTo(1));
            Assert.That(user.Favorites.First().Id, Is.EqualTo(post.Id));

        }


        [Test]
        public void FavoritePost_ShouldRemovePostFromFavorites_WhenSuccessful()
        {
            user.FavoritePost(post);

            Assert.That(user.Favorites, Is.Not.Null);
            Assert.That(user.Favorites.Count(), Is.EqualTo(1));

            user.FavoritePost(post);

            Assert.That(user.Favorites, Is.Empty);


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
