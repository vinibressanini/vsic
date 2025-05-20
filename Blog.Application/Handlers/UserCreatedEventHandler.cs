using Blog.Application.Models;
using Blog.Domain.Events;
using Blog.Domain.Events.User;
using Blog.Infra.Context;
using Blog.Shared.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Blog.Application.Handlers
{
    public class UserCreatedEventHandler : IDomainEventHandler
    {

        private IEmailService emailService;
        private IEmailTemplateRenderer<WelcomeEmailModel> renderer;
        private BlogDbContext context;

        public UserCreatedEventHandler(IEmailService emailService, IEmailTemplateRenderer<WelcomeEmailModel> renderer, BlogDbContext context)
        {
            this.emailService = emailService;
            this.renderer = renderer;
            this.context = context;
        }

        public async Task Handle(UserCreatedEvent @event)
        {

            var posts = await context.Post
                .AsNoTracking()
                .OrderByDescending(p => p.Views)
                .Take(4)
                .Select(post => new PostEmailModel() {Title = post.Title, Description = post.Description,Url = "", ImageUrl = ""} ).ToListAsync();
                

            var emailModel = new WelcomeEmailModel() { Username = @event.Username , Posts = posts};

            var html = await renderer.RenderEmailTemplate(emailModel);

            var message = new EmailMessage(To: @event.Email, Subject: "Welcome to Vsic", Body: html, IsHtml: true);


            try
            {
                await emailService.SendAsync(message);

            } catch (Exception)
            {
                throw;
            }




        }

    }
}
