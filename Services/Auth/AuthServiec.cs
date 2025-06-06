using AuthenticationApp.help;
using EcommerceApi.Models.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace AuthenticationApp.Services
{
    public class AuthServiec : IAuthServiec
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly JWT _jwt;

        public AuthServiec(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager, IOptions<JWT> jwt)
        {
            _jwt = jwt.Value;
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<AuthModel> Registeration(RegisteredModel registeredModel)
        {
            if (await _userManager.FindByEmailAsync(registeredModel.Email) is not null)
                return new AuthModel { Message = "this email is alreade active" };
            if (await _userManager.FindByNameAsync(registeredModel.UserName) is not null)
                return new AuthModel { Message = "this user name is alreade active" };
            var user = new ApplicationUser
            {
                firstName = registeredModel.FirstName,
                lastName = registeredModel.LastName,
                UserName = registeredModel.UserName,
                Email = registeredModel.Email
            };
            var result = await _userManager.CreateAsync(user, registeredModel.Password);
            if (!result.Succeeded) { 
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                return new AuthModel { Message = errors };
            }
            await _userManager.AddToRoleAsync(user, "User");
            var jwtSecurityToken = await CreateJwtToken(user);
            return new AuthModel {
                Email = user.Email,
                IsAuthenticated = true,
               // ExpiresOn = jwtSecurityToken.ValidTo,
                Message = "User created successfully",
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Roles = new List<string> { "User" },
                UserName = user.UserName
            };
        }

        public async Task<AuthModel> SignIn(SignInModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null ||
                !await _userManager.CheckPasswordAsync(user,model.Password))
                return new AuthModel {
                    Message = "this email or password is not correct"
                };
            var jwtSecurityToken = await CreateJwtToken(user);
            var roles = await _userManager.GetRolesAsync(user);
            var authModel = new AuthModel
            {
                Email = user.Email,
                IsAuthenticated = true,
                Message = "User Signed in successfully",
                Token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken),
                Roles = roles.ToList(),
                UserName = user.UserName,
                //  ExpiresOn = jwtSecurityToken.ValidTo

            };

            if (user.refrachedTokens.Any(t => t.IsActived))
            {
                var activeRefreshToken = user.refrachedTokens.FirstOrDefault(t => t.IsActived);
                authModel.RefreshToken = activeRefreshToken.Token;
                authModel.RefreshTokenExpiryTime = activeRefreshToken.ExpiredOn;
            }
            else
            {
                var refreshToken = GenerateRefrachedToken();
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpiryTime = refreshToken.ExpiredOn;
                user.refrachedTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            return authModel;

        }
        public async Task<string> AddRole(RoleModel model)
        {
            var user = await _userManager.FindByIdAsync(model.UserId);

            if (user is null || !await _roleManager.RoleExistsAsync(model.RoleName))
                return "Invalid user ID or Role";

            if (await _userManager.IsInRoleAsync(user, model.RoleName))
                return "User already assigned to this role";

            var result = await _userManager.AddToRoleAsync(user, model.RoleName);

            return result.Succeeded ? string.Empty : "Sonething went wrong";
        }


        private async Task<JwtSecurityToken> CreateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = new List<Claim>();

            foreach (var role in roles)
                roleClaims.Add(new Claim("roles", role));

            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email),
                new Claim("uid", user.Id)
            }
            .Union(userClaims)
            .Union(roleClaims);

            var symmetricSecurityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwt.key));
            var signingCredentials = new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha256);
            var jwtSecurityToken = new JwtSecurityToken(
                issuer: _jwt.Issuer,
                audience: _jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddDays(_jwt.DurationInDay),
                signingCredentials: signingCredentials);

            return jwtSecurityToken;
        }

        private RefrachedToken GenerateRefrachedToken()
        {
            var randomNumber = new byte[32];
            using var generator = RandomNumberGenerator.Create();
            generator.GetBytes(randomNumber);
            return new RefrachedToken
            {
                Token = Convert.ToBase64String(randomNumber),
                ExpiredOn = DateTime.UtcNow.AddDays(7),  // Using UtcNow instead of Now
                CreatedOn = DateTime.UtcNow              // Using UtcNow instead of Now
            };
        }
        public async Task<AuthModel> RefrachToken(string token)
        {
            var authModel = new AuthModel();

            var user = await _userManager.Users.SingleOrDefaultAsync(u => u.refrachedTokens.Any(t => t.Token == token));

            if (user == null)
            {
                authModel.Message = "Invalid token";
                return authModel;
            }

            var refreshToken = user.refrachedTokens.Single(t => t.Token == token);

            if (!refreshToken.IsActived)
            {
                authModel.Message = "Inactive token";
                return authModel;
            }

            refreshToken.RevokedOn = DateTime.UtcNow;

            var newRefreshToken = GenerateRefrachedToken();
            user.refrachedTokens.Add(newRefreshToken);
            await _userManager.UpdateAsync(user);

            var jwtToken = await CreateJwtToken(user);
            authModel.IsAuthenticated = true;
            authModel.Token = new JwtSecurityTokenHandler().WriteToken(jwtToken);
            authModel.Email = user.Email;
            authModel.UserName = user.UserName;
            var roles = await _userManager.GetRolesAsync(user);
            authModel.Roles = roles.ToList();
            authModel.RefreshToken = newRefreshToken.Token;
            authModel.RefreshTokenExpiryTime = newRefreshToken.ExpiredOn;

            return authModel;
        }
    }
}
