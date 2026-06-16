using Microsoft.AspNetCore.Mvc;

namespace Mettings.API.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class MeetingController : ControllerBase
    {
      

        private readonly ILogger<MeetingController> _logger;

        public MeetingController(ILogger<MeetingController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public async Task<IActionResult> ScedhualMeeting([FromBody] MeetingDto meeting)
        {
            //apply Validation And AddMeeting 
            await Task.CompletedTask;
            // Logic to create a meeting
            return Ok("Meeting Schedualed successfully.");
        }
    }

    public class MeetingDto
    {
    }
}
