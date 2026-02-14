using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Entities;
using OnlineCoursesPlatform.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace OnlineCoursesPlatform.Data;


public class ApplicationUser : IdentityUser
{
    [StringLength(250)]
    public string FirstName { set; get; }

    [StringLength(250)]
    public string LastName { set; get; }

    [StringLength(250)]
    public string Address1 { set; get; }

    [StringLength(250)]
    public string Address2 { set; get; }

    [StringLength(250)]
    public string PostCode { set; get; }


    [ForeignKey("UserId")]
    public ICollection<UserCategory> UserCategories { get; set; }


}
public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
{


    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    { }

    public DbSet<Category> Categories { get; set; }
    public DbSet<CategoryItem> CategoryItems { get; set; }
    public DbSet<Content> Contents { get; set; }
    public DbSet<MediaType> MediaTypes { get; set; }
    public DbSet<UserCategory> UserCategories { get; set; }
    public DbSet<Instructor> Instructors { get; set; }
    public DbSet<Appointment> Appointments { get; set; }
    public DbSet<Newsletter> Newsletters { get; set; }
    public DbSet<LiveSession> LiveSessions { get; set; }

    public DbSet<ChatMessage> ChatMessages { get; set; }
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Instructor>().HasData(
            new Instructor
            {
                Id = 1,
                Name = "Dr. Ahmed Ali",
                Email = "ahmed.ali@example.com",
                Description = "Expert in Software Engineering and C#."
            },
            new Instructor
            {
                Id = 2,
                Name = "Eng. Sarah Hassan",
                Email = "sarah.hassan@example.com",
                Description = "Senior Web Developer with 10 years experience."
            }
        );

        modelBuilder.Entity<Category>().HasData(
            new Category
            {
                Id = 10001,
                Title = "ASP.NET Core Masterclass",
                Description = "Complete guide to build APIs",
                ThumbnailImagePath = "/images/asp-net.jpg",
                InstructorId = 1
            },
            new Category
            {
                Id = 10002,
                Title = "Advanced React & Redux",
                Description = "Master modern frontend development",
                ThumbnailImagePath = "/images/react.jpg",
                InstructorId = 2
            }
        );
    }
}
