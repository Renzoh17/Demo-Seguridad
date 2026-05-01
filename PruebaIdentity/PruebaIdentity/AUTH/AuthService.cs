using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace PruebaIdentity.AUTH;

public class AuthService : IAuthService
{
    private readonly UserManager<User> _userManager;
    private readonly IConfiguration _config;

    public AuthService(UserManager<User> userManager, IConfiguration config)
    {
        _userManager = userManager;
        _config = config;
    }

    public async Task<string?> LoginAsync(LoginDTO dto)
    {
        // Lo busco y Valido que exista.
        var user = await _userManager.FindByNameAsync(dto.Name);
        if (user is null) return null;
        var passwordValido = await _userManager.CheckPasswordAsync(user, dto.Password);
        if (!passwordValido) return null;
        // Le genero y devuelvo el token.
        return GenerarJwt(user);
    }

    public async Task<IEnumerable<string>> RegisterAsync(RegisterDTO dto)
    {
        var user = new User { Name = dto.Name };
        var result = await _userManager.CreateAsync(user, dto.Password);

        // Si hay errores los devuelve, si no devuelve lista vacía
        return result.Succeeded ? []: result.Errors.Select(e => e.Description);
    }

    private string GenerarJwt(User user)
    {
        var claims = new[]
        { // Los claims son la informacion que vas a meter dentro del token.
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Name),
        };

        // Creo la clave con la que se firma el token. 
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]!));
        //  Construye y serializa el token.
        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.UtcNow.AddHours(1),
            signingCredentials: new SigningCredentials(key, SecurityAlgorithms.HmacSha256)
        );
        return new JwtSecurityTokenHandler().WriteToken(token);  // Lo convierto a string.
    }
}