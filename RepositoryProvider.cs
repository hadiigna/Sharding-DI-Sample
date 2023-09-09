using Microsoft.Extensions.Primitives;

namespace Sharding_DI_Sample;

/**
 * 各リクエストに紐づくレポジトリのプロバイダー
 * 必ずScopedライフタイムでDIコンテナに登録してください！
 * (一応Singletonでもちょっとややこしくなりますが、できます。コンストラクタ内でhttpContextAccessorそのものをクラスのプロパティに保存してください)
 */
public class RepositoryProvider : IRepositoryProvider
{
    private DbConnectionContext _dbConnectionContext;
    
    // 参考: https://learn.microsoft.com/en-us/aspnet/core/fundamentals/http-context?view=aspnetcore-7.0&source=recommendations#access-httpcontext-from-custom-components
    // DIでHttpのコンテキスト(ヘッダー内容) に依存していますよ、ということをDIに教えるには
    // IHttpContextAccessor を使う.
    // (今回はリクエストスコープ前提でこのRepositoryProviderを作っているので、ヘッダーはコンストラクタ内で参照する)
    // (もしSingletonがいい場合は、コンストラクタ内で直接HttpContextを見ずに、別のメソッドでリクエスト中にHttpContextAccessor.HttpContextを参照してください)
    public RepositoryProvider(IHttpContextAccessor httpContextAccessor)
    {
        if (httpContextAccessor.HttpContext == null)
        {
            throw new Exception(
                "HttpContext is null! Check your RepositoryProvider registration method. (Must be registered as request-scoped lifetime)");
        }
        
        var userIdHeader = httpContextAccessor.HttpContext.Request.Headers["X-UserId"];
        if (userIdHeader == StringValues.Empty)
        {
            throw new Exception("X-UserId header is missing in HTTP request!");
        }

        var userIdHeaderPrefix = userIdHeader.ToString()[0];
        if ('0' <= userIdHeaderPrefix && userIdHeaderPrefix <= '9')
        {
            _dbConnectionContext = new DbConnectionContext()
            {
                DbConnectionId = "UserDB-Shard_0"
            };
        } else if ('a' <= userIdHeaderPrefix && userIdHeaderPrefix <= 'f')
        {
            _dbConnectionContext = new DbConnectionContext()
            {
                DbConnectionId = "UserDB-Shard_1"
            };
        }
        else
        {
            throw new Exception("Invalid X-UserId value. User Id must start with [0-9a-f].");
        }
    }

    public DbConnectionContext GetDbConnectionContext()
    {
        return _dbConnectionContext;
    }
}