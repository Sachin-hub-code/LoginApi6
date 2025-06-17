using LoginApi6.Models;
//using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LoginApi6.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly StudentDbContext _context;
        public StudentController(StudentDbContext context)
        {
            _context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<RegisterStudent>>> GetAll()
        {
            return await _context.Registration.ToListAsync();

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<RegisterStudent>> GetById(int id)
        {
            var student = await _context.Registration.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }
       
        [HttpPut]
        public async Task<ActionResult<RegisterStudent>> Update(int id, RegisterStudent student)
        {

            var existingStudent = await _context.Registration.FindAsync(id);
            if (existingStudent == null)
            {
                return Ok(student);
            }

            // _context.Registration.Update(student);

            // Update the student's properties
            existingStudent.UserName = student.UserName;
            existingStudent.Email = student.Email;
            existingStudent.IsActive = student.IsActive;

            _context.SaveChanges();
            return Ok(existingStudent);
        }
         /*
        [HttpPut]
        public async Task<ActionResult<Update>> Update2(int id, Update student)
        {

            var existingStudent = await _context.Registration.FindAsync(id);
            if (existingStudent == null)
            {
                return Ok(student);
            }

            // _context.Registration.Update(student);

            // Update the student's properties
            existingStudent.UserName = student.UserName;
            existingStudent.Email = student.Email;
            existingStudent.IsActive = student.IsActive;

            _context.SaveChanges();
            return Ok(existingStudent);
        }*/

        [HttpDelete("{id}")]
        public async Task<ActionResult<RegisterStudent>> Delete(int id)
        {
            var existingStudent = await _context.Registration.FindAsync(id);
           if(existingStudent == null) 
           {
                return Ok();
           }
            _context.Registration.Remove(existingStudent);
            await _context.SaveChangesAsync();
            return Ok(existingStudent);

        }
        [HttpPost]
        [Route("Register")]
        public async Task<ActionResult<RegisterStudent>> Register(RegisterStudent student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.Email) || string.IsNullOrWhiteSpace(student.Password))
            {
                return BadRequest("Invaild register data.");
            }
            var existingStudent = await _context.Registration.FirstOrDefaultAsync(s => s.Email == student.Email);
            if (existingStudent != null)
            {
                return Conflict("Invaild email or password.");
            }
            student.Password = BCrypt.Net.BCrypt.HashPassword(student.Password);
            _context.Registration.Add(student);
            _context.SaveChanges();
            return Ok(student);
        }
        [HttpPost]
        [Route("Login")]
        public async Task<ActionResult<LoginStudent>> Login(LoginStudent student)
        {
            if (student == null || string.IsNullOrWhiteSpace(student.Email) || string.IsNullOrWhiteSpace(student.Password))
            {
                return BadRequest("Invaild login data.");
            }
            var existingStudent = await _context.Registration.FirstOrDefaultAsync(s => s.Email == student.Email);
            if (existingStudent == null)
            {
                return Unauthorized("Invaild email or password");
            }
            if (!BCrypt.Net.BCrypt.Verify(student.Password, existingStudent.Password))
            {
                return Unauthorized("Invaild email or password");
            }
            return Ok(existingStudent);
        }



    }
}
