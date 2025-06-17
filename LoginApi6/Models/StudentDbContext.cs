using Microsoft.EntityFrameworkCore;
namespace LoginApi6.Models
{
    public class StudentDbContext: DbContext
    {
        public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options) { }
        public DbSet<RegisterStudent> Registration { get; set; }
    }
}
