using MediatR;

namespace OnlineCoursesPlatform.Notifications
{
    public class LiveStreamStartedNotification : INotification
    {
        public int CategoryId { get; set; }
        public string CategoryTitle { get; set; }
        public string StreamTitle { get; set; }
        public string InstructorName { get; set; }
    }
}