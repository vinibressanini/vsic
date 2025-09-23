namespace Blog.Shared.Interfaces
{
    public interface IEmailTemplateRenderer<T> where T : IEmailModel
    {
        Task<string> RenderEmailTemplate(T model);
    }
}
