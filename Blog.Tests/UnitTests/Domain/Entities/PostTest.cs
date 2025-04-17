using Blog.Domain.Entities;
using Blog.Domain.Events.Post;

namespace Blog.Tests.UnitTests.Domain.Entities
{

    internal class PostTest
    {

        private Post post;
        private User author;
        private Category category;

        [SetUp]
        public void SetUp()
        {
            post = new Post(id: new Guid(), title : "Post Title",content : "Post Content");
            author = new User(id: new Guid(), name: "User", email: "email.com", password: "password");
            category = new(id: new Guid(),name: "Tech");

        }

        [Test]
        public void AddComent_ShouldAddANewComment_WhenSuccessful()
        {

            Comment comment = new Comment()
            {
                Id = new Guid(),
                Author = author,
                Content = "This is my comment"
            };

            post.AddComment(comment);

            Assert.That(post.Comments, Is.Not.Empty);
            Assert.That(post.Comments.Count(), Is.EqualTo(1));
            Assert.That(post.Comments.First().Author, Is.EqualTo(author));

        }

        [Test]
        public void AddComent_ShouldThrowException_WhenContentIsNullOrEmpty()
        {

            Comment comment = new Comment()
            {
                Id = new Guid(),
                Author = author,
                Content = ""
            };

            Comment nullContentComment = new Comment()
            {
                Id = new Guid(),
                Author = author,
            };

            var call = post.AddComment;

            Assert.Throws<Exception>(() => call(comment));
            Assert.Throws<Exception>(() => call(nullContentComment));

        }

        [Test]
        public void AssignToCategory_ShouldAddCategoryToPost_WhenSuccessful()
        {
            post.AssignToCategory(category);

            Assert.That(post.Categories, Is.Not.Empty);
            Assert.That(post.Categories.Count(), Is.EqualTo(1));
            Assert.That(post.Categories.First(), Is.EqualTo(category));

        }

        [Test]
        public void AssignToCategory_ShouldThrowException_WhenCategoryIsAlreadyAssigned()
        {
            var call = post.AssignToCategory;

            post.AssignToCategory(category);

            Assert.Throws<Exception>(() => call(category));

        }

        [Test]
        public void RemoveAssignedCategory_ShouldRemoveAssignedCategory_WhenSuccessful()
        {

            post.AssignToCategory(category);
            post.RemoveAssignedCategory(category);

            Assert.That(post.Categories, Is.Empty);
            Assert.That(post.Categories.Count(), Is.EqualTo(0));


        }

        [Test]
        public void RemoveAssignedCategory_ShouldThrowException_WhenCategoryIsntAssigned()
        {

            var call = post.RemoveAssignedCategory;

            Assert.Throws<Exception>(() => call(category));


        }

        [Test]
        public void Publish_ShouldRaiseDomainEvent_WhenSuccessful()
        {
            post.AssignToCategory(category);

            post.Publish();

            var dEvent = post.GetDomainEvents();

            Assert.That(dEvent, Is.Not.Empty);
            Assert.That(dEvent.First(), Is.TypeOf<PostCreatedEvent>());
        }

        [Test]
        public void Publish_ShouldInitializePostCreationData_WhenAtleastOneCategoryIsAssigned()
        {
            post.AssignToCategory(category);

            post.Publish();

            Assert.That(post.Status, Is.EqualTo(PostStatus.Active));
            Assert.That(post.Slug, Is.Not.Empty);
            Assert.That(post.CreatedAt, Is.Not.EqualTo(default(DateTime)));
            Assert.That(post.UpdatedAt, Is.Not.EqualTo(default(DateTime)));

        }

        [Test]
        public void Publish_ShouldThrowException_WhenNoCategoryIsAssigned()
        {

            var call = post.Publish;

            Assert.Throws<Exception>(() => call());

            Assert.That(post.CreatedAt, Is.EqualTo(default(DateTime)));
            Assert.That(post.UpdatedAt, Is.EqualTo(default(DateTime)));

        }

        [Test]
        public void PublishAtDate_ShouldInitializePublishAt_WhenAtleastOneCategoryIsAssignedAndDateIsGreaterThanNow()
        {
            post.AssignToCategory(category);

            DateTime publishDate = DateTime.UtcNow.AddDays(1);

            post.PublishAtDate(publishDate);

            Assert.That(post.Status, Is.EqualTo(PostStatus.Inactive));
            Assert.That(post.Slug, Is.Not.Empty);
            Assert.That(post.CreatedAt, Is.EqualTo(publishDate));
            Assert.That(post.UpdatedAt, Is.EqualTo(publishDate));
            Assert.That(post.PublishAt, Is.EqualTo(publishDate));
        }

        [Test]
        public void PublishAtDate_ShouldThrowException_WhenNoCategoryIsAssigned()
        {

            DateTime publishDate = DateTime.UtcNow.AddDays(1);

            var call = post.PublishAtDate;

            Assert.Throws<Exception>(() => call(publishDate));

            Assert.That(post.CreatedAt, Is.EqualTo(default(DateTime)));
            Assert.That(post.UpdatedAt, Is.EqualTo(default(DateTime)));
            Assert.That(post.PublishAt, Is.Null);
        }

        [Test]
        public void PublishAtDate_ShouldThrowException_WhenDateIsEqualOrLessThanNow()
        {

            DateTime publishDate = DateTime.UtcNow;

            post.AssignToCategory(category);

            var call = post.PublishAtDate;

            Assert.Throws<Exception>(() => call(publishDate));

            Assert.That(post.CreatedAt, Is.EqualTo(default(DateTime)));
            Assert.That(post.UpdatedAt, Is.EqualTo(default(DateTime)));
            Assert.That(post.PublishAt, Is.Null);
        }

        [Test]
        public void GenerateSlug_ShouldCreatePostSlug_WhenSuccessful()
        {
            post.AssignToCategory(category);

            post.Publish();

            Assert.That(post.Slug, Is.Not.Empty);
        }

    }
}
