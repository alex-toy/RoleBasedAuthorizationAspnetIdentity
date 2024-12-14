using IdentityApp.Dtos;
using IdentityApp.Entities;
using IdentityApp.Utils;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace IdentityApp.Repo.Authentication;

public class UserAuthentication : IUserAuthentication
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public UserAuthentication(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
    }

    public async Task<StatusDto> RegisterAsync(RegistrationDto model)
    {
        StatusDto statusDto = new ();
        ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);
        if (user is not null)
        {
            statusDto.StatusCode = 0;
            statusDto.Message = "User already exist";
            return statusDto;
        }

        user = new ApplicationUser()
        {
            Email = model.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = model.Username,
            FirstName = model.FirstName,
            LastName = model.LastName,
            EmailConfirmed = true,
            PhoneNumberConfirmed = true,
        };

        IdentityResult result = await _userManager.CreateAsync(user, model.Password);
        if (!result.Succeeded)
        {
            statusDto.StatusCode = AuthenticationStatus.Failure;
            statusDto.Message = "User creation failed";
            return statusDto;
        }

        if (!await _roleManager.RoleExistsAsync(model.Role)) await _roleManager.CreateAsync(new IdentityRole(model.Role));

        if (await _roleManager.RoleExistsAsync(model.Role)) await _userManager.AddToRoleAsync(user, model.Role);

        statusDto.StatusCode = AuthenticationStatus.Success;
        statusDto.Message = "You have registered successfully";
        return statusDto;
    }

    public async Task<StatusDto> LoginAsync(LoginDto model)
    {
        StatusDto StatusDto = new();
        ApplicationUser user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            StatusDto.StatusCode = AuthenticationStatus.Failure;
            StatusDto.Message = "Invalid username";
            return StatusDto;
        }

        if (!await _userManager.CheckPasswordAsync(user, model.Password))
        {
            StatusDto.StatusCode = AuthenticationStatus.Failure;
            StatusDto.Message = "Invalid Password";
            return StatusDto;
        }

        SignInResult signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, true);

        if (!signInResult.Succeeded)
        {
            StatusDto.StatusCode = AuthenticationStatus.Failure;
            StatusDto.Message = signInResult.IsLockedOut ? "User is locked out" : "Error on logging in";
            return StatusDto;
        }

        IList<string> userRoles = await _userManager.GetRolesAsync(user);
        AddClaims(user, userRoles);

        StatusDto.StatusCode = AuthenticationStatus.Success;
        StatusDto.Message = "Logged in successfully";

        return StatusDto;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    public async Task<StatusDto> ChangePasswordAsync(ChangePasswordDto model)
    {
        StatusDto StatusDto = new ();

        ApplicationUser? user = await _userManager.FindByEmailAsync(model.Email);
        if (user is null)
        {
            StatusDto.Message = "User does not exist";
            StatusDto.StatusCode = AuthenticationStatus.Failure;
            return StatusDto;
        }

        IdentityResult result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

        if (!result.Succeeded)
        {
            StatusDto.Message = "Some error occcured";
            StatusDto.StatusCode = 0;
            return StatusDto;
        }

        StatusDto.Message = "Password has updated successfully";
        StatusDto.StatusCode = AuthenticationStatus.Success;
        return StatusDto;
    }

    private static void AddClaims(ApplicationUser user, IList<string> userRoles)
    {
        List<Claim> authClaims = new () { new Claim(ClaimTypes.Name, user.UserName) };

        foreach (var userRole in userRoles) authClaims.Add(new Claim(ClaimTypes.Role, userRole));
    }
}
