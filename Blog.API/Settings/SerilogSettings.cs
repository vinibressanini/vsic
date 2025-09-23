using Serilog;

namespace Blog.API.Settings
{
    public static class SerilogSettings
    {

        public static WebApplicationBuilder AddLogging(this WebApplicationBuilder builder)
        {

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .CreateLogger();

            builder.Host.UseSerilog();

            return builder;

        }

    }
}
