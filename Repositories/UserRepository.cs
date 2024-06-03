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
    public class UserRepository(
        UserManager<ApplicationUser> userManager,
        // RoleManager<IdentityRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration config,
        AppDbContext context
    ) : IUser
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

        public async Task<SignUpResponse> SignUp(SignUpDTO user)
        {
            var newUser = new ApplicationUser()
            {
                UserName = user.FirstName + user.Lastname,
                FirstName = user.FirstName,
                LastName = user.Lastname,
                Email = user.Email,
                DepartmentId = user.DepartmentId,
                PasswordHash = user.Password,
            };

            var createUser = await userManager.CreateAsync(newUser!, user.Password);
            if (!createUser.Succeeded)
            {
                var errorDescriptions = createUser.Errors.Select(error => error.Description);
                var errorMessage = string.Join(" ", errorDescriptions);
                Console.Error.WriteLine(errorMessage);
                return new SignUpResponse(false, errorMessage);
            }
            // var checkAdmin = await roleManager.FindByNameAsync("Admin");
            // if (checkAdmin is null)
            // {
            //     await roleManager.CreateAsync(new IdentityRole() { Name = "Admin" });
            //     await userManager.AddToRoleAsync(newUser, "Admin");
            //     return new SignUpResponse(true, "Account successfully created");
            // }
            // else
            // {
            // var checkUser = await roleManager.FindByNameAsync("User");
            // if (checkUser is null)
            //     await roleManager.CreateAsync(new IdentityRole() { Name = "User" });

            if (user.Role == "User")
            {
                await userManager.AddToRoleAsync(newUser, "User");
                return new SignUpResponse(true, "Account successfully created");
            }
            else
            {
                await userManager.AddToRoleAsync(newUser, "Admin");
                return new SignUpResponse(true, "Account successfully created");
            }
            // }
        }

        public async Task<SignInResponse> SignIn(SignInDTO user)
        {
            var getUser = await userManager.FindByEmailAsync(user.Email!);
            if (getUser is null)
                return new SignInResponse(null!, null!, null!, "User not found");
            var result = await signInManager.PasswordSignInAsync(
                getUser,
                user.Password!,
                isPersistent: false,
                lockoutOnFailure: false
            );
            if (!result.Succeeded)
                return new SignInResponse(null!, null!, null!, "Invalid password");

            var getUserRole = await userManager.GetRolesAsync(getUser);
            var userSession = new UserSession(
                getUser.Id,
                getUser.FirstName,
                getUser.LastName,
                getUser.Email,
                getUserRole.First()
            );
            string jwtToken = GenerateToken(userSession);
            return new SignInResponse(
                jwtToken!,
                getUser.Id,
                getUserRole.First(),
                "Login completed"
            );
        }

        public async Task<bool> SignOut()
        {
            await signInManager.SignOutAsync();
            return true;
        }

        public async Task<UserGetDTO> ViewUser(string userId)
        {
            var user = await context
                .ApplicationUsers.Include(u => u.Complaints!)
                .ThenInclude(c => c.ComplaintType!)
                .Include(u => u.Complaints!)
                .ThenInclude(c => c.Status)
                .FirstOrDefaultAsync(u => u.Id == userId);
            var role = await userManager.GetRolesAsync(user!);
            var userToDisplay = new UserGetDTO
            {
                UserId = user?.Id,
                FirstName = user?.FirstName,
                LastName = user?.LastName,
                Email = user?.Email,
                PhoneNumber = user?.PhoneNumber,
                ImageUrl = user?.ImageUrl,
                Role = [.. role],
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
                })
                    .ToList()
            };
            return userToDisplay;
        }

        public async Task<List<UserGetDTO>> ViewUsers()
        {
            var users = await context
                .ApplicationUsers.Include(u => u.Complaints!)
                .ThenInclude(c => c.ComplaintType)
                .Include(u => u.Complaints!)
                .ThenInclude(c => c.Status)
                .Include(d => d.Department)
                .ToListAsync();

            var usersToDisplay = new List<UserGetDTO>();

            foreach (var user in users)
            {
                var role = await userManager.GetRolesAsync(user);
                var userDto = new UserGetDTO
                {
                    UserId = user.Id,
                    FirstName = user.FirstName,
                    LastName = user.LastName,
                    Email = user.Email,
                    PhoneNumber = user.PhoneNumber,
                    ImageUrl = user.ImageUrl,
                    Deparment = user.Department!.DepartmentName,
                    Role = [.. role],
                    Complaints = user.Complaints!.Select(c => new ComplaintGetUserDTO
                    {
                        ComplaintId = c.ComplaintId,
                        Title = c.Title,
                        ComplaintType = c.ComplaintType!.Type,
                        Status = c.Status!.Type,
                        Description = c.Description,
                        FileUrl = c.FileUrl,
                        CreatedAt = c.CreatedAt,
                        UpdatedAt = c.UpdatedAt,
                    })
                        .ToList()
                };
                usersToDisplay.Add(userDto);
            }

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

        public UserDataFromJWT DecodeJwt(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);
                var roleClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role);
                var userIdClaim = jwtToken.Claims.FirstOrDefault(c =>
                    c.Type == ClaimTypes.NameIdentifier
                );
                if (roleClaim != null && userIdClaim != null)
                {
                    var userData = new UserDataFromJWT
                    {
                        UserId = userIdClaim.Value,
                        Role = roleClaim.Value,
                        Token = token
                    };
                    return userData;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }

            return new UserDataFromJWT { };
        }

        public async Task<bool> EditUser(string userId, UserUpdateDTO userUpdate)
        {
            try
            {
                var userExist = await userManager.FindByIdAsync(userId);
                userExist!.FirstName = userUpdate.FirstName;
                userExist.LastName = userUpdate.LastName;
                userExist.ImageUrl = userUpdate.ImageUrl;
                userExist.PhoneNumber = userUpdate.PhoneNumber;
                context.ApplicationUsers.Update(userExist);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }

        public async Task<bool> RemoveUser(string userId)
        {
            try
            {
                var findUser = await context.ApplicationUsers.FindAsync(userId);
                if (findUser == null)
                {
                    return false;
                }

                context.ApplicationUsers.Remove(findUser);
                await context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                return false;
            }
        }
    }
}
