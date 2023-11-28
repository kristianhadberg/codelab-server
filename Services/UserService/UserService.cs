using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using codelab_exam_server.Data;
using codelab_exam_server.Dtos.User;
using codelab_exam_server.Helpers;
using codelab_exam_server.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace codelab_exam_server.Services.UserService;

public class UserService : IUserService
{
    private readonly DatabaseContext _dbContext;
    private readonly AppSettings _appSettings;

    public UserService(DatabaseContext dbContext, IOptions<AppSettings> appSettings)
    {
        _dbContext = dbContext;
        _appSettings = appSettings.Value;
    }
    
    public async Task<UserResponse> Register(UserRequest userRequest)
    {
        CreatePasswordHash(userRequest.Password, out byte[] passwordHash, out byte[] passwordSalt);

        var user = new User()
        {
            Username = userRequest.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        _dbContext.Add(user);
        await _dbContext.SaveChangesAsync();

        return UserToResponse(user);
    }

    public async Task<UserResponse> Login(LoginRequest loginRequest)
    {
        var user = await _dbContext.Users.FirstOrDefaultAsync(u => u.Username.Equals(loginRequest.Username));
        if (user == null)
        {
            throw new Exception("User not found");
        }

        if (!VerifyPasswordHash(loginRequest.Password, user.PasswordHash, user.PasswordSalt))
        {
            throw new Exception("Bad password");
        }
        
        string token = CreateToken(user);
        
        var userResponse = new UserResponse()
        {
            Id = user.Id,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName,
            Token = token
        };
        return userResponse;
    }

    private string CreateToken(User user)
    {
        List<Claim> claims = new List<Claim>()
        {
            new Claim(ClaimTypes.Name, user.Username)
        };
        var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(
            _appSettings.Secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddDays(1),
            signingCredentials: creds);

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);
        return jwt;
    }
    
    private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512())
        {
            passwordSalt = hmac.Key;
            passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
        }
    }
    
    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using (var hmac = new HMACSHA512(passwordSalt))
        {
            var computedHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            return computedHash.SequenceEqual(passwordHash);
        }
    }
    private static UserResponse UserToResponse(User user) =>
        new UserResponse()
        {
            Id = user.Id,
            Username = user.Username,
            FirstName = user.FirstName,
            LastName = user.LastName
        };
    
    private static User ToEntity(UserResponse userResponse) =>
        new User()
        {
            Username = userResponse.Username
        };
}