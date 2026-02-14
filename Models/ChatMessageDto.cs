namespace OnlineCoursesPlatform.Models
{
    public class ChatMessageDto
    {
        public string SenderId { get; set; }
        public int RoomId { get; set; } // تأكد هل هو int ولا string
        public string Content { get; set; }
        public DateTime Timestamp { get; set; }
    }
}