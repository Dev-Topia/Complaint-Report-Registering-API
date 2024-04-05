using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Complaint_Report_Registering_API.Contracts;
using Complaint_Report_Registering_API.Data;
using Complaint_Report_Registering_API.DTOs.GetDTOs;
using Complaint_Report_Registering_API.DTOs.PostDTOs;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using WebApiWithPostgreSQL.DTOs;
using static Complaint_Report_Registering_API.DTOs.ServiceResponses;

namespace Complaint_Report_Registering_API.Repositories
{
    public class UserRepository(UserManager<ApplicationUser> userManager,
                                    RoleManager<IdentityRole> roleManager,
                                    SignInManager<ApplicationUser> signInManager,
                                    IConfiguration config,
                                    AppDbContext context) : IUser
    {
        public async Task<bool> FindUserByEmail(string email)
        {
            var userExist = await userManager.FindByEmailAsync(email);
            return userExist != null;
        }

        public async Task<bool> FindUserById(string userId)
        {
            var userExist = await context.ApplicationUsers.AnyAsync(u => u.Id == userId);
            return userExist;
        }

        public async Task<bool> SignUp(SignUpDTO user)
        {
            var newUser = new ApplicationUser()
            {
                UserName = user.FirstName + user.Lastname,
                FirstName = user.FirstName,
                LastName = user.Lastname,
                Email = user.Email,
                PasswordHash = user.Password,
            };
            var createUser = await userManager.CreateAsync(newUser!, user.Password);
            if (!createUser.Succeeded)
            {
                var errorDescriptions = createUser.Errors.Select(error => error.Description);
                var errorMessage = string.Join(" ", errorDescriptions);
                Console.Error.WriteLine(errorMessage);
                return false;
            }
            var checkAdmin = await roleManager.FindByNameAsync("Admin");
            if (checkAdmin is null)
            {
                await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
                await userManager.AddToRoleAsync(newUser, "Admin");
                return true;
            }
            else
            {
                var checkUser = await roleManager.FindByNameAsync("User");
                if (checkUser is null)
                    await roleManager.CreateAsync(new IdentityRole() { Name = "User" });

                await userManager.AddToRoleAsync(newUser, "User");
                return true;
            }
        }

        public async Task<LoginResponse> SignIn(SignInDTO user)
        {
            var getUser = await userManager.FindByEmailAsync(user.Email!);
            if (getUser is null)
                return new LoginResponse(null!, null!, null!, "User not found");
            var result = await signInManager.PasswordSignInAsync(getUser, user.Password!, isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
                return new LoginResponse(null!, null!, null!, "Invalid password");

            var getUserRole = await userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(getUser.Id, getUser.FirstName, getUser.LastName, getUser.Email, getUserRole.First());
            string jwtToken = GenerateToken(userSession);
            return new LoginResponse(jwtToken!, getUser.Id, getUserRole.First(), "Login completed");
        }

        public async Task<bool> SignOut()
        {
            await signInManager.SignOutAsync();
            return true;
        }

        public async Task<UserGetDTO> ViewUser(string userId)
        {
            var user = await context.ApplicationUsers
                .Include(u => u.Complaints!)
                .ThenInclude(c => c.ComplaintType!)
                .Include(u => u.Complaints!)
                .ThenInclude(c => c.Status)
                .FirstOrDefaultAsync(u => u.Id == userId);
            var userToDisplay = new UserGetDTO
            {
                UserId = user?.Id,
                FirstName = user?.FirstName,
                LastName = user?.LastName,
                Email = user?.Email,
                Complaints = user?.Complaints!.Select(c => new ComplaintGetUserDTO
                {
                    ComplaintId = c.ComplaintId,
                    Title = c.Title,
                    ComplaintType = c.ComplaintType?.Type,
                    Status = c.Status?.Type,
                    Description = c.Description,
                    FileUrl = c.FileUrl,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                }).ToList()
            };
            return userToDisplay;
        }

        public async Task<List<UserGetDTO>> ViewUsers()
        {
            var users = await context.ApplicationUsers
                .Include(u => u.Complaints!)
                .ThenInclude(c => c.ComplaintType)
                .Include(u => u.Complaints!)
                .ThenInclude(c => c.Status)
                .ToListAsync();
            var usersToDisplay = users.Select(u => new UserGetDTO
            {
                UserId = u.Id,
                FirstName = u.FirstName,
                LastName = u.LastName,
                Email = u.Email,
                Complaints = u.Complaints!.Select(c => new ComplaintGetUserDTO
                {
                    ComplaintId = c.ComplaintId,
                    Title = c.Title,
                    ComplaintType = c.ComplaintType?.Type,
                    Status = c.Status?.Type,
                    Description = c.Description,
                    FileUrl = c.FileUrl,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt,
                }).ToList()
            }).ToList();
            return usersToDisplay;
        }

        private string GenerateToken(UserSession user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]!));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var userClaims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id!),
                new Claim(ClaimTypes.Name, user.FirstName!),
                new Claim(ClaimTypes.Email, user.Email!),
                new Claim(ClaimTypes.Role, user.Role!)
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