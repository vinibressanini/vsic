namespace Blog.Application.Exceptions
{
    public class EmailSendException : Exception
    {
        public EmailSendException(string? message, Exception? innerException) : base(message, innerException)
        {
        }
    }
}
