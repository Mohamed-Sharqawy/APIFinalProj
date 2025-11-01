namespace APIFinalProj.DTOs
{
    public class RegisterResponseDto
    {
        public bool Successe { get; set; }
        public string Message { get; set; }
        public int? StudentId { get; set; }
        public string UserId { get; set; }
    }
}
