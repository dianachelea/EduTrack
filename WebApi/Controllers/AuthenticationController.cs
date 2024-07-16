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
        private readonly NotificationService _notificationSender;
        private readonly UserService _userService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(AuthorizationService authorizationService, UserService userService, NotificationService notificationSender, ILogger<AuthenticationController> logger)
        {
            _authorizationService = authorizationService;
            _notificationSender = notificationSender;
            _logger = logger;
            _userService = userService;
		}

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<bool>> RegisterUser([FromBody] RegisterUserCredentialsContract credentialsContract)
        {
            var result = await this._userService.RegisterUser(credentialsContract.MapToUserRegister());
            return Ok(result);
        }
        [HttpPost]
        [Authorize(Policy = IdentityData.AdminUserPolicyName)]
        public async Task<ActionResult<bool>> RegisterTeacher([FromBody] RegisterUserCredentialsContract credentialsContract)
        {
            var result = await this._userService.RegisterTeacher(credentialsContract.MapToUserRegister());
            return Ok(result);
        }
		[HttpPost]
		[AllowAnonymous]
		public async Task<ActionResult<bool>> DevRegisterAdmin([FromBody] RegisterUserCredentialsContract credentialsContract)
		{
			var result = await this._userService.RegisterAdmin(credentialsContract.MapToUserRegister());
			return Ok(result);
		}

		[HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<string>> LoginUser([FromBody] LoginUserCredentialsContract credentialsContract)
        {
            var result = await this._authorizationService.LoginUser(credentialsContract.MapToUserCredentials());

            _logger.LogInformation("Logged in as user: "+result.Username);

            return Ok(result);
        }
		[HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<string>> RecoverPassword([FromQuery] string email)
        {
            var result = await this._userService.RecoverPassword(email);

            _logger.LogInformation(email + " wants to recover his password");

            return Ok(result);
        }
		[HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult<string>> ResetPassword([FromQuery] string token,[FromBody] string password)
        {
            var result = await this._userService.ResetPassword(token, password);

            _logger.LogInformation("Password reseted with succes for token: " + token);

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
        public async Task<ActionResult<string>> TestMethod()
        {
            _notificationSender.NotifyTeacher("asmarandei.catalin@yahoo.com", "Test notification message");
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
