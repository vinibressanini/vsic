using Blog.Domain.ContentManagement;
using Blog.Domain.UserInteraction;
using Blog.Domain.UserManagement;

namespace Blog.Tests.UnitTests.Domain.ContentManagement
{

    internal class PostTest
    {

        private Post post;
        private User author;
        private Category category;

        [SetUp]
        public void SetUp()
        {
            post = new Post() { Id = new Guid(), Content = "This is the post content", Title = "Post Title"};
            author = new User() { Id = new Guid(),Name = "user", Email = "user@email", Password = "password"};
            category = new() { Id = new Guid(), Name = "Tech"};

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

    }
}
