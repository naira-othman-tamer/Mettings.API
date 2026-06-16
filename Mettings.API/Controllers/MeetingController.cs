using MassTransit;
using Mettings.API.Messages;
using Microsoft.AspNetCore.Mvc;

namespace Mettings.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MeetingController : ControllerBase
    {
      

        private readonly ILogger<MeetingController> _logger;
        private readonly ISendEndpointProvider _sendEndPointProvider;
        private readonly IPublishEndpoint _publishEndpoint;

        public MeetingController(ILogger<MeetingController> logger, ISendEndpointProvider sendEndPointProvider, IPublishEndpoint publishEndpoint)
        {
            _logger = logger;
            _sendEndPointProvider = sendEndPointProvider;
            _publishEndpoint = publishEndpoint;
        }

        [HttpPost]
        public async Task<IActionResult> ScheduleMeeting([FromBody] MeetingDto meeting)
        {
            //apply Validation And AddMeeting 
            await Task.CompletedTask;

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
