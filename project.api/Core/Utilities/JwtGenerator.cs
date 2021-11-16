using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
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

namespace project.api.Core.Utilities
{
    public class CustomClaims
    {
        public const string UserId = "userId";
        public const string ClientId = "clientId";


    }

    public interface ITokenManager
    {
        public RefreshToken GetValidRefreshToken(string token, User identityUser);
        public Task<User> ValidateUser(SignInUser credentials);
        public Task<string> GenerateAccessToken(User identityUser);
        public RefreshToken GenerateRefreshToken(string ipAddress, string userId);
        public bool IsRefreshTokenValid(RefreshToken existingToken);
    }
    public class JwtGenerator : ITokenManager
    {
        private readonly UserManager<User> _userManager;
        private readonly SymmetricSecurityKey _authSigningKey;
        private readonly string _issuer;
        private readonly string _audience;
        private readonly IConfiguration _configuration;
        public JwtGenerator(IConfiguration configuration,UserManager<User> userManager)
        {
            _userManager = userManager;
            _authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:JWT_SECRET"]));
            _issuer = configuration["JWT:VALIDISSUER"];
            _audience = configuration["JWT:VALIDAUDIENCE"];
            _configuration = configuration;
        }

        public RefreshToken GetValidRefreshToken(string token, User identityUser)
        {
            if (identityUser == null)
            {
                return null;
            }

            var existingToken = identityUser.RefreshTokens.FirstOrDefault(x => x.Token == token);
            return IsRefreshTokenValid(existingToken) ? existingToken : null;
        }
        public async Task<User> ValidateUser(SignInUser credentials)
        {
            var identityUser = await _userManager.FindByNameAsync(credentials.Username);
            if (identityUser != null)
            {
                var result = _userManager.PasswordHasher.VerifyHashedPassword(identityUser, identityUser.PasswordHash, credentials.Password);
                return result == PasswordVerificationResult.Failed ? null : identityUser;
            }

            return null;
        }
       
        public async Task<string> GenerateAccessToken(User identityUser)
        {
            var userRoles = await _userManager.GetRolesAsync(identityUser);
            var roleClaims = new List<Claim> { };
            roleClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_configuration["JWT:JWT_SECRET"]);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
              new Claim(ClaimTypes.Name, identityUser.UserName.ToString()),
              new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
              new Claim(ClaimTypes.Email, identityUser.Email),
              new Claim(CustomClaims.UserId,identityUser.Id.ToString()),


                })
                ,

                Expires = DateTime.Now.AddSeconds(86300),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                Audience = _configuration["JWT:VALIDAUDIENCE"],
                Issuer = _configuration["JWT:VALIDISSUER"],
            };
            foreach (var claim in roleClaims)
            {
                tokenDescriptor.Subject.AddClaim(claim);
            }
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
        public RefreshToken GenerateRefreshToken(string ipAddress, string userId)
        {
            using (var rngCryptoServiceProvider = new RNGCryptoServiceProvider())
            {
                var randomBytes = new byte[64];
                rngCryptoServiceProvider.GetBytes(randomBytes);
                return new RefreshToken
                {
                    Token = Convert.ToBase64String(randomBytes),
                    ExpiryOn = DateTime.UtcNow.AddDays(1),
                    CreatedOn = DateTime.UtcNow,
                    CreatedByIp = ipAddress,
                    UserId = userId
                };
            }

        }
        public bool IsRefreshTokenValid(RefreshToken existingToken)
        {
            // Is token already revoked, then return false
            if (existingToken.RevokedByIp != null && existingToken.RevokedOn != DateTime.MinValue)
            {
                return false;
            }

            // Token already expired, then return false
            if (existingToken.ExpiryOn <= DateTime.UtcNow)
            {
                return false;
            }

            return true;
        }

        //public async Task<string> CreateAccessToken(User user)
        //{
        //    var userRoles = await _userManager.GetRolesAsync(user);
        //    var authClaims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name,user.UserName),
        //        new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString()),
        //        new Claim(CustomClaims.UserId,user.Id.ToString()),
        //    };
        //    authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

        //    var accessSecurityToken = new JwtSecurityToken
        //    (
        //        issuer: _issuer,
        //        audience: _audience,
        //        expires: DateTime.UtcNow.AddHours(1),
        //        claims: authClaims,
        //        signingCredentials: new SigningCredentials(_authSigningKey,SecurityAlgorithms.HmacSha256)
        //    );
        //    return new JwtSecurityTokenHandler().WriteToken(accessSecurityToken);
        //}


    }
}
