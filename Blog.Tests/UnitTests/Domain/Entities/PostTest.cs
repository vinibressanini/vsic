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
            post = new Post() { Id = new Guid(), Content = "This is the post content", Title = "Post Title" };
            author = new User() { Id = new Guid(), Name = "user", Email = "user@email", Password = "password" };
            category = new() { Id = new Guid(), Name = "Tech" };

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
        public void PostCreation_ShouldRaiseDomainEvent_WhenSuccessful()
        {
            Post newPost = new Post(id: new Guid(),title: "new title",content: "Lorem Ipsum is simply dummy text of the printing and typesetting industry. Lorem Ipsum has been the industry's standard dummy text ever since the 1500s, when an unknown printer took a galley of type and scrambled it to make a type specimen book. It has survived not only five centuries, but also the leap into electronic typesetting, remaining essentially unchanged. It was popularised in the 1960s with the release of Letraset sheets containing Lorem Ipsum passages, and more recently with desktop publishing software like Aldus PageMaker including versions of Lorem Ipsum.");

            var dEvent = newPost.GetDomainEvents();

            Assert.That(dEvent, Is.Not.Empty);
            Assert.That(dEvent.First(), Is.TypeOf<PostCreatedEvent>());
        }

    }
}
