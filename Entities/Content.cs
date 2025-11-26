using System.ComponentModel.DataAnnotations.Schema;

namespace OnlineCoursesPlatform.Entities;

public class Content
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string HTMLContent { get; set; }
    public string VideoLink { get; set; }

    [NotMapped]
    public int CatItemId { get; set; }


    [NotMapped]

    public int CategoryId { get; set; }

    // Navigation property
    public virtual CategoryItem CategoryItem { get; set; }


}
