namespace Quoridor.ErrorHandling
{
    public class ValidationError
    {
        public string Message { get; }
        public ValidationError(string message)
        {
            Message = message;
        }
    }
}
