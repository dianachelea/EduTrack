using Application.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebApiContracts;
using WebApiContracts.Mappers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AssignmentController : ControllerBase
    {
        private readonly AssignmentInventoryService _assignmentService;
        private readonly ILogger<AuthenticationController> _logger;

        public AssignmentController(AssignmentInventoryService assignmentService, ILogger<AuthenticationController> logger)
        {
            _assignmentService = assignmentService;
            _logger = logger;
        }


        [HttpGet]
        //[Authorize(Policy = IdentityData.TeacherUserPolicyName)]
        public async Task<ActionResult<string>> GetAssignment([FromQuery] string coursename, string lessontitle)
        {
            var result = await this._assignmentService.GetAssignment(coursename, lessontitle);

            return Ok(result);
        }

        
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> AddAssignment([FromQuery] AssignmentContentContract assignmentContract, string coursename, string lessontitle)
        {
            var result = await this._assignmentService.AddAssignment(coursename, lessontitle, assignmentContract.MapTestToDomain());
            return Ok(result);
        }
        
    }
}
