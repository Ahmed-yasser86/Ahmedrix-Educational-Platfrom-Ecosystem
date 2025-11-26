using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc.Rendering;
using OnlineCoursesPlatform.Interfaces;

namespace OnlineCoursesPlatform.Extensions
{
    public static class ConvertExtensions
    {

        public static List<SelectListItem> ConvertToSelectedListItems<T>(this IEnumerable<T> items, int selectedItemIndex) where T : IPrimaryProperty
        {
            return items.Select(item => new SelectListItem
            {
                Text = item.Title,
                Value = item.Id.ToString(),
                Selected = item.Id == selectedItemIndex
            }).ToList();

        }




    }
}
