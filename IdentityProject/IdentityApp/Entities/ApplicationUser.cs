using Microsoft.AspNetCore.Identity;

namespace IdentityApp.Entities;

public class ApplicationUser : IdentityUser
{
    public string Name { get; set; }
}
