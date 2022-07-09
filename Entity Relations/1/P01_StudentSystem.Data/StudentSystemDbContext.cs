using System;
using Microsoft.EntityFrameworkCore;

namespace P01_StudentSystem.Data
{
    public class StudentSystemDbContext : DbContext
    {
        public StudentSystemDbContext()
        {
            
        }

        public StudentSystemDbContext(DbContextOptions options)
        : base(options)
        {
            
        }

        public DbSet<Student>
    }
}
