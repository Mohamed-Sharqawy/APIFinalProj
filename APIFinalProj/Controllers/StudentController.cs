using APIFinalProj.DTOs;
using APIFinalProj.Models;
using APIFinalProj.UOF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
        }
}
