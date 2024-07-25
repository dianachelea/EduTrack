using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Services;
using WebApiContracts;
using WebApiContracts.Mappers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Application.Interfaces;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LessonController : ControllerBase
    {
        private readonly LessonInventoryService _lessonInventory;
        private readonly LessonService _lessonService;
       // private readonly ILogger<LessonController> _logger;

        public LessonController(LessonInventoryService lessonServiceInventory, LessonService lessonService)
        {
            this._lessonService = lessonService;
			_lessonInventory = lessonServiceInventory;
            //_logger = logger;
        }

        [HttpGet]
     //   [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<LessonDisplay>>> GetAllLessons([FromQuery] string courseName)
        {
            // Apel din serivciu de courses Inventory
            var lessons = await _lessonInventory.GetAllLessons(courseName);
            return Ok(lessons);
        }

        [HttpGet]
      //  [Authorize]
        public async Task<ActionResult<Lesson>> GetLesson([FromQuery] string courseTitle, [FromQuery] string lessonTitle)
        {
            var lesson = await _lessonInventory.GetLesson(courseTitle, lessonTitle); 
            if (lesson == null)
            {
                return NotFound();
            }
            return Ok(lesson);
        }

        [HttpPost]
      //  [Authorize(Policy = IdentityData.TeacherUserPolicyName)]
        public async Task<ActionResult<bool>> AddLesson([FromQuery] string courseTitle, [FromBody] LessonContract lessonContract)
        {
            string teacherEmail = "teacher@teacher.com"; // "User.Identity.Email

			var result = await _lessonInventory.AddLesson(courseTitle, teacherEmail, lessonContract.MaptoLesson());
            return Ok(result);
        }

        [HttpPatch]
      //  [Authorize(Policy = IdentityData.TeacherUserPolicyName)]
        public async Task<ActionResult<bool>> EditLesson([FromQuery] string lessonTitle, [FromBody] LessonContract lessonContract)
        {
            var result = await _lessonInventory.UpdateLesson(lessonTitle, lessonContract.MaptoLesson());
            return Ok(result);
        }

        [HttpDelete]
     //   [Authorize(Policy = IdentityData.TeacherUserPolicyName)]
        public async Task<ActionResult<bool>> DeleteLesson([FromQuery] string courseName, [FromQuery] string lessonTitle)
        {
            var result = await _lessonInventory.DeleteLesson(courseName, lessonTitle); 
            return Ok(result);
        }

        [HttpPost]
     //   [Authorize(Policy = IdentityData.TeacherUserPolicyName)]
        public async Task<ActionResult<bool>> MakeAttendance([FromQuery] string courseName, [FromQuery] string lessonTitle, [FromBody] List<UserContract> users)
        {
            var students = users.Select(u => u.MapToUser()).ToList();
			var result = await _lessonService.MakeAttendance(courseName, lessonTitle, students); 
            return Ok(result);
        }
        [HttpGet]
     //   [Authorize]
        public async Task<ActionResult<List<Attendance>>> GetAttendance([FromQuery] string courseName)
        {
            var email = "user@yahoo.com"; // User.Identity.Email

            // Call get Student attendance from CoursesService
			var result = await _lessonService.GetSAttendance(courseName, email);
			return Ok(result);
        }
        [HttpGet]
     //   [Authorize(Policy = IdentityData.TeacherUserPolicyName)]
        public async Task<ActionResult<Dictionary<string, List<Attendance>>>> GetAllAttendace([FromQuery] string courseName)
        {
			Dictionary<string,List<Attendance>> attendance = new Dictionary<string, List<Attendance>>();
			var lessons = await _lessonInventory.GetAllLessons(courseName);
			foreach (var lesson in lessons)
			{
			    var result = await _lessonService.GetAttendance(courseName, lesson.Name);
				attendance.Add(lesson.Name, result);
			}
            return Ok(attendance);
        }

    }
}
