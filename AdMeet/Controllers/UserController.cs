using AdMeet.Attributes;
using AdMeet.Inter;
using AdMeet.Models;
using Microsoft.AspNetCore.Mvc;

namespace AdMeet.Controllers;

[ApiController]
[Route("api/user")]
[Consumes("application/json")]
[Produces("application/json")]
[TypeFilter(typeof(Tracker))]
public class UserController(IUserServices userServices, ILogger<UserController> logger) : ControllerBase
{
    [HttpPost("login/", Name = "Login")]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        var token = await userServices.LogIn(user);
        if (token == null) return NotFound($"User {user.Email} not found");
        return Ok(token);
    }

    [HttpGet("{jwt}/all", Name = "GetUsers")]
    [JwtAuth]
    public async Task<IActionResult> GetUsers([FromRoute] string jwt)
    {
        logger.LogInformation($"Retrieving users: {jwt}");
        var users = await userServices.GetUsers();
        logger.LogInformation($"Done retrieving users: {jwt}");
        return Ok(users);
    }

    [HttpPost("register", Name = "Register")]
    public async Task<IActionResult> Register([FromBody] User user)
    {
        var result = await userServices.Register(user);
        if (result != "UAE") return Ok(result);
        return BadRequest("User already exists");
    }

    [HttpGet("{jwt}/get", Name = "GetInfo")]
    [JwtAuth]
    public async Task<IActionResult> GetInfo([FromRoute] string jwt)
    {
        try
        {
            var (user, newTok) = await userServices.GetInfo(jwt);
            return Ok(new
            {
                user,
                newTok
            });
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    // Create an enpoint, put endpoint, to update profile
    [HttpPut("{jwt}/update", Name = "UpdateProfile")]
    [JwtAuth]
    public async Task<IActionResult> UpdateProfile([FromRoute] string jwt, [FromBody] User u)
    {
        try
        {
            var result = await userServices.UpdateProf(u);
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}