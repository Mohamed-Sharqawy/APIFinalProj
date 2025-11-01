using System.ComponentModel.DataAnnotations;

namespace APIFinalProj.DTOs
{
    public class EnrollmentDto
    {
        [Required]
        public int CourseId { get; set; }
    }
}
