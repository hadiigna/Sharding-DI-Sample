namespace Sharding_DI_Sample;

public class DbConnectionContext
{
    /**
     * ダミーですが、シャードされたユーザーDBへの接続だと思ってください
     */
    public string DbConnectionId;
}

public interface IRepositoryProvider
{
    public DbConnectionContext GetDbConnectionContext();
}