using Application.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Security.Claims;
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
        private readonly FileService _fileService;

        public AssignmentController(AssignmentInventoryService assignmentInventoryService, ILogger<AuthenticationController> logger,AssignmentService assignmentService, FileService fileService)
        {
            _assignmentInventoryService = assignmentInventoryService;
            _assignmentService = assignmentService;
            _logger = logger;
            _fileService = fileService;
        }


        [HttpGet]
        [Authorize]
        //[Authorize(Policy = IdentityData.TeacherUserPolicyName)]
        public async Task<ActionResult<string>> GetAssignment([FromQuery] string CourseName, string LessonTitle)
        {
            var result = await this._assignmentInventoryService.GetAssignment(CourseName, LessonTitle);

            return Ok(result);
        }

        
        [HttpPost]
        [Authorize(Policy = IdentityData.TeacherUserPolicyName)]
        public async Task<ActionResult<bool>> AddAssignment([FromQuery] AssignmentContentContract assignmentContract, string CourseName, string LessonTitle, IFormFile file)
        {
			try
			{
				var checkFile = await this._fileService.GetFile(file.FileName);

				return Ok(await this._assignmentInventoryService.AddAssignment(CourseName, LessonTitle, assignmentContract.MapTestToDomain(), file.FileName));
			}
			catch (Exception ex)
			{
				var saveFileResult = await this._fileService.SaveFile(file);
				if (saveFileResult == true)
					return Ok(await this._assignmentInventoryService.AddAssignment(CourseName, LessonTitle, assignmentContract.MapTestToDomain(), file.FileName));
			}
			//var resultSaveFile = await this._fileService.SaveFile(file);
           /* if (resultSaveFile) 
            {
                var result = await this._assignmentInventoryService.AddAssignment(CourseName, LessonTitle, assignmentContract.MapTestToDomain(), file.FileName);
                return Ok(result);
            }*/
                
            return Ok(false);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<AssignmentSolutionDo>> GetSolution([FromQuery] string CourseName, string LessonTitle,string StudentEmail)
        {
            var result = await this._assignmentService.GetStudentSolution(CourseName, LessonTitle, StudentEmail);

            return Ok(result);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<bool>> Solve([FromQuery] AssignmentSolutionContract solutionContract, string CourseName, string LessonTitle, IFormFile file)
        {
			string StudentEmail = User.FindFirstValue(ClaimTypes.Email);

			var resultSaveFile = await this._fileService.SaveFile(file);
            if (resultSaveFile)
            {
                var result = await this._assignmentService.SolveAssignment(CourseName, LessonTitle, StudentEmail, solutionContract.MapTestToDomain(),file.FileName);
                return Ok(result);
            }
            return Ok(resultSaveFile);
            
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<AssignmentGradeDo>>> GetAllAssignmentsSent([FromQuery] string CourseName, string LessonTitle)
        {
            var result = await this._assignmentService.GetAllAssignmentsSent(CourseName, LessonTitle);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<AssignmentGradeDo>>> GetGrade([FromQuery] string CourseName, string LessonTitle, string StudentEmail)
        {
            var result = await this._assignmentService.GetGrade(CourseName, LessonTitle, StudentEmail);
            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = IdentityData.TeacherUserPolicyName)]
        public async Task<ActionResult<bool>> GradeAssignment([FromQuery] string CourseName, string LessonTitle, double Grade, string StudentEmail)
        {
            var result = await this._assignmentService.GradeAssignment(CourseName, LessonTitle, Grade, StudentEmail);
            return Ok(result);
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<List<AssignmentGradeDo>>> GetStudentAssignments([FromQuery] string CourseName , string StudentEmail)
        {
            var result = await this._assignmentInventoryService.GetStudentAssignments(CourseName, StudentEmail);
            return Ok(result);
        }

        [HttpPatch]
        [Authorize(Policy = IdentityData.TeacherUserPolicyName)]
        public async Task<ActionResult<bool>> EditAssignment([FromQuery] AssignmentContentContract Content, string CourseName, string LessonTitle, IFormFile file)
        {
			try
			{
				var checkFile = await this._fileService.GetFile(file.FileName);

				return Ok(await this._assignmentInventoryService.EditAssignment(CourseName, LessonTitle, Content.MapTestToDomain(), file.FileName));
			}
			catch (Exception ex)
			{
				var saveFileResult = await this._fileService.SaveFile(file);
				if (saveFileResult == true)
					return Ok(await this._assignmentInventoryService.EditAssignment(CourseName, LessonTitle, Content.MapTestToDomain(), file.FileName));
			}

			/*if (file != null)
            {
                var resultSaveFile = await this._fileService.SaveFile(file);
                if (resultSaveFile)
                {
                    var result = await this._assignmentInventoryService.EditAssignment(CourseName, LessonTitle, Content.MapTestToDomain(), file.FileName);
                    return Ok(result);
                }
				return Ok(resultSaveFile);
			}*/
           return Ok(false);
        }

        [HttpDelete]
        [Authorize(Policy = IdentityData.TeacherUserPolicyName)]
        public async Task<ActionResult<bool>> DeleteAssignment([FromQuery]string CourseName, string LessonTitle)
        {
            var result = await this._assignmentInventoryService.DeleteAssignment(CourseName, LessonTitle);
            return Ok(result);

        }

    }
}
