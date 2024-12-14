using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IdentityApp.Controllers;

[Authorize]
public class DashboardController : Controller
{
    public IActionResult Display()
    {
        return View();
    }
}
