namespace Quoridor.Models.General
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
