using Abp.Zero.EntityFrameworkCore;
using TOEICReading4.Authorization.Roles;
using TOEICReading4.Authorization.Users;
using TOEICReading4.MultiTenancy;
using Microsoft.EntityFrameworkCore;
using TOEICReading4.Exams; // <-- THÊM DÒNG NÀY ĐỂ NHẬN DIỆN CÁC ENTITY

namespace TOEICReading4.EntityFrameworkCore;

public class TOEICReading4DbContext : AbpZeroDbContext<Tenant, Role, User, TOEICReading4DbContext>
{
    /* Define a DbSet for each entity of the application */
    
    // THÊM 3 DÒNG NÀY:
    public DbSet<Exam> Exams { get; set; }
    public DbSet<Passage> Passages { get; set; }
    public DbSet<Question> Questions { get; set; }
    public DbSet<ExamAttempt> ExamAttempts { get; set; }

    public TOEICReading4DbContext(DbContextOptions<TOEICReading4DbContext> options)
        : base(options)
    {
    }
}
