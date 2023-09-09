using Microsoft.Extensions.Primitives;

namespace Sharding_DI_Sample;

/// <summary>
/// 各APIのリクエストに紐づく、ユーザー情報やユーザーのデータを取得するためのレポジトリを保管しているコンテキスト.
/// 必ずScopedで登録してください。 (Singletonが良い場合は <see cref="RepositoryProvider"/> のコメントを参照してください。)
/// </summary>
public class GameApiContext : IGameApiContext
{
    private IRepositoryProvider _repositoryProvider;
    private string _userId;
    
    // RepositoryProviderはDIで注入されます
    public GameApiContext(IRepositoryProvider repositoryProvider, IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext == null)
        {
            throw new Exception(
                "HttpContext is null! Check your GameApiContext registration method. (Must be registered as request-scoped lifetime)");
        }
        
        var userIdHeader = httpContextAccessor.HttpContext.Request.Headers["X-UserId"];
        if (userIdHeader == StringValues.Empty)
        {
            throw new Exception("X-UserId header is missing in HTTP request!");
        }

        _userId = userIdHeader.ToString();
        
        _repositoryProvider = repositoryProvider;
    }

    public string GetUserId()
    {
        return _userId;
    }

    public IRepositoryProvider GetUserRepositoryProvider()
    {
        return _repositoryProvider;
    }
}