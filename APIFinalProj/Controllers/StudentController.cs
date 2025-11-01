using APIFinalProj.DTOs;
using APIFinalProj.Models;
using APIFinalProj.UOF;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIFinalProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
            UnitOfWork unit;

            public StudentController(UnitOfWork unit)
            {
                this.unit = unit;
            }

            [HttpGet]
            public IActionResult GetAll()
            {
                var Students = unit.StdRepo.GetAll();
                List<StdDto> res = new List<StdDto>();
                foreach (var item in Students)
                {
                    var student = new StdDto { Age = item.Age, Name = item.Name, };

                    res.Add(student);
                }

                return Ok(res);
            }

            [HttpGet("{id:int}")]
            public IActionResult GetById(int id)
            {
                Student Student = unit.StdRepo.GetById(id);
                if (Student == null)
                    return NotFound();
                else
                {
                    var item = new StdDto()
                    {
                        Age = Student.Age,
                        Name = Student.Name,
                    };

                    return Ok(item);
                }
            }

            [HttpGet("{name}")]
            public IActionResult GetByName(string name)
            {
                var Student = unit.StdRepo.GetByName(c => c.Name.Contains(name));
                if (Student == null)
                    return NotFound();
                else return Ok(Student);
            }

            [HttpPost]
            public IActionResult Add(StdDto dto)
            {
                if (!ModelState.IsValid)
                    return BadRequest("Invalid Input!");
                else
                {
                    Student Student = new Student()
                    {
                        Age = dto.Age,
                        Name = dto.Name,
                        
                    };

                    unit.StdRepo.Add(Student);
                    unit.StdRepo.Save();

                    return Created();
                }
            }

            [HttpDelete]
            public IActionResult Delete(int id)
            {
                var crs = unit.StdRepo.GetById(id);
                if (crs == null)
                    return NotFound();
                else
                {
                    unit.StdRepo.Delete(crs);
                    unit.StdRepo.Save();
                    return NoContent();
                }
            }

            [HttpPatch("{id}")]
            public IActionResult Update(int id, [FromBody] Student Student)
            {
                if (!ModelState.IsValid)
                    return BadRequest("SomeThing Went Wrong!");

                var oldcrs = unit.StdRepo.GetById(id);
                if (oldcrs == null)
                    return NotFound();

                if (!string.IsNullOrWhiteSpace(Student.Name))
                    oldcrs.Name = Student.Name;

                unit.StdRepo.Save();
                return Ok(oldcrs);
            }


        [HttpPost("enroll")]
        [Authorize]
        public IActionResult EnrollInCourse([FromBody] EnrollmentDto dto)
        {
            var studentIdClaim = User.FindFirstValue("StudentId");

            if (string.IsNullOrWhiteSpace(studentIdClaim) || !int.TryParse(studentIdClaim, out int studentId))
                return BadRequest(new { Message = "Student ID not found in token" });

            var student = unit.StdRepo.GetById(studentId);
            if (student == null)
                return NotFound(new { Message = "Student Not Found" });

            var course = unit.CrsRepo.GetById(dto.CourseId);
            if (course == null)
                return NotFound(new { Message = "Course Not Found" });

            var existingEnrollment = unit.EnrRepo.GetAll()
                .FirstOrDefault(e => e.StudentId == studentId && e.CourseId == dto.CourseId);

            if (existingEnrollment != null)
                return BadRequest(new { Message = "Already enrolled in this course" });

            var enrollment = new Enrollment
            {
                StudentId = studentId,
                CourseId = dto.CourseId,
                EnrollmentDate = DateTime.UtcNow,
                Grade = null // Grade not assigned yet
            };

            unit.EnrRepo.Add(enrollment);
            unit.EnrRepo.Save();

            return Ok(new { Message = "Enrolled successfully", Enrollment = enrollment });
        }

        


        }
}
