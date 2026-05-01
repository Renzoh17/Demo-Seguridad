using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using PruebaIdentity.Data;


namespace PruebaIdentity.AUTH;

public class EjemploUserStore : IUserStore<User>, IUserPasswordStore<User>
{
    private readonly ApplicationDbContext _context;

    public EjemploUserStore(ApplicationDbContext context) => _context = context;

    public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
    {
        await _context.Usuarios.AddAsync(user);
        await _context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _context.Usuarios.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
    {
        _context.Usuarios.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        if (!long.TryParse(userId, out var id)) return null;
        return await _context.Usuarios.FindAsync([id], cancellationToken);
    }

    public async Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        return await _context.Usuarios.FirstOrDefaultAsync(u => u.Name.ToUpper() == normalizedUserName, cancellationToken);
    }

    public void Dispose()
    {
    }

    public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken) => user.Id.ToString();


    public async Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken) => user.Name;


    public Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken) => Task.CompletedTask;

    public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }
    public Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task SetPasswordHashAsync(User user, string? passwordHash, CancellationToken ct)
    {
        user.Password = passwordHash!;
        return Task.CompletedTask;
    }

    public Task<string?> GetPasswordHashAsync(User user, CancellationToken ct) 
        => Task.FromResult<string?>(user.Password);

    public Task<bool> HasPasswordAsync(User user, CancellationToken ct)
        => Task.FromResult(!string.IsNullOrEmpty(user.Password));
}