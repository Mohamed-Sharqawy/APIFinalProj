using APIFinalProj.Models;

namespace APIFinalProj.DTOs
{
    public class StudentResponseDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Age { get; set; }
        public string Email { get; set; }
        public List<Enrollment> Enrollments { get; set; }
    }
}
