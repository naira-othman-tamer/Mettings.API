namespace Mettings.API.Messages
{
    public interface MeetingScheduledMessage
    {
        public Guid MeetingID { get; set; }
        public List<string> ParticipantEmails { get; set; }
        public DateTime ScheduledTime { get; set; }
    }
}
