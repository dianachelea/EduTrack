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
	public class CourseController : ControllerBase
	{

		private readonly AuthorizationService _authorizationService;
		private readonly CourseService _courseService;
		private readonly CourseInventoryService _courseInventoryService;

		public CourseController(AuthorizationService authorizationService, CourseService courseService, 
			CourseInventoryService courseInventoryService) {
			this._authorizationService = authorizationService;
			this._courseService = courseService;	
			this._courseInventoryService = courseInventoryService;
		}

		[HttpGet]
		[AllowAnonymous]
		public async Task<ActionResult<List<CourseDisplay>>> GetAllCourses([FromQuery] CourseFilterContract filterContract)
		{
			
			List<CourseDisplay> result = await this._courseInventoryService.GetAllCourses(filterContract.MapToCourseFilter());

			if (result.Count == 0)
			{
				return NotFound();
			}
			return Ok(result);

		}


	}
}
