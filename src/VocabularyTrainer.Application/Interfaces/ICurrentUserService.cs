using System.Security.Claims;

namespace VocabularyTrainer.Application.Interfaces;

public interface ICurrentUserService
{
    Task<ClaimsPrincipal> GetUserAsync();
    Task<string?> GetUserNamerAsync();
    Task<bool> IsAuthenticatedAsync();
    Task<IReadOnlyCollection<string>> GetRolesAsync();
    Task<string?> GetUserIdAsync();
}
