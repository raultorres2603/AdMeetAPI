using AdMeet.Attributes;
using AdMeet.Models;
using AdMeet.Services;
using Microsoft.AspNetCore.Mvc;

namespace AdMeet.Controllers;

[ApiController]
[Route("api/user")]
[Consumes("application/json")]
[Produces("application/json")]
public class UserController : ControllerBase
{
    private readonly UserServices _userServices;

    public UserController(UserServices userServices)
    {
        _userServices = userServices;
    }

    [HttpPost("login", Name = "Login")]
    [TypeFilter(typeof(Tracker))]
    public async Task<IActionResult> Login([FromBody] User user)
    {
        var token = await _userServices.LogIn(user);
        if (token == null) return NotFound($"User {user.Email} not found");
        return Ok(token);
    }
}