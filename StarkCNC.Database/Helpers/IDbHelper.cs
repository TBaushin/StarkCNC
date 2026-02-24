namespace StarkCNC.Database.Helpers;

public interface IDbHelper
{
    public void Read();

    public Task Save();

    public Task Delete();
}