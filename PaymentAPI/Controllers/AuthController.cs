using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using PaymentAPI.Configuration;
using PaymentAPI.Data;
using PaymentAPI.Models;
using PaymentAPI.RequestBodies;
using PaymentAPI.Responses;

namespace PaymentAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly ApiDbContext _db;
    private readonly ApiConfiguration _configuration;

    public AuthController(ApiDbContext db, ApiConfiguration configuration)
    {
        _db = db;
        _configuration = configuration;
    }

    [AllowAnonymous]
    [HttpGet("Register")]
    public async Task<ActionResult<User>> Register(RegisterBody body)
    {
        if (await _db.Users.FirstOrDefaultAsync(u => u.Username.Equals(body.Username)) != null)
        {
            return Conflict();
        }

        CreatePasswordHash(body.Password, out var passwordHash, out var passwordSalt);

        var user = new User
        {
            Username = body.Username,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt
        };

        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();

        return Created("/Auth/Register", user);
    }

    [Authorize]
    [HttpGet("Whoami")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status401Unauthorized)]
    public ActionResult<UserResponse> Whoami()
    {
        var user = GetCurrentUser();

        if (user is not null)
        {
            return Ok(user);
        }

        return Unauthorized();
    }
    
    [AllowAnonymous]
    [HttpPost("Login")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<string>> Login(LoginBody body) {
        var user = await _db.Users
            .FirstOrDefaultAsync(u => u.Username.Equals(body.Username));

        if (user is null)
        {
            return NotFound();
        }

        if (!VerifyPasswordHash(body.Password, user.PasswordHash, user.PasswordSalt))
        {
            return Unauthorized();
        }

        return Ok(GenerateToken(user));
    }

    private UserResponse? GetCurrentUser()
    {
        if (HttpContext.User.Identity is not ClaimsIdentity identity)
        {
            return null;
        }

        var userClaims = identity.Claims;
        var username = userClaims.First(c => c.Type == ClaimTypes.Name).Value;
        var user = _db.Users.First(u => u.Username.Equals(username));

        return new UserResponse
        {
            Username = user.Username
        };
    }

    private string GenerateToken(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimTypes.Name, user.Username),
        };
        var securityToken = _configuration.Security.Token;

        var key = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(securityToken)
        );

        var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddMinutes(30),
            signingCredentials: credentials
        );

        var jwt = new JwtSecurityTokenHandler().WriteToken(token);

        return jwt;
    }

    private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
    {
        using var hmac = new HMACSHA512(passwordSalt);
        var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
        return computedHash.SequenceEqual(passwordHash);
    }

    private void CreatePasswordHash(
        string password, out byte[] passwordHash, out byte[] passwordSalt
    )
    {
        using var hmac = new HMACSHA512();
        passwordSalt = hmac.Key;
        passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
    }
}