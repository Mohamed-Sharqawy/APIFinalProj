using APIFinalProj.Context;
using APIFinalProj.Models;
using APIFinalProj.Repository;

namespace APIFinalProj.UOF
{
    public class UnitOfWork
    {
        AppDbContext context;
        GenericRepository<Course> CourseRepo;
        GenericRepository<Student> StudentRepo;
        GenericRepository<Enrollment> EnrollemtnRepo;

        public UnitOfWork(AppDbContext context)
        {
            this.context = context;
        }
        public GenericRepository<Enrollment> EnrRepo
        {
            get
            {
                if (EnrollemtnRepo == null)
                    EnrollemtnRepo = new GenericRepository<Enrollment>(context);
                return EnrollemtnRepo;
            }
        }
        public GenericRepository<Course> CrsRepo
        {
            get
            {
                if (CourseRepo == null)
                    CourseRepo = new GenericRepository<Course>(context);
                return CourseRepo;
            }
        }

        public GenericRepository<Student> StdRepo
        {
            get
            {
                if (StudentRepo == null)
                    StudentRepo = new GenericRepository<Student>(context);
                return StudentRepo;
            }
        }

    }
}
