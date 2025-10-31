using Microsoft.AspNetCore.Identity;

namespace APIFinalProj.Models
{
    public class ApplicationUser : IdentityUser
    {
        public int? StudentId { get; set; }
        public Student Student { get; set; }   
    }
}
