using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Complaint_Report_Registering_API.Contracts;
using Complaint_Report_Registering_API.Data;
using Complaint_Report_Registering_API.DTOs;
using Complaint_Report_Registering_API.Entities;
using Complaint_Report_Registering_API.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApiWithPostgreSQL.DTOs;
using static Complaint_Report_Registering_API.DTOs.ServiceResponses;

namespace Complaint_Report_Registering_API.Repositories
{
    public class AccountRepository(UserManager<ApplicationUser> userManager,
                                    RoleManager<IdentityRole> roleManager,
                                    SignInManager<ApplicationUser> signInManager,
                                    IConfiguration config,
                                    IMailService mailService,
                                    AppDbContext context) : IAccount
    {
        public Task<ObjectResponse> GetUserData(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);

            var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier);
            if (roleClaim != null && userIdClaim != null)
            {
                var data = new { Role = roleClaim.Value, UserId = userIdClaim.Value };
                return Task.FromResult(new ObjectResponse(true, data));
            }
            else
            {
                return Task.FromResult(new ObjectResponse(false, "Role or UserId not found in token"));
            }
        }

        public async Task<ObjectResponse> GetUserProfile(string userId)
        {
            var user = await context.Users.OfType<ApplicationUser>()
            .Include(u => u.Complaints)
            .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return new ObjectResponse(false, "Profile not found");
            }
            return new ObjectResponse(true, user);
        }

        public async Task<Test> ConvertToken(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var claims = jwtToken.Claims.ToDictionary(c => c.Type, c => c.Value);
            return new Test(true, claims);
        }

        public async Task<GeneralResponse> CreateAccount(UserDTO userDTO)
        {
            if (userDTO is null) return new GeneralResponse(false, "Model is empty");
            var newUser = new ApplicationUser()
            {
                UserName = userDTO.FirstName + userDTO.LastName,
                FirstName = userDTO.FirstName,
                LastName = userDTO.LastName,
                Email = userDTO.Email,
                PasswordHash = userDTO.Password,
            };
            var user = await userManager.FindByEmailAsync(newUser.Email);
            if (user is not null) return new GeneralResponse(false, "User registered already");

            var createUser = await userManager.CreateAsync(newUser!, userDTO.Password);
            if (!createUser.Succeeded)
            {
                foreach (var error in createUser.Errors)
                {
                    Console.Error.WriteLine(error.Description);
                }
                return new GeneralResponse(false, "Error occured.. please try again");
            }

            //Assign Default Role : Admin to first registrar; rest is user
            var checkAdmin = await roleManager.FindByNameAsync("Admin");
            if (checkAdmin is null)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                await userManager.AddToRoleAsync(newUser, "Admin");
                return new GeneralResponse(true, "Account Created");
            }
            else
            {
                var checkUser = await roleManager.FindByNameAsync("User");
                if (checkUser is null)
                    await roleManager.CreateAsync(new IdentityRole() { Name = "User" });

                await userManager.AddToRoleAsync(newUser, "User");
                return new GeneralResponse(true, "Account Created");
            }
        }

        public async Task<GeneralResponse> CreateResetToken(MailData mailData)
        {
            var user = await userManager.FindByEmailAsync(mailData.EmailToId);
            if (user == null)
                return new GeneralResponse(false, "User not found");

            var token = await userManager.GeneratePasswordResetTokenAsync(user);

            mailData.EmailBody = $"Your password reset token is: {token}";
            var response = await mailService.SendMailAsync(mailData);
            if (!response)
            {
                return new GeneralResponse(false, "Failed to generate reset token");
            }
            return new GeneralResponse(true, "Reset token generated");
        }

        public Task<bool> IsUserLoggedIn(string token)
        {
            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadToken(token) as JwtSecurityToken;

            if (jwtToken == null)
                return Task.FromResult(false);

            return Task.FromResult(jwtToken.ValidTo > DateTime.UtcNow);
        }

        public async Task<LoginResponse> LoginAccount(LoginDTO loginDTO)
        {
            if (loginDTO == null)
                return new LoginResponse(false, null!, null!, null!, "Login container is empty");

            var getUser = await userManager.FindByEmailAsync(loginDTO.Email);
            if (getUser is null)
                return new LoginResponse(false, null!, null!, null!, "User not found");
            // bool checkUserPasswords = await userManager.CheckPasswordAsync(getUser, loginDTO.Password);
            // if (!checkUserPasswords)
            //     return new LoginResponse(false, null!, "Invalid email/password");
            var result = await signInManager.PasswordSignInAsync(getUser, loginDTO.Password, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
                return new LoginResponse(false, null!, null!, null!, "Invalid email/password");

            var getUserRole = await userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(getUser.Id, getUser.FirstName, getUser.LastName, getUser.Email, getUserRole.First());
            string jwtToken = GenerateToken(userSession);
            return new LoginResponse(true, jwtToken!, getUser.Id, getUserRole.First(), "Login completed");
        }

        public async Task<GeneralResponse> LogoutAccount()
        {
            await signInManager.SignOutAsync();
            return new GeneralResponse(true, "Logout successful");
        }

        public async Task<GeneralResponse> ResetPassword(string resetToken, string email, string newPassword)
        {
            var getUser = await userManager.FindByEmailAsync(email);
            if (getUser is null)
                return new GeneralResponse(false, "User not found");
            var result = await userManager.ResetPasswordAsync(getUser, resetToken, newPassword);
            if (!result.Succeeded)
                return new GeneralResponse(false, "Password reset failed");
            return new GeneralResponse(true, "Email Successfully send!");
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Name, user.FirstName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role)
            };
            var token = new JwtSecurityToken(
                issuer: config["Jwt:Issuer"],
                audience: config["Jwt:Audience"],
                claims: userClaims,
                expires: DateTime.Now.AddDays(1),
                signingCredentials: credentials
                );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}