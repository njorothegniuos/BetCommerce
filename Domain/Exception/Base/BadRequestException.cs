namespace Domain.Exception.Base
{
    public abstract class BadRequestException : System.Exception
    {
        protected BadRequestException(string message)
            : base(message)
        {
        }
    }
}
