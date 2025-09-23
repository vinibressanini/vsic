using Blog.Shared.Interfaces;
using RazorLight;

namespace Blog.Infra.Services.EmailRenderer
{

    public class EmailRenderer<T> : IEmailTemplateRenderer<T> where T : IEmailModel
    {

        private readonly RazorLightEngine _engine;

        public EmailRenderer()
        {

            var templatesPath = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", "..", "Blog.Application", "Templates"));

            _engine = new RazorLightEngineBuilder()
                .UseFileSystemProject(templatesPath)
                .UseMemoryCachingProvider()
                .Build();
        }

        public async Task<string> RenderEmailTemplate(T model)
        {
            var type = model.GetType().Name.Replace("Model", ".cshtml");

            return await _engine.CompileRenderAsync(type, model);
        }
    }
}
