namespace ContactsApp.Server.DTOs
{
    public class AuthResultDto
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}
