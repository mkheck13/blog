namespace blog.Models.DTOS
{
    public class PasswordDTO
    {
        public string? Salt {get; set;}
        public string? Hash {get; set;}
    }
}