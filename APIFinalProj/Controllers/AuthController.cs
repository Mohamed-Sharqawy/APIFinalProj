using APIFinalProj.Context;
using APIFinalProj.DTOs;
using APIFinalProj.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace APIFinalProj.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        UserManager<ApplicationUser> _userManager;
        SignInManager<ApplicationUser> _signInManager;
        AppDbContext _appDbContext;
        IConfiguration _config;

        public AuthController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, AppDbContext appDbContext, IConfiguration config)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _appDbContext = appDbContext;
            _config = config;
        }

        [HttpPost("register-student")]
        public async Task<IActionResult> RegisterStudent([FromBody] RegisterStudentDto dto)
        {
            var user = new ApplicationUser
            {
                UserName = dto.Name,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
                return BadRequest(result.Errors);

            var student = new Student
            {
                Name = dto.Name,
                Age = dto.Age,
                UserId = user.Id
            };

            _appDbContext.Students.Add(student);
            await _appDbContext.SaveChangesAsync();

            user.StudentId = student.Id;
            await _appDbContext.SaveChangesAsync();

            return Ok(new
            {
                Message = "Student Registerd Successfully",
                StudentId = student.Id
            });
        }
    }
}
