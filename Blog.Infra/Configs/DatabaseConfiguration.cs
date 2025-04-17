namespace Blog.Infra.Configs
{
    public class DatabaseConfiguration
    {
        public string Host { get; private init; }
        public string Database{ get; private init; }
        public string User{ get; private init; }
        public string Password{ get; private init; }

        public DatabaseConfiguration(string host, string database, string user, string password)
        {
            Host = host;
            Database = database;
            User = user;
            Password = password;
        }
    }
}
