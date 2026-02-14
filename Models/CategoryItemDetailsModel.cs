namespace OnlineCoursesPlatform.Models
{
    public class CategoryItemDetailsModel
    {
        public int CategoryId { get; set; }
        public int CategoryItemId { get; set; }
        public string CategoryTitle { get; set; }

        public string CategoryItemTitle { get; set; }
        public string CategoryItemDescription { get; set; }

        public string? MediaImagePath { get; set; }



    }
}
