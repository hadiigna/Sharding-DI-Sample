namespace Sharding_DI_Sample;

public interface IGameApiContext
{
    public string GetUserId();
    public IRepositoryProvider GetUserRepositoryProvider();
}