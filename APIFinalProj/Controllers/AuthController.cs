using APIFinalProj.Context;
using APIFinalProj.DTOs;
using APIFinalProj.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

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
            var existingUser = await _userManager.FindByEmailAsync(dto.Email);
            if(existingUser != null)
            {
                return BadRequest(new RegisterResponseDto
                {
                    Successe = false,
                    Message = "Email already registered"
                });
            }

            var user = new ApplicationUser
            {
                UserName = dto.Name,
                Email = dto.Email
            };

            var result = await _userManager.CreateAsync(user, dto.Password);

            if (!result.Succeeded)
            {
                return BadRequest(new RegisterResponseDto
                {
                    Successe = false,
                    Message = string.Join(", ", result.Errors.Select(e => e.Description))
                });
            }
                

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

            return Ok(new RegisterResponseDto
            {
                Successe = true,
                Message = "Student Registerd Successfully",
                StudentId = student.Id,
                UserId = user.Id
            });
        }

        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] LoginDto dto)
        {
            var user = await _userManager.FindByEmailAsync(dto.Email);
            if(user == null)
            {
                return Unauthorized(new { Message = "Invalid email or Passowrd" });
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, dto.Password, false);
            if(!result.Succeeded)
            {
                return Unauthorized(new { Message = "Invalid email or Password" });
            }

            var student = await _appDbContext.Students.FirstOrDefaultAsync(s => s.UserId == user.Id);

            var token = await GenerateJwtToken(user);

            return Ok(new LoginResponseDto
            {
                Token = token,
                Expiration = DateTime.UtcNow.AddMinutes(
                    int.Parse(_config["JwtSettings:ExpiryInMinutes"])),
                User = new UserInfoDto
                {
                    UserId = user.Id,
                    Email = user.Email,
                    StudentId = student?.Id,
                    StudentName = student?.Name
                }
            });
        }

        private async Task<string> GenerateJwtToken(ApplicationUser user)
        {
            var jwtSetttings = _config.GetSection("JwtSettings");
            var secretKey = jwtSetttings["SecretKey"];
            var Issuer = jwtSetttings["Issuer"];
            var audience = jwtSetttings["Audience"];
            var expiryInMinutes = int.Parse(jwtSetttings["ExpiryInMinutes"]);

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            if (user.StudentId.HasValue)
            {
                claims.Add(new Claim("StudentId", user.StudentId.Value.ToString()));
            }

            var roles = await _userManager.GetRolesAsync(user);
            foreach(var role in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, role));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: Issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiryInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
