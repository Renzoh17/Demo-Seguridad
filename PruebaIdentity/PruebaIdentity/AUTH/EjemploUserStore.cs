namespace PruebaIdentity.AUTH;

using Microsoft.AspNetCore.Identity;
using PruebaIdentity.Data;

public class EjemploUserStore : IUserStore<User>
{
    private readonly ApplicationDbContext _context;
    public EjemploUserStore(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IdentityResult> CreateAsync(User user, CancellationToken cancellationToken)
    {
        await _context.Usuarios.AddAsync(user);
        await _context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public async Task<IdentityResult> DeleteAsync(User user, CancellationToken cancellationToken)
    {
        _context.Usuarios.Remove(user);
        await _context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }

    public void Dispose()
    {
    }

    public async Task<User?> FindByIdAsync(string userId, CancellationToken cancellationToken)
    {
        if (!long.TryParse(userId, out var id)) return null;
        return await _context.Usuarios.FindAsync([id], cancellationToken);
    }

    public async Task<User?> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
    {
        var userName = normalizedUserName.ToUpper();
        return await _context.Usuarios.FindAsync(userName, cancellationToken);
    }

    public Task<string?> GetNormalizedUserNameAsync(User user, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<string> GetUserIdAsync(User user, CancellationToken cancellationToken)
    {
        return user.Id.ToString();
    }

    public async Task<string?> GetUserNameAsync(User user, CancellationToken cancellationToken)
    {
        return user.Name;
    }

    public async Task SetNormalizedUserNameAsync(User user, string? normalizedName, CancellationToken cancellationToken)
    {
        if (normalizedName is not null) user.Name = normalizedName;
    }

    public Task SetUserNameAsync(User user, string? userName, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<IdentityResult> UpdateAsync(User user, CancellationToken cancellationToken)
    {
        _context.Usuarios.Update(user);
        await _context.SaveChangesAsync(cancellationToken);
        return IdentityResult.Success;
    }
}
