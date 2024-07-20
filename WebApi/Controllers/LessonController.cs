using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Application.Services;
using WebApiContracts;
using WebApiContracts.Mappers;
using System.Collections.Generic;
using System.Threading.Tasks;
using Domain;
using Application.Interfaces;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class LessonController : ControllerBase
    {
        private readonly ILessonRepository _lessonService;
        private readonly ILogger<LessonController> _logger;

        public LessonController(ILessonRepository lessonService, ILogger<LessonController> logger)
        {
            _lessonService = lessonService;
            _logger = logger;
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<IEnumerable<LessonDisplay>>> GetAllLessons([FromQuery] string courseName)
        {
            var lessons = await _lessonService.GetLessons(courseName);
            return Ok(lessons);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<Lesson>> GetLesson([FromQuery] string courseTitle, [FromQuery] string lessonTitle)
        {
            var lesson = await _lessonService.GetLesson(courseTitle, lessonTitle); // Implement this method in your service
            if (lesson == null)
            {
                return NotFound();
            }
            return Ok(lesson);
        }

        [HttpPost]
        [Authorize(Policy = IdentityData.AdminUserPolicyName)]
        public async Task<ActionResult<bool>> AddLesson([FromQuery] string courseTitle, [FromQuery] string teacherEmail, [FromBody] LessonContract lessonContract)
        {
            var result = await _lessonService.AddLesson(courseTitle, teacherEmail, lessonContract.MaptoLesson());
            return Ok(result);
        }

        [HttpPatch]
        [Authorize(Policy = IdentityData.AdminUserPolicyName)]
        public async Task<ActionResult<bool>> EditLesson([FromQuery] string lessonTitle, [FromBody] LessonContract lessonContract)
        {
            var result = await _lessonService.UpdateLesson(lessonTitle, lessonContract.MaptoLesson());
            return Ok(result);
        }

        [HttpDelete]
        [Authorize(Policy = IdentityData.AdminUserPolicyName)]
        public async Task<ActionResult<bool>> DeleteLesson([FromQuery] string courseName, [FromQuery] string lessonTitle)
        {
            var result = await _lessonService.DeleteLesson(courseName, lessonTitle); // Implement this method in your service
            return Ok(result);
        }

    }
}
