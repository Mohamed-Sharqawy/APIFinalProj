using System.ComponentModel.DataAnnotations;

namespace APIFinalProj.DTOs
{
    public class RegisterStudentDto
    {
        [Required, EmailAddress]
        public string Email { get; set; }
        [Required, MinLength(6)]
        public string Password { get; set; }
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
        [Required, Range(18, 33)]
        public int Age { get; set; }
    }
}
