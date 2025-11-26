using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using OnlineCoursesPlatform.Entities;
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


}
