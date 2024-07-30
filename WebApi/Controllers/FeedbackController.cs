using Application.Services;
using Domain;
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

        public FeedbackController(FeedbackService feedbackService, AuthorizationService authorizationService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> AddFeedback([FromBody] FeedbackContract feedbackContract)
        {
            var result = await _feedbackService.AddFeedback(feedbackContract.MapToFeedback());
            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = IdentityData.TeacherUserPolicyName)]
        public async Task<ActionResult<string>> GetFeedback([FromQuery] FeedbackFiltersContract feedbackFiltersContract)
        {
            var result = await _feedbackService.GetFeedback(feedbackFiltersContract.MapToFeedbackFilters());

            return Ok(result);
        }
    }
}
