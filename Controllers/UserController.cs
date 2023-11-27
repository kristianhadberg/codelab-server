
using codelab_exam_server.Dtos.User;
using codelab_exam_server.Services.UserService;
using Microsoft.AspNetCore.Mvc;

namespace codelab_exam_server.Controllers;

public class UserController
{
    private readonly IUserService _userService;

    public UserController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("register")]
    public async Task<UserResponse> Register(UserRequest userRequest)
    {
        var userResponse = await _userService.Register(userRequest);
        return userResponse;
    }
    
    [HttpPost("login")]
    public async Task<UserResponse> Login(UserRequest userRequest)
    {
        var userResponse = await _userService.Login(userRequest);
        return userResponse;
    }
}