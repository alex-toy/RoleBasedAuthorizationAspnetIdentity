using IdentityApp.Dtos;
using IdentityApp.Repo.Authentication;
using IdentityApp.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers;

public class AuthenticationController : Controller
{
    private readonly IUserAuthentication _authService;

    public AuthenticationController(IUserAuthentication authService)
    {
        _authService = authService;
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto model)
    {
        if (!ModelState.IsValid) return View(model);

        StatusDto result = await _authService.LoginAsync(model);
        if (result.StatusCode == AuthenticationStatus.Success) return RedirectToAction("Display", "Dashboard");

        TempData["msg"] = result.Message;
        return RedirectToAction(nameof(Login));
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegistrationDto model)
    {
        if (!ModelState.IsValid) { return View(model); }
        model.Role = "user";
        StatusDto result = await this._authService.RegisterAsync(model);
        TempData["msg"] = result.Message;
        return RedirectToAction(nameof(Register));
    }

    [Authorize]
    public async Task<IActionResult> Logout()
    {
        await this._authService.LogoutAsync();
        return RedirectToAction(nameof(Login));
    }

    [AllowAnonymous]
    public async Task<IActionResult> RegisterAdmin()
    {
        RegistrationDto model = new ()
        {
            Username = "admin",
            Email = "admin@gmail.com",
            FirstName = "John",
            LastName = "Doe",
            Password = "Admin@12345#"
        };
        model.Role = "admin";
        StatusDto result = await this._authService.RegisterAsync(model);
        return Ok(result);
    }

    [Authorize]
    public IActionResult ChangePassword()
    {
        return View();
    }

    [Authorize]
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordDto model)
    {
        if (!ModelState.IsValid) return View(model);
        StatusDto result = await _authService.ChangePasswordAsync(model);
        TempData["msg"] = result.Message;
        return RedirectToAction(nameof(ChangePassword));
    }
}
