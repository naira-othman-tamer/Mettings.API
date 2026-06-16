using MassTransit;
using Mettings.API.Messages;

namespace Mettings.Worker.Consumers
{
    public class LogMeetingDetailsSecondaryConsumer : IConsumer<IMeetingScheduledMessage>
    {
        public async Task Consume(ConsumeContext<IMeetingScheduledMessage> context)
        {
            var command = context.Message;
            Console.WriteLine($"Notifying Recipinets for Meeting ID: {command.MeetingID}");

            //Simulate Sendenig Email for each Recipient
            foreach (var email in command.ParticipantEmails)
            {
                Console.WriteLine($"[Worker] Sending email to: {email} for Meeting at: {command.ScheduledTime}");

                //Insert Actual email sending logic here using SMTP or any other email service provider
            }

            await Task.CompletedTask;
        }
    }
}
