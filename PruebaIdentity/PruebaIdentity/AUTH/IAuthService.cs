namespace PruebaIdentity.AUTH;

public interface IAuthService
{
    Task<string?> LoginAsync(LoginDTO dto);
    Task<IEnumerable<string>> RegisterAsync(RegisterDTO dto);
}