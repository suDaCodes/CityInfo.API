using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace CityInfo.API.Controllers;

[ApiController]
[Route("api/authentication")]
public class AuthenticationController : ControllerBase
{
    private readonly IConfiguration _configuration;

    /// <summary>
    /// We won't use this outside of this class
    /// </summary>
    public class AuthenticationRequestBody
    {
        public string? Username { get; set; }
        public string? Password { get; set; }
    }

    public class AppUser
    {
        public int UserId { get; set; }
        public string Username { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string City { get; set; }

        public AppUser(int userId, string username, string firstName, string lastName, string city)
        {
            UserId = userId;
            Username = username;
            FirstName = firstName;
            LastName = lastName;
            City = city;
        }
    }

    public AuthenticationController(IConfiguration configuration)
    {
        _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
    }
    
    /// <summary>
    /// The idea is it will return a token, which is essentially a string
    /// </summary>
    /// <returns>token</returns>
    [HttpPost("authenticate")]
    public ActionResult<string> Authenticate(AuthenticationRequestBody authRequestBody)
    {
        // STEP1: validate username/password
        var appUser = ValidateUserCredentials(authRequestBody.Username, authRequestBody.Password);

        if (appUser == null) return Unauthorized();
        
        // STEP2: create a token
        var securityKey = new SymmetricSecurityKey(
            Encoding.ASCII.GetBytes(_configuration["Authentication:SecretKey"]));

        var signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        
        // claim
        var claimForToken = new List<Claim>();
        // claim type 'sub' is a standardized key for unique user identifier
        claimForToken.Add(new Claim("sub", appUser.UserId.ToString()));
        claimForToken.Add(new Claim("given_name", appUser.FirstName));
        claimForToken.Add(new Claim("family_name", appUser.LastName));
        claimForToken.Add(new Claim("city", appUser.City));

        var jwtSecurityToken = new JwtSecurityToken(
            _configuration["Authentication:Issuer"],
            _configuration["Authentication:Audience"],
            claimForToken,
            DateTime.UtcNow,
            DateTime.UtcNow.AddHours(1),
            signingCredentials);

        var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return Ok(tokenToReturn);
    }

    private AppUser ValidateUserCredentials(string? username, string? password)
    {
        // for demo purpose, we assume the credentials are valid
        return new AppUser(1, "sule", "Su", "Le", "Ho Chi Minh City");
    }
}