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
        private readonly AuthorizationService _authorizationService;

        public FeedbackController(FeedbackService feedbackService, AuthorizationService authorizationService)
        {
            _feedbackService = feedbackService;
            _authorizationService = authorizationService;
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> AddFeedback([FromQuery] FeedbackContract feedbackContract)
        {
            var result = await _feedbackService.AddFeedback(feedbackContract.MapToFeedback());
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]    //[Authorize]  //(Policy = IdentityData.TeacherUserPolicyName)]
        public async Task<ActionResult<string>> GetFeedback([FromQuery] FeedbackFiltersContract feedbackFiltersContract)
        {
            var result = await _feedbackService.GetFeedback(feedbackFiltersContract.MapToFeedbackFilters());

            return Ok(result);
        }
    }
}
