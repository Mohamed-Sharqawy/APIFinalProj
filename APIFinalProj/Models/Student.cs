using System.ComponentModel.DataAnnotations;

namespace APIFinalProj.Models
{
    public class Student : BaseModel
    {
        [Required]
        [Range(18,33)]
        public int Age { get; set; }

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        public ICollection<Enrollment> Enrollments { get; set; }   
    }
}
