using APIFinalProj.Context;
using APIFinalProj.Models;
using Microsoft.EntityFrameworkCore;

namespace APIFinalProj.Repository
{
    public class StudentRepository
    {
        AppDbContext context;

        public StudentRepository(AppDbContext context)
        {
            this.context = context;
        }

        public async Task<Student> GetUserByIdAsync (string userid)
        {
            return await context.Students
                .Include(s => s.Enrollments)
                .ThenInclude(e => e.Course)
                .FirstOrDefaultAsync(s => s.UserId == userid);
        }
    }
}
