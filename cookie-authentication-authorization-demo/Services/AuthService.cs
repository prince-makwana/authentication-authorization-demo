using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using cookie_authentication_authorization_demo.Models.DTOs;
using cookie_authentication_authorization_demo.Models.ViewModels;

namespace cookie_authentication_authorization_demo.Services
{
    /// <summary>
    /// Service responsible for handling user authentication and authorization operations
    /// </summary>
    public class AuthService : IAuthService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuditService _auditService;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ILogger<AuthService> _logger;

        /// <summary>
        /// Constructor for AuthService
        /// </summary>
        /// <param name="userManager">UserManager for handling user operations</param>
        /// <param name="signInManager">SignInManager for handling sign-in operations</param>
        /// <param name="auditService">Service for logging audit actions</param>
        /// <param name="httpContextAccessor">IHttpContextAccessor for accessing HttpContext</param>
        /// <param name="logger">ILogger for logging messages</param>
        public AuthService(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAuditService auditService,
            IHttpContextAccessor httpContextAccessor,
            ILogger<AuthService> logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _auditService = auditService;
            _httpContextAccessor = httpContextAccessor;
            _logger = logger;
        }

        /// <summary>
        /// Registers a new user in the system
        /// </summary>
        /// <param name="model">User registration information</param>
        /// <returns>IdentityResult indicating success or failure</returns>
        public async Task<IdentityResult> RegisterAsync(RegisterModel model)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                CreatedAt = DateTime.UtcNow,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
                await _auditService.LogActionAsync(
                    "User",
                    "Register",
                    user.Id,
                    JsonSerializer.Serialize(new { user.UserName, user.Email, user.FirstName, user.LastName })
                );
            }
            return result;
        }

        /// <summary>
        /// Authenticates a user and creates a session
        /// Authenticates a user and creates an authentication cookie
        /// </summary>
        /// <param name="model">User login credentials</param>
        /// <returns>SignInResult indicating success or failure</returns>
        public async Task<SignInResult> LoginAsync(LoginModel model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                await _auditService.LogActionAsync(
                    "User",
                    "LoginAttempt",
                    null,
                    JsonSerializer.Serialize(new { Email = model.Email, Success = false, Reason = "User not found" })
                );
                return SignInResult.Failed;
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);
            
            await _auditService.LogActionAsync(
                "User",
                "LoginAttempt",
                user.Id,
                JsonSerializer.Serialize(new { 
                    Email = model.Email, 
                    Success = result.Succeeded, 
                    Reason = result.Succeeded ? "Success" : "Invalid password",
                    RememberMe = model.RememberMe
                })
            );

            return result;
        }

        /// <summary>
        /// Logs out the currently authenticated user
        /// </summary>
        public async Task LogoutAsync()
        {
            var user = await _userManager.GetUserAsync(_httpContextAccessor.HttpContext?.User);
            if (user != null)
            {
                await _auditService.LogActionAsync(
                    "User",
                    "Logout",
                    user.Id,
                    JsonSerializer.Serialize(new { user.UserName, user.Email })
                );
            }
            await _signInManager.SignOutAsync();
        }

        /// <summary>
        /// Changes the password for a user
        /// </summary>
        /// <param name="userId">ID of the user</param>
        /// <param name="model">Password change information</param>
        /// <returns>IdentityResult indicating success or failure</returns>
        public async Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            var result = await _userManager.ChangePasswordAsync(user, currentPassword, newPassword);
            if (result.Succeeded)
            {
                await _auditService.LogActionAsync(
                    "User",
                    "ChangePassword",
                    user.Id,
                    JsonSerializer.Serialize(new { user.UserName, user.Email })
                );
            }
            return result;
        }

        /// <inheritdoc/>
        public async Task<IdentityResult> UpdateProfileAsync(string userId, UpdateProfileModel model)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            user.FirstName = model.FirstName;
            user.LastName = model.LastName;
            user.PhoneNumber = model.PhoneNumber;

            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                await _auditService.LogActionAsync(
                    "User",
                    "UpdateProfile",
                    user.Id,
                    JsonSerializer.Serialize(new { user.UserName, user.Email, user.FirstName, user.LastName, user.PhoneNumber })
                );
            }
            return result;
        }

        public async Task<ApplicationUser?> GetUserByIdAsync(string userId)
        {
            return await _userManager.FindByIdAsync(userId);
        }

        public async Task<ApplicationUser?> GetUserByEmailAsync(string email)
        {
            return await _userManager.FindByEmailAsync(email);
        }

        public async Task<IList<string>> GetUserRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> IsUserInRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }

        public async Task<IdentityResult> DeleteAccountAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "User not found" });
            }

            var result = await _userManager.DeleteAsync(user);
            if (result.Succeeded)
            {
                await _auditService.LogActionAsync(
                    "User",
                    "DeleteAccount",
                    user.Id,
                    JsonSerializer.Serialize(new { user.UserName, user.Email })
                );
            }
            return result;
        }

        public async Task<IdentityResult> CreateUserAsync(ApplicationUser user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<IdentityResult> UpdateUserAsync(ApplicationUser user)
        {
            return await _userManager.UpdateAsync(user);
        }

        public async Task<IdentityResult> DeleteUserAsync(ApplicationUser user)
        {
            return await _userManager.DeleteAsync(user);
        }

        public async Task<SignInResult> SignInAsync(string email, string password, bool rememberMe)
        {
            return await _signInManager.PasswordSignInAsync(email, password, rememberMe, lockoutOnFailure: false);
        }

        public async Task<IdentityResult> ResetPasswordAsync(ApplicationUser user, string token, string newPassword)
        {
            return await _userManager.ResetPasswordAsync(user, token, newPassword);
        }

        public async Task<string> GeneratePasswordResetTokenAsync(ApplicationUser user)
        {
            return await _userManager.GeneratePasswordResetTokenAsync(user);
        }

        public async Task<bool> IsEmailConfirmedAsync(ApplicationUser user)
        {
            return await _userManager.IsEmailConfirmedAsync(user);
        }

        public async Task<IdentityResult> ConfirmEmailAsync(ApplicationUser user, string token)
        {
            return await _userManager.ConfirmEmailAsync(user, token);
        }

        public async Task<string> GenerateEmailConfirmationTokenAsync(ApplicationUser user)
        {
            return await _userManager.GenerateEmailConfirmationTokenAsync(user);
        }

        public async Task<IdentityResult> AddToRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.AddToRoleAsync(user, role);
        }

        public async Task<IdentityResult> RemoveFromRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.RemoveFromRoleAsync(user, role);
        }

        public async Task<IList<string>> GetRolesAsync(ApplicationUser user)
        {
            return await _userManager.GetRolesAsync(user);
        }

        public async Task<bool> IsInRoleAsync(ApplicationUser user, string role)
        {
            return await _userManager.IsInRoleAsync(user, role);
        }
    }
} 