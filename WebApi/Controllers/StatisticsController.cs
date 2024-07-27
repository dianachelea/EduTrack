using Application.Services;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using System.Security.Claims;
using WebApiContracts;
using WebApiContracts.Mappers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class StatisticsController : ControllerBase
    {
        private readonly StatisticsService _statisticsService;
        private readonly AuthorizationService _authorizationService;

        public StatisticsController(StatisticsService statisticsService, AuthorizationService authorizationService)
        {
            _statisticsService = statisticsService;
            _authorizationService = authorizationService;
        }

        [HttpGet]
        [Authorize]
        public async Task<ActionResult<StatisticsDO>> GetStudentStats()
        {
            //var email = "student1@example.com";
            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _statisticsService.GetStudentStats(email);

            return Ok(result);
        }

        [HttpGet]
        [Authorize(Policy = IdentityData.TeacherUserPolicyName)]
        public async Task<ActionResult<StatisticsDO>> GetTeacherStats()
        {
            //var email = "teacher1@example.com";
            var claims = User.Claims;
            foreach (var claim in claims)
            {
                Console.WriteLine($"{claim.Type}: {claim.Value}");
            }

            var email = User.FindFirstValue(ClaimTypes.Email);
            var result = await _statisticsService.GetTeacherStats(email);

            return Ok(result);
        }
    }
}
