using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using WebApiContracts;
using WebApiContracts.Mappers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class FeedbackController : ControllerBase
    {
        private readonly FeedbackService _feedbackService;

        public FeedbackController(FeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> AddFeedback([FromQuery] FeedbackContract feedbackContract)
        {
            var result = await this._feedbackService.AddFeedback(feedbackContract.MapToFeedback());
            return Ok(result);
        }
    }
}
