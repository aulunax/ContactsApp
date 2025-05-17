namespace ContactsApp.Server.DTOs
{
    /// <summary>
    /// DTO for Identity authentication results.
    /// </summary>
    public class AuthResultDto
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
