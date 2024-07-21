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
    public class AssignmentController : ControllerBase
    {
        private readonly AssignmentInventoryService _assignmentInventoryService;
        private readonly AssignmentService _assignmentService;
        private readonly ILogger<AuthenticationController> _logger;

        public AssignmentController(AssignmentInventoryService assignmentInventoryService, ILogger<AuthenticationController> logger,AssignmentService assignmentService)
        {
            _assignmentInventoryService = assignmentInventoryService;
            _assignmentService = assignmentService;
            _logger = logger;
        }


        [HttpGet]
        //[Authorize(Policy = IdentityData.TeacherUserPolicyName)]
        public async Task<ActionResult<string>> GetAssignment([FromQuery] string CourseName, string LessonTitle)
        {
            var result = await this._assignmentInventoryService.GetAssignment(CourseName, LessonTitle);

            return Ok(result);
        }

        
        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> AddAssignment([FromQuery] AssignmentContentContract assignmentContract, string CourseName, string LessonTitle)
        {
            var result = await this._assignmentInventoryService.AddAssignment(CourseName, LessonTitle, assignmentContract.MapTestToDomain());
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult<AssignmentSolutionDo>> GetSolution([FromQuery] string CourseName, string LessonTitle,string StudentEmail)
        {
            var result = await this._assignmentService.GetStudentSolution(CourseName, LessonTitle, StudentEmail);

            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> Solve([FromQuery] AssignmentSolutionContract solutionContract, string CourseName, string LessonTitle, string StudentEmail)
        {
            var result = await this._assignmentService.SolveAssignment(CourseName, LessonTitle, StudentEmail, solutionContract.MapTestToDomain());
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<AssignmentGradeDo>>> GetAllAssignmentsSent([FromQuery] string CourseName, string LessonTitle)
        {
            var result = await this._assignmentService.GetAllAssignmentsSent(CourseName, LessonTitle);
            return Ok(result);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<AssignmentGradeDo>>> GetGrade([FromQuery] string CourseName, string LessonTitle, string StudentEmail)
        {
            var result = await this._assignmentService.GetGrade(CourseName, LessonTitle, StudentEmail);
            return Ok(result);
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> GradeAssignment([FromQuery] string CourseName, string LessonTitle, double Grade, string StudentEmail)
        {
            var result = await this._assignmentService.GradeAssignment(CourseName, LessonTitle, Grade, StudentEmail);
            return Ok(result);
        }

    }
}
