namespace EduResourceAPI.Controllers.Errors
{
    public class BadRequestError
    {
        public BadRequestError(string? traceId, object errors)
        {
            if (traceId == null) this.TraceId = String.Empty;
            else this.TraceId = traceId;
            this.Errors = errors;
        }

        public string Type { get; set; } = "https://tools.ietf.org/html/rfc7231#section-6.5.1";
        public string Title { get; set; } = "One or more validation errors occurred.";
        public int Status { get; set; } = 400;
        public string TraceId { get; set; }
        public object Errors { get; set; }
    }
}
