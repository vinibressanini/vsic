﻿namespace Blog.Infra.Configs
{
    public class SmtpConfiguration
    {
        public string Server { get; set; }
        public int Port { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public SmtpConfiguration() { }
    }
}
