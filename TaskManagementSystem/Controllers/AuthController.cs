using Microsoft.AspNetCore.Identity.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using TaskManagementSystem.interfaces;
using TaskManagementSystem.Model;
using TaskManagementSystem.Utility.JWTTokenGenrator;

namespace TaskManagementSystem.Controllers;
[ApiController]
public class AuthController : Controller
{
    private readonly IEmployeeServices _employeeServices;
   private readonly GenrateJWTTokenGenrater genrateJWTTokenGenrater;
    public AuthController(IEmployeeServices employeeServices, GenrateJWTTokenGenrater genrateJWTTokenGenrater)
    {
        _employeeServices = employeeServices;
        this.genrateJWTTokenGenrater = genrateJWTTokenGenrater;
    }

    [HttpPost("api/login")]
    public async Task<IActionResult> Login(LoginModel loginModel)
    {
        if (loginModel != null)   {
            var result =  await _employeeServices.GetByEmilEmployee(loginModel.email);

            if(result != null)
            {
                 var verifyPassword = BCrypt.Net.BCrypt.Verify(loginModel.password, result.password);
                 if (verifyPassword)
                 {
                    var token = this.genrateJWTTokenGenrater.GenrateJWTToken(result.Id, result.Email, result.role);
                    return Ok(new { Token = token });   
                 }
                 return Unauthorized("Invalid password");
            }
            return NotFound("User not found");
        }
        return NotFound("User not found");
    }

   
}