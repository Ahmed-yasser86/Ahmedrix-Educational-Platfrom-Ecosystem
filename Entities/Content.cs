using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCoursesPlatform.Entities;

public class Content
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string HTMLContent { get; set; }
    public string VideoLink { get; set; }

    [Column("CategoryItemId")]  // Maps C# property to database column
    public int CatItemId { get; set; }


    [NotMapped]

    public int CategoryId { get; set; }

    // Navigation property
    [ForeignKey("CatItemId")]
    public virtual CategoryItem CategoryItem { get; set; }


}
