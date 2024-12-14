using IdentityApp.Utils;

namespace IdentityApp.Dtos;

public class StatusDto
{
    public AuthenticationStatus StatusCode { get; set; }
    public string Message { get; set; }
}
