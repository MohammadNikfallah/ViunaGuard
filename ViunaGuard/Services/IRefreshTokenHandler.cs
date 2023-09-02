using System.Text.Json;

namespace ViunaGuard.Services;

public interface IRefreshTokenHandler
{
    public Task<JsonDocument> AccessRefresh(string refreshToken);
}