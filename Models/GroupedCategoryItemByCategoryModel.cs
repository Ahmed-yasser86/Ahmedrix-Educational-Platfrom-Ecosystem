namespace OnlineCoursesPlatform.Models
{
    public class GroupedCategoryItemByCategoryModel
    {

        public int Id { get; set; }

        public string Title { get; set; }

        public IGrouping<int , CategoryItemDetailsModel> Items { get; set; }

    }
}
