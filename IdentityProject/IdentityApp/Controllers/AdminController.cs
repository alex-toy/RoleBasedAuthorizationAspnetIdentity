using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers;


[Authorize(Roles = "admin")]
public class AdminController : Controller
{
    public IActionResult Display()
    {
        return View();
    }
}