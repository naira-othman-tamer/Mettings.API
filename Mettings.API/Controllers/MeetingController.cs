using MassTransit;
using Meetings.Domain;
using Meetings.Infrastructure.Data;
using Mettings.API.Messages;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Mettings.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MeetingController : ControllerBase
    {
        private readonly ILogger<MeetingController> _logger;
        private readonly ISendEndpointProvider _sendEndPointProvider;
        private readonly IPublishEndpoint _publishEndpoint;
        private readonly AppDbContext _dbContext;

        public MeetingController(ILogger<MeetingController> logger, ISendEndpointProvider sendEndPointProvider, IPublishEndpoint publishEndpoint, AppDbContext dbContext)
        {
            _logger = logger;
            _sendEndPointProvider = sendEndPointProvider;
            _publishEndpoint = publishEndpoint;
            _dbContext = dbContext;
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleMeeting([FromBody] MeetingDto meetingDto)
        {

            //prepare queue 
            var endPoint = await _sendEndPointProvider.GetSendEndpoint(new Uri("queue:notify-recipients"));

            //send message
            await endPoint.Send<INotifyRecipientsMessage>(new 
            { 
                MeetingID = Guid.NewGuid(),
                ParticipantEmails = new List<string>
                {
                    "Test1@gmail.com",
                    "Test2@gmail.com",
                    "Test3@gmail.com"
                },
                ScheduledTime = DateTime.UtcNow
            
            });

            // Logic to create a meeting
            return Ok("Meeting Schedualed successfully.");
        }


        [HttpPost]
        public async Task<IActionResult> ScheduleMeetingUsingOutbox([FromBody] MeetingDto meetingDto)
        {
            using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                //create A new Meeting Entity
                var meeting = new Meeting
                {
                    Id = Guid.NewGuid(),
                    ScheduledTime = DateTime.UtcNow,
                    ParticipantEmails = string.Join(",", meetingDto.ParticipantEmails)
                };

                //save to Database
                _dbContext.Meetings.Add(meeting);
                await _dbContext.SaveChangesAsync();


                // prepare event data
                var command = new NotifyRecipientsMessage
                {
                    MeetingID = meeting.Id,
                    ParticipantEmails = meeting.ParticipantEmails.Split(",").ToList(),
                    ScheduledTime = meeting.ScheduledTime
                };

                //prepare queue 
                var endPoint = await _sendEndPointProvider.GetSendEndpoint(new Uri("queue:notify-recipients"));

                await endPoint.Send(command);
                await _dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new { message = "Meeting scheduled successfully." });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while scheduling meeting.");
                await transaction.RollbackAsync();
                return StatusCode(500, ex.Message);
            }
        }
    


        [HttpPost]
        public async Task<IActionResult> PublishMeeting()
        {
            //apply Validation And AddMeeting 
            await Task.CompletedTask;

           await _publishEndpoint.Publish<IMeetingScheduledMessage>(new 
            {
               MeetingID = Guid.NewGuid(),
               ParticipantEmails = new List<string>
                {
                    "Test1@gmail.com",
                    "Test2@gmail.com",
                    "Test3@gmail.com"
                },
               ScheduledTime = DateTime.UtcNow
           });

            // Logic to create a meeting
            return Ok("Meeting Schedualed successfully.");
        }
    }



    public class MeetingDto
    {
        public string Title { get; set; }
        public DateTime ScheduledTime { get; set; }
        public List<string> ParticipantEmails { get; set; }
    }
}

