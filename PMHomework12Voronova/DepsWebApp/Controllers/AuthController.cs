using System.Net;
using System.Threading.Tasks;
using DepsWebApp.Models;
using DepsWebApp.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DepsWebApp.Controllers
{
    /// <summary>
    /// Authentication Controller for registration
    /// </summary>
    [AllowAnonymous]
    [ApiController]
    [Route("[controller]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _accountService;

        /// <summary>
        /// RatesController constructor.
        /// </summary>
        /// <param name="accountService">Account service.</param>
        public AuthController(IAuthService accountService)
        {
            _accountService = accountService;
        }

        /// <summary>
        /// Method to register user account
        /// </summary>
        /// <param name="request">User data from request body.</param>
        /// <returns>Registration attempt result.</returns>
        [HttpPost("register")]
        [ProducesResponseType((int)HttpStatusCode.OK)]
        [ProducesResponseType(typeof(Error), (int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.Conflict)]
        public async Task<ActionResult<string>> Register([FromBody]AuthRequest request)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var account = await _accountService.RegisterAsync(request.Login, request.Password);

            if (account == null) return Conflict("User already exists.");

            return Ok($"User {request.Login} registered successfully. " + account);
        }
    }
}
