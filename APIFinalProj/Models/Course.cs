using System.ComponentModel.DataAnnotations;

namespace APIFinalProj.Models
{
    public class Course : BaseModel
    {
        [Required]
        [StringLength(10)]
        public string Code { get; set; }

        [Required]
        public string Duration { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }
    }
}
