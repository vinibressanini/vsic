using Blog.Worker.Settings;

namespace Blog.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddHostedService<Listener>();
            builder.AddHangfire();

            var app = builder.Build();

            // Configure the HTTP request pipeline.


            app.Run();
        }
    }
}
