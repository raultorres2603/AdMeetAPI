using AdMeet.Attributes;
using AdMeet.Models;
using AdMeet.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdMeet.Controllers;

[ApiController]
[Route("api/user")]
[Consumes("application/json")]
[Produces("application/json")]
[TypeFilter(typeof(Tracker))]
public class UserController(UserServices userServices, ILogger logger) : ControllerBase
{
    [HttpPost("login", Name = "Login")]
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
        List<User> users = await userServices.GetUsers();
        logger.LogInformation($"Done retrieving users: {jwt}");
        return Ok(users);
    }
}