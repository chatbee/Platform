using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Platform.Core;
using Platform.Core.Components.Logging;
using Platform.Core.Exceptions;
using Platform.Core.Models;
using Platform.Core.Models.Api;
using Platform.Core.Models.Api.Responses;
using Platform.Core.Services;
using Platform.Database;
using Serilog;

namespace Platform.Controllers.Api
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppSettings _appSettings;
        private readonly IUserService _userService;
        private readonly IAppLogger _logger;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1062:Validate arguments of public methods", Justification = "This is injected via DI, it should never be null")]
        public UsersController(IUserService userService, IOptions<AppSettings> appSettings, IAppLogger appLogger )
        {
            _appSettings = appSettings.Value;
            _userService = userService;
            _logger = appLogger;

        }
        // POST: api/Users/authenticate
        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult<AuthenticationResponse>> Authenticate([FromBody] AuthenticationModel model)
        {
            if (model is null)
            {
                return NotFound();
            }

            var user = _userService.Authenticate(model.Username, model.Password);
            if (user == null)
            {
                return BadRequest(new ErrorResponse("Username or password is incorrect"));
            }
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtSecret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Email, user.Email),
                }),
                Expires = DateTime.UtcNow.AddHours(18),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new AuthenticationResponse(user, tokenString));
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return Ok(_userService.GetAll());
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(Guid id)
        {
            var user = _userService.GetById(id);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(Guid id, User user, string password = null)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            try
            {
                _userService.Update(user, password);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!_userService.UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Users
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for
        // more details see https://aka.ms/RazorPagesCRUD.
        [HttpPost]
        public async Task<ActionResult<User>> PostUser(User user, string password)
        {
            try
            {
                 user = _userService.Create(user, password);

            }
            catch (ArgumentException e)
            {
                return BadRequest(new ErrorResponse(e));
            }
            catch(PlatformException e)
            {
                return BadRequest(new ErrorResponse(e));
            }
            catch (Exception e)
            {
                _logger.e(e);
                return new StatusCodeResult(StatusCodes.Status500InternalServerError);
            }

            if (user is null)
            {
                return BadRequest();
            }
                
            return CreatedAtAction("GetUser", new { id = user.Id }, user);
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<ActionResult<User>> DeleteUser(Guid id)
        {
            _userService.Delete(id);
            return NoContent();
        }

       
    }
}
