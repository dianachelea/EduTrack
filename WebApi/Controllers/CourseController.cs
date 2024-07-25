using Application.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.InteropServices;
using System.Security.Claims;
using WebApiContracts;
using WebApiContracts.Mappers;

namespace WebApi.Controllers
{
	[ApiController]
	[Route("[controller]/[action]")]
	public class CourseController : ControllerBase
	{

		private readonly AuthorizationService _authorizationService;
		private readonly CourseService _courseService;
		private readonly CourseInventoryService _courseInventoryService;
		private readonly FileService _fileService;


		public CourseController(AuthorizationService authorizationService, CourseService courseService,
			CourseInventoryService courseInventoryService, FileService fileService)
		{
			this._authorizationService = authorizationService;
			this._courseService = courseService;
			this._courseInventoryService = courseInventoryService;
			this._fileService = fileService;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult<List<CourseDisplay>>> GetAllCourses([FromQuery] CourseFilterContract filterContract) //check filter
		{

			List<CourseDisplay> result = this._courseInventoryService.GetAllCourses(filterContract.MapToCourseFilter());

			if (result.Count == 0)
			{
				return NotFound();
			}

			foreach (var course in result)
			{
				var file = await _fileService.GetFile(course.Image);
				course.ImageContents = file.FileContents;
			}

			return Ok(result);

		}

		[HttpPost]
		[Authorize(Policy = IdentityData.TeacherUserPolicyName)]
		public async Task<ActionResult<bool>> AddCourse([FromForm] CourseContract courseContract) //works
		{
			//var email = "teacher1@teacher.com";
			
			var email = User.FindFirstValue(ClaimTypes.Email);
			if (email == null || email.Split('@')[1] != "teacher.com")
			{
				return Forbid();
			}
			

			var checkFile = await this._fileService.GetFile(courseContract.Image.FileName);

			var result = false;

			if (checkFile == null)
			{
				var saveFileResult = await this._fileService.SaveFile(courseContract.Image);
				if (saveFileResult == true)
					result = await this._courseInventoryService.AddCourse(email, courseContract.MapToCourse());
			}
			else
			{
				result = await this._courseInventoryService.AddCourse(email, courseContract.MapToCourse());
			}

			if (result == true)
			{
				return Ok(result);
			}
			return BadRequest();
		}


		[HttpDelete]
		[Authorize(Policy = IdentityData.TeacherUserPolicyName)]
		public async Task<ActionResult<bool>> DeleteCourse([FromQuery] string courseName) //works
		{
			var email = User.FindFirstValue(ClaimTypes.Email);
			if (email == null || email.Split('@')[1] != "teacher.com")
			{
				return Forbid();
			}
			
			//var email = "teacher1@teacher.com";
			var result = await this._courseInventoryService.DeleteCourse(email, courseName);

			if (result == true)
			{
				return Ok(result);
			}
			return NotFound();
		}

		[HttpPatch]
		[Authorize(Policy = IdentityData.TeacherUserPolicyName)]
		public async Task<ActionResult<bool>> EditCourse([FromQuery] string courseName, [FromForm] CourseContract courseContract) //works
		{
			
			var email = User.FindFirstValue(ClaimTypes.Email);
			if (email == null || email.Split('@')[1] != "teacher.com")
			{
				return Forbid();
			}
			
			//var email = "teacher1@teacher.com";
			var checkFile = await this._fileService.GetFile(courseContract.Image.FileName);

			var result = false;

			if (checkFile == null)
			{
				var saveFileResult = await this._fileService.SaveFile(courseContract.Image);
				if (saveFileResult == true)
					 result = await this._courseInventoryService.UpdateCourse(email, courseName, courseContract.MapToCourse());
			}
			else
			{
				     result = await this._courseInventoryService.UpdateCourse(email, courseName, courseContract.MapToCourse());
			}

			if (result == true)
			{
				return Ok(result);
			}
			return NotFound();
		}

		

		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult<List<CourseDisplay>>> GetMostPopularCourses() //works
		{
			List<CourseDisplay> result = this._courseInventoryService.GetMostPopularCourses();

			if (result.Count == 0)
			{
				return NotFound();
			}

			foreach (var course in result)
			{
				var file = await _fileService.GetFile(course.Image);
				course.ImageContents = file.FileContents;
			}

			return Ok(result);
		}

		[HttpPost]
		[Authorize]
		public async Task<ActionResult<bool>> EnrollToCourse([FromBody] string courseName) //works
		{
			
			//get student email
			var studentEmail = User.FindFirstValue(ClaimTypes.Email);

			if (studentEmail == null || studentEmail.Split('@')[1] != "student.com")
			{
				return Forbid();

			}
			

			var result = await this._courseService.EnrollToCourse(courseName, studentEmail);

			if (result == true)
			{
				return Ok(result);
			}
			return NotFound();

		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult<CourseInfoPage>> GetCourse([FromQuery] string courseName) //works
		{
			var result = this._courseService.GetCoursePresentation(courseName); //returns partial info

			if (result != null)
			{
				var file = await _fileService.GetFile(result.Image);
				result.ImageContents = file.FileContents;
				return Ok(result);
			}

			return NotFound();

		}

		[HttpGet]
		[Authorize(Policy = IdentityData.TeacherUserPolicyName)]
		public ActionResult<List<Student>> GetStudentsEnrolled([FromQuery] string courseName) //works
		{
			
			var email = User.FindFirstValue(ClaimTypes.Email);
			if (email == null || email.Split('@')[1] != "teacher.com")
			{
				return Forbid();
			}
			
			//var email = "teacher@teacher.com";

			List<Student> result = this._courseService.GetAllStudentsEnrolled(courseName, email);

			if (result.Count == 0)
			{
				return NotFound();
			}
			return Ok(result);
		}

		[HttpGet]
		[Authorize]
		public ActionResult<List<Attendance>> GetStudentAttendance([FromQuery] string courseName) //works
		{
			//var email = "student2@student.com";
			var studentEmail = User.FindFirstValue(ClaimTypes.Email);

			if (studentEmail == null || studentEmail.Split('@')[1] != "student.com")
			{
				return Forbid();

			}

			var result = this._courseService.GetStudentAttendance(courseName, studentEmail);

			return Ok(result);
		}

		
	}
}
