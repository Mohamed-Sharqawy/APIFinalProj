using System.ComponentModel.DataAnnotations;

namespace APIFinalProj.DTOs
{
    public class UpdateStudentDto
    {
        [StringLength(100)]
        public string? Name { get; set; }
        [Range(18, 33)]
        public int? Age { get; set; }
    }
}
