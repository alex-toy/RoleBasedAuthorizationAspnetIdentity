using IdentityApp.Dtos;

namespace IdentityApp.Repo.Authentication;

public interface IUserAuthentication
{
    Task<StatusDto> LoginAsync(LoginDto model);
    Task LogoutAsync();
    Task<StatusDto> RegisterAsync(RegistrationDto model);
    Task<StatusDto> ChangePasswordAsync(ChangePasswordDto model);
}
