using cookie_authentication_authorization_demo.Models;
using Microsoft.AspNetCore.Identity;

namespace cookie_authentication_authorization_demo.Services;

/// <summary>
/// Interface for authentication service operations
/// </summary>
public interface IAuthService
{
    /// <summary>
    /// Registers a new user
    /// </summary>
    /// <param name="model">User registration information</param>
    /// <returns>IdentityResult indicating success or failure</returns>
    Task<IdentityResult> RegisterAsync(RegisterModel model);

    /// <summary>
    /// Authenticates a user
    /// </summary>
    /// <param name="model">User login credentials</param>
    /// <returns>SignInResult indicating success or failure</returns>
    Task<SignInResult> LoginAsync(LoginModel model);

    /// <summary>
    /// Logs out the current user
    /// </summary>
    Task LogoutAsync();

    /// <summary>
    /// Changes a user's password
    /// </summary>
    /// <param name="userId">ID of the user</param>
    /// <param name="currentPassword">Current password</param>
    /// <param name="newPassword">New password</param>
    /// <returns>IdentityResult indicating success or failure</returns>
    Task<IdentityResult> ChangePasswordAsync(string userId, string currentPassword, string newPassword);

    /// <summary>
    /// Updates a user's profile
    /// </summary>
    /// <param name="userId">ID of the user</param>
    /// <param name="model">Updated profile information</param>
    /// <returns>IdentityResult indicating success or failure</returns>
    Task<IdentityResult> UpdateProfileAsync(string userId, UpdateProfileModel model);

    /// <summary>
    /// Deactivates a user's account
    /// </summary>
    /// <param name="userId">ID of the user</param>
    /// <returns>IdentityResult indicating success or failure</returns>
    Task<IdentityResult> DeleteAccountAsync(string userId);

    /// <summary>
    /// Gets a user by their ID
    /// </summary>
    /// <param name="userId">ID of the user</param>
    /// <returns>The user if found, null otherwise</returns>
    Task<ApplicationUser?> GetUserByIdAsync(string userId);

    /// <summary>
    /// Gets a user by their email
    /// </summary>
    /// <param name="email">Email of the user</param>
    /// <returns>The user if found, null otherwise</returns>
    Task<ApplicationUser?> GetUserByEmailAsync(string email);

    /// <summary>
    /// Gets all roles for a user
    /// </summary>
    /// <param name="user">The user</param>
    /// <returns>List of role names</returns>
    Task<IList<string>> GetUserRolesAsync(ApplicationUser user);

    /// <summary>
    /// Checks if a user is in a specific role
    /// </summary>
    /// <param name="user">The user</param>
    /// <param name="role">The role to check</param>
    /// <returns>True if the user is in the role, false otherwise</returns>
    Task<bool> IsUserInRoleAsync(ApplicationUser user, string role);
} 