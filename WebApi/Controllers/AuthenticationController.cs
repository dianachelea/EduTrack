using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

using Application.Services;
using WebApiContracts;
using WebApiContracts.Mappers;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("[controller]/[action]")]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthorizationService _authorizationService;
        private readonly UserService _userService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(AuthorizationService authorizationService, UserService userService, ILogger<AuthenticationController> logger)
        {
            _authorizationService = authorizationService;
            _logger = logger;
            _userService = userService;
		}

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> RegisterUser([FromQuery] RegisterUserCredentialsContract credentialsContract)
        {
            var result = await this._userService.RegisterUser(credentialsContract.MapToUserRegister());
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Policy = IdentityData.AdminUserPolicyName)]
        public async Task<ActionResult<bool>> RegisterTeacher([FromQuery] RegisterUserCredentialsContract credentialsContract)
        {
            var result = await this._userService.RegisterTeacher(credentialsContract.MapToUserRegister());
            return Ok(result);
        }
		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult<bool>> DevRegisterAdmin([FromQuery] RegisterUserCredentialsContract credentialsContract)
		{
			var result = await this._userService.RegisterAdmin(credentialsContract.MapToUserRegister());
			return Ok(result);
		}

		[HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<string>> LoginUser([FromQuery] LoginUserCredentialsContract credentialsContract)
        {
            var result = await this._authorizationService.LoginUser(credentialsContract.MapToUserCredentials());

            _logger.LogInformation("Logged in as user: "+result.Username);

            return Ok(result);
        }

        [HttpPost]
        [Authorize(Policy = IdentityData.AdminUserPolicyName)]
        public async Task<ActionResult<string>> GiveUserAdminRights([FromQuery] string email)
        {
            var result = await this._authorizationService.GiveUserAdminRights(email);

            return Ok(result);
        }

        [HttpGet]
        public ActionResult<string> TestMethod()
        {
            return Ok("Test works!");
        }

        [HttpGet]
        [Authorize(Policy = IdentityData.AdminUserPolicyName)]
        public ActionResult<string> TestMethod2()
        {
            return Ok("Test works!");
        }
    }
}
