using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using project.api.Core.CustomExceptions;
using project.api.Core.Interfaces;
using project.api.Core.Utilities;
using project.api.Data.Context;
using project.api.Data.DTO.Users;
using project.api.Data.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace project.api.Controllers
{
    [ApiController]
    
    
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        //private readonly IUserServices _userServices;
        private readonly IMapper _mapper;
        private readonly UserManager<User> _userManager;
        private readonly ITokenManager _tokenManager;
        private readonly AppDBContext _context;
        private readonly IConfiguration _configuration;
        public UserController(IConfiguration configuration, UserManager<User> userManager, AppDBContext context, /*IUserServices userServices,*/ IMapper mapper,ITokenManager tokenManager)
        {
            _userManager = userManager;
           // _userServices = userServices;
            _mapper = mapper;
            _context = context;
            _tokenManager = tokenManager;
            _configuration = configuration;
        }
        
        [Authorize]
        [AllowAnonymous]
        [HttpPost("signup")]
        public async Task<IActionResult> SignUp(SignUpUser signUpUser)
        {
         
                var user = await _userManager.FindByNameAsync(signUpUser.Username);
                //var result = _userServices.SignUp(user);
                if(user != null)
                {
                    return BadRequest("User already exists");
                }

                var newUser = new User
                {
                    Email = signUpUser.Email,
                    UserName = signUpUser.Username
                };
                var createdUserResult = await _userManager.CreateAsync(newUser, signUpUser.Password);
                if (!createdUserResult.Succeeded)
                    return BadRequest("Could not create an user");

                await _userManager.AddToRoleAsync(newUser, Roles.SimpleUser);

                return CreatedAtAction(nameof(SignUp), _mapper.Map<UserDTO>(newUser));
           
        }
        [Authorize]
        [AllowAnonymous]
        [HttpPost("signin")]
        public async Task<IActionResult> SignIn(SignInUser signInUser)
        {
            var user = await _userManager.FindByNameAsync(signInUser.Username);
            if(user == null)
            {
                return BadRequest("Username or password is invalid!");
            }
            var isPasswordValid = await _userManager.CheckPasswordAsync(user, signInUser.Password);
            if (!isPasswordValid)
                return BadRequest("User name or password is invalid!");
            var accessToken = await _tokenManager.GenerateAccessToken(user);
            AuthenticatedUser auser = new AuthenticatedUser();
            auser.Token =  accessToken;
            auser.Username = signInUser.Username;
            var token = await GenerateTokens(user);
            return Ok(token);
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("RefreshToken")]
        public async Task<IActionResult> RefreshToken()
        {
            var token = HttpContext.Request.Cookies["refreshToken"];
            var identityUser = _context.Users.Include(x => x.RefreshTokens)
                .FirstOrDefault(x => x.RefreshTokens.Any(y => y.Token == token && y.UserId == x.Id));

            // Get existing refresh token if it is valid and revoke it
            var existingRefreshToken = _tokenManager.GetValidRefreshToken(token, identityUser);
            if (existingRefreshToken == null)
            {
                return new BadRequestObjectResult(new { Message = "Failed" });
            }

            existingRefreshToken.RevokedByIp = HttpContext.Connection.RemoteIpAddress.ToString();
            existingRefreshToken.RevokedOn = DateTime.UtcNow;

            // Generate new tokens
            var newToken = await GenerateTokens(identityUser); 
            return Ok(new { Token = newToken, Message = "Success" });
        }
        [AllowAnonymous]
        [HttpPost]
        [Route("RevokeToken")]
        public IActionResult RevokeToken(string token)
        {
            // If user found, then revoke
            if (RevokeRefreshToken(token))
            {
                return Ok(new { Message = "Success" });
            }

            // Otherwise, return error
            return new BadRequestObjectResult(new { Message = "Failed" });
        }
        [Authorize]
        [HttpPost]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            // Revoke Refresh Token 
            RevokeRefreshToken();
            return Ok(new { Token = "", Message = "Logged Out" });
        }
        //[HttpPost]
        //[Route("reset-password")]
        //public async Task<IActionResult> ResetPassword(PasswordDTO model)
        //{
        //    if (!ModelState.IsValid)
        //        return BadRequest(ModelState);
        //    try
        //    {
        //        if (model is null)
        //            return BadRequest();


        //        var user = await _userManager.FindByNameAsync(User.Identity.Name);
        //        if (user == null)
        //            return BadRequest("No user found!");



        //        string resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
        //        if (string.IsNullOrEmpty(resetToken))
        //            return BadRequest("Error while generating reset token.");

        //        var result = await _userManager.ResetPasswordAsync(user, resetToken, model.NewPassword);

        //        if (result.Succeeded)
        //            return Ok();
        //        else
        //            return BadRequest();
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(ex);
        //    }
        //}
        [Authorize(Roles = Roles.Admin)]
        [HttpPost]
        [Route("{userId}/make-employer")]
        public async Task<IActionResult> MakeEmployer(string userId)
        {
            var user =  await _userManager.FindByIdAsync(userId);
            if(user == null)
            {
                return NotFound("User not found");
            }
            await _userManager.AddToRoleAsync(user, Roles.Employer);
            await _userManager.RemoveFromRoleAsync(user, Roles.SimpleUser);
            var identityUser = _context.Users.Include(x => x.RefreshTokens).FirstOrDefault(x => x.RefreshTokens.Any(y =>  y.UserId == user.Id)); ;
            var r = _context.RefreshTokens.Where(X => X.UserId == userId).ToList();
            foreach(var token in r) 
            {
                RevokeRefreshToken(token.Token);
            }


            return Ok(user);
        }
    
        public bool RevokeRefreshToken(string token = null)
        {
            token = token == null ? HttpContext.Request.Cookies["refreshToken"] : token;
            var identityUser = _context.Users.Include(x => x.RefreshTokens)
                .FirstOrDefault(x => x.RefreshTokens.Any(y => y.Token == token && y.UserId == x.Id));
            if (identityUser == null)
            {
                return false;
            }

            // Revoke Refresh token
            var existingToken = identityUser.RefreshTokens.FirstOrDefault(x => x.Token == token);
     
            existingToken.RevokedByIp = HttpContext.Connection.RemoteIpAddress.ToString();
            existingToken.RevokedOn = DateTime.UtcNow;
            _context.Update(identityUser);
            _context.SaveChanges();
            return true;
        }
        public async Task<string> GenerateTokens(User identityUser)
        {
            // Generate access token
            string accessToken = await _tokenManager.GenerateAccessToken(identityUser);

            // Generate refresh token and set it to cookie
            var ipAddress = HttpContext.Connection.RemoteIpAddress.ToString();
            var refreshToken = _tokenManager.GenerateRefreshToken(ipAddress, identityUser.Id);

            // Set Refresh Token Cookie
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTime.UtcNow.AddDays(7)
            };
            HttpContext.Response.Cookies.Append("refreshToken", refreshToken.Token, cookieOptions);

            // Save refresh token to database
            if (identityUser.RefreshTokens == null)
            {
                identityUser.RefreshTokens = new List<RefreshToken>();
            }

            identityUser.RefreshTokens.Add(refreshToken);
            _context.Update(identityUser);
            _context.SaveChanges();
            return accessToken;
        }

        //    [HttpGet]
        //    public IActionResult GetUsers()
        //    {
        //        return Ok(_userServices.GetUsers()); 
        //    }

        //    [HttpGet("{id}", Name = "GetUser")]
        //    public IActionResult GetUser(int id)
        //    {
        //        return Ok(_userServices.GetUser(id));
        //    }
    }
}
