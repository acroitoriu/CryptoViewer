namespace CryptoViewer.Application.Exceptions
{
    public class ExchangeServiceException : ApplicationException
    {
        public ExchangeServiceException() { }
        public ExchangeServiceException(string message) : base(message) { }
        public ExchangeServiceException(string message, Exception inner) : base(message, inner) { }
    }
}
