using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using cookie_authentication_authorization_demo.Data;
using cookie_authentication_authorization_demo.Models;
using cookie_authentication_authorization_demo.Services;
using System.Security.Claims;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;

namespace cookie_authentication_authorization_demo.Controllers
{
    /// <summary>
    /// Controller responsible for handling user authentication operations
    /// </summary>
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IAuditService _auditService;

        /// <summary>
        /// Constructor for AuthController
        /// </summary>
        /// <param name="userManager">User manager for handling user operations</param>
        /// <param name="signInManager">Sign-in manager for handling sign-in operations</param>
        /// <param name="auditService">Service for logging audit actions</param>
        public AuthController(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IAuditService auditService)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _auditService = auditService;
        }

        /// <summary>
        /// Registers a new user in the system
        /// </summary>
        /// <param name="model">User registration information</param>
        /// <returns>ActionResult indicating success or failure</returns>
        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterModel model)
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
                return Ok(new { message = "Registration successful" });
            }
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        /// <summary>
        /// Authenticates a user and creates a session
        /// </summary>
        /// <param name="model">User login credentials</param>
        /// <returns>ActionResult with authentication result</returns>
        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginModel model)
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
                return BadRequest(new { message = "Invalid email or password" });
            }

            var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, lockoutOnFailure: true);
            if (result.Succeeded)
            {
                await _auditService.LogActionAsync(
                    "User",
                    "Login",
                    user.Id,
                    JsonSerializer.Serialize(new { user.UserName, user.Email })
                );
                return Ok(new { message = "Login successful" });
            }
            if (result.IsLockedOut)
            {
                return BadRequest(new { message = "Account is locked out" });
            }
            return BadRequest(new { message = "Invalid email or password" });
        }

        /// <summary>
        /// Logs out the current user and ends their session
        /// </summary>
        /// <returns>ActionResult indicating success</returns>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            var user = await _userManager.GetUserAsync(User);
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
            return Ok(new { message = "Logout successful" });
        }

        /// <summary>
        /// Changes the current user's password
        /// </summary>
        /// <param name="model">Password change information</param>
        /// <returns>ActionResult indicating success or failure</returns>
        [HttpPost("change-password")]
        [Authorize]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                await _auditService.LogActionAsync(
                    "User",
                    "ChangePassword",
                    user.Id,
                    JsonSerializer.Serialize(new { user.UserName, user.Email })
                );
                return Ok(new { message = "Password changed successfully" });
            }
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        /// <summary>
        /// Updates the current user's profile information
        /// </summary>
        /// <param name="model">Updated user information</param>
        /// <returns>ActionResult indicating success or failure</returns>
        [HttpPut("profile")]
        [Authorize]
        public async Task<IActionResult> UpdateProfile(UpdateProfileModel model)
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
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
                return Ok(new { message = "Profile updated successfully" });
            }
            return BadRequest(new { errors = result.Errors.Select(e => e.Description) });
        }

        /// <summary>
        /// Gets the current user's profile information
        /// </summary>
        /// <returns>ActionResult with user profile information</returns>
        [HttpGet("profile")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            var roles = await _userManager.GetRolesAsync(user);
            return Ok(new
            {
                user.Id,
                user.UserName,
                user.Email,
                user.FirstName,
                user.LastName,
                user.PhoneNumber,
                user.CreatedAt,
                user.IsActive,
                Roles = roles
            });
        }
    }

    public class ChangePasswordModel
    {
        [Required]
        public string CurrentPassword { get; set; } = null!;

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public string NewPassword { get; set; } = null!;
    }
} 
