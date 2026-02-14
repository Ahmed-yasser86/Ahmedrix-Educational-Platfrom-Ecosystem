using OnlineCoursesPlatform.Entities;
namespace OnlineCoursesPlatform.Areas.Admin.Models
{
    public class AdminStreamDashboardViewModel
    {
        public List<LiveSession> ActiveSessions { get; set; } = new();
        public List<string> RealTimeStreamingKeys { get; set; } = new List<string>();
        public List<LiveSession> PastSessions { get; set; } = new();
    }
}