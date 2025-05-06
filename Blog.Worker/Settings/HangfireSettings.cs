using Hangfire;

namespace Blog.Worker.Settings
{
    public static class HangfireSettings
    {

        public static WebApplicationBuilder AddHangfire(this WebApplicationBuilder builder)
        {

            builder.Services.AddHangfire((cfg) =>
            {

                cfg.UseInMemoryStorage();
                cfg.UseRecommendedSerializerSettings();
                cfg.UseSimpleAssemblyNameTypeSerializer();

            });

            builder.Services.AddHangfireServer();

            return builder;
        }

    }
}
