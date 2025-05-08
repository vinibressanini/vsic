using Hangfire;
using Npgsql;

namespace Blog.Worker
{
    public class Listener : BackgroundService
    {
        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await using var conn = new NpgsqlConnection("Host=localhost;Database=postgres;Username=postgres;Password=postgres;Port=5432");

            await conn.OpenAsync();

            conn.Notification += (obj, args) =>
            {

                BackgroundJob.Enqueue<JobHandler>(jb => jb.Handle());


            };

            using var command = new NpgsqlCommand("LISTEN new_domain_event_channel",conn);

            await command.ExecuteNonQueryAsync();

            while(!stoppingToken.IsCancellationRequested)
            {

                await conn.WaitAsync();

            }

        }
    }

    
}