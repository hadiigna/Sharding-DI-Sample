using Microsoft.AspNetCore.Mvc;

namespace Sharding_DI_Sample.Controllers;

public class HomeApiResponse
{
    public string userId { get; set; }
    public string shardId { get; set; }
}

[ApiController]
[Route("/home")]
public class HomeApiController : ControllerBase
{

    private readonly ILogger<HomeApiController> _logger;

    public HomeApiController(ILogger<HomeApiController> logger)
    {
        _logger = logger;
    }

    /// <summary>
    /// HTTPのヘッダーに X-UserId キーでユーザIDっぽい文字列 (0-9またはa-fで始まる任意の文字列) を指定すると、
    /// そのユーザーIDと、DBへの接続ID (ダミー) を返す。
    /// ユーザーIDの文字列が 0-9 なら シャードID 0、 a-f ならシャードID 1 を返す。
    /// </summary>
    [HttpGet]
    public HomeApiResponse Get([FromServices] IGameApiContext ctx)
    {
        return new HomeApiResponse()
        {
            userId = ctx.GetUserId(),
            shardId = ctx.GetUserRepositoryProvider().GetDbConnectionContext().DbConnectionId
        };
    }
}