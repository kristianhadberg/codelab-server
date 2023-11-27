using codelab_exam_server.Dtos.User;

namespace codelab_exam_server.Services.UserService;

public interface IUserService
{
    Task<UserResponse> Register(UserRequest userRequest);
    Task<UserResponse> Login(UserRequest userRequest);

}