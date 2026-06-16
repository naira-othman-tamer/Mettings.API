namespace Meetings.Domain
{
    public class Meeting
    {
        public Guid Id { get; set; }

        public DateTime ScheduledTime { get; set; } = DateTime.Now;
        public string? ParticipantEmails { get; set; }
    }
}
