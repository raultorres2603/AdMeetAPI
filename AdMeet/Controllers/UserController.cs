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
public class UserController : ControllerBase
{
    private readonly UserServices _userServices;
    private readonly ILogger _logger;

    public UserController(UserServices userServices, ILogger logger)
    {
        _userServices = userServices;
        _logger = logger;
    }

    [HttpPost("login", Name = "Login")]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        var token = await _userServices.LogIn(user);
        if (token == null) return NotFound($"User {user.Email} not found");
        return Ok(token);
    }

    [HttpGet("{jwt}/all", Name = "GetUsers")]
    [JwtAuth]
    public async Task<IActionResult> GetUsers([FromRoute] string jwt)
    {
        _logger.LogInformation($"Retrieving users: {jwt}");
        List<User> users = await _userServices.GetUsers();
        _logger.LogInformation($"Done retrieving users: {jwt}");
        return Ok(users);
    }
}