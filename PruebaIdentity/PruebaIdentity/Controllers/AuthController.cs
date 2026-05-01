using Microsoft.AspNetCore.Mvc;
using PruebaIdentity.AUTH;

namespace PruebaIdentity.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginDTO dto)
    {
        var token = await _authService.LoginAsync(dto);
        if (token is null) return Unauthorized("Credenciales inválidas");
        return Ok(new { token });
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
    {
        var errores = await _authService.RegisterAsync(dto);
        if (errores.Any()) return BadRequest(errores);
        return Ok(new { mensaje = "Usuario creado correctamente" });
    }
}