namespace Secrets.Models
{
    public abstract class Response
    {
        protected Response(bool success)
        {
            this.Success = success;
        }

        public bool Success { get; }
    }
}
