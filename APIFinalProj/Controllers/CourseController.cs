using APIFinalProj.DTOs;
using APIFinalProj.Models;
using APIFinalProj.UOF;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIFinalProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        UnitOfWork unit;

        public CourseController(UnitOfWork unit)
        {
            this.unit = unit;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var courses = unit.CrsRepo.GetAll();
            List<CrsDto> res = new List<CrsDto>();
            foreach (var item in courses)
            {
                var course = new CrsDto { Id = item.Id, Name = item.Name, Duration = item.Duration };

                res.Add(course);
            }

            return Ok(res);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetById(int id)
        {
            Course course = unit.CrsRepo.GetById(id);
            if (course == null)
                return NotFound();
            else
            {
                var item = new CrsDto()
                {
                    Id = course.Id,
                    Name = course.Name,
                    Duration = course.Duration
                };

                return Ok(item);
            }
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var course = unit.CrsRepo.GetByName(c => c.Name.Contains(name));
            if (course == null)
                return NotFound();
            else return Ok(course);
        }

        [HttpPost]
        public IActionResult Add(CrsDto dto)
        {
            if(!ModelState.IsValid)
                return BadRequest("Invalid Input!");
            else
            {
                Course course = new Course()
                {
                    Id = dto.Id,
                    Name = dto.Name,
                    Duration = dto.Duration ?? 0
                };
                course.Code = $"CRS{course.Name}{DateTime.Now:MMddyy}".Substring(0, 8);

                unit.CrsRepo.Add(course);
                unit.CrsRepo.Save();

                return Created();
            }
        }

        [HttpDelete]
        public IActionResult Delete(int id)
        {
            var crs = unit.CrsRepo.GetById(id);
            if(crs == null)
                return NotFound();
            else
            {
                unit.CrsRepo.Delete(crs);
                unit.CrsRepo.Save();
                return NoContent();
            }
        }

        [HttpPatch("{id}")]
        public IActionResult Update(int id, [FromBody] Course course)
        {
            if (!ModelState.IsValid)
                return BadRequest("SomeThing Went Wrong!");

            var oldcrs = unit.CrsRepo.GetById(id);
            if (oldcrs == null)
                return NotFound();

            if(!string.IsNullOrWhiteSpace(course.Name))
                oldcrs.Name = course.Name;

            unit.CrsRepo.Save();
            return Ok(oldcrs);
        }
    }
}
