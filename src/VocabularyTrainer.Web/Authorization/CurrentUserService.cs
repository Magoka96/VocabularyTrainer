using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using VocabularyTrainer.Application.Interfaces;

namespace VocabularyTrainer.Web.Authorization;

public class CurrentUserService(AuthenticationStateProvider asp) : ICurrentUserService
{
    public async Task<ClaimsPrincipal> GetUserAsync()
    {
        var authState = await asp.GetAuthenticationStateAsync();
        return authState.User;
    }

    public async Task<string?> GetUserNamerAsync()
    {
        var user = await GetUserAsync();
        return user?.Identity?.Name;
    }

    public async Task<bool> IsAuthenticatedAsync()
    {
        var user = await GetUserAsync();
        return user?.Identity?.IsAuthenticated ?? false;
    }

    public async Task<IReadOnlyCollection<string>> GetRolesAsync()
    {
        var user = await GetUserAsync();
        var roles = user?.FindAll(ClaimTypes.Role).Select(c => c.Value);
        return roles?.ToArray() ?? [];
    }

    public async Task<string?> GetUserIdAsync()
    {
        var user = await GetUserAsync();
        return user?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
    }
}
