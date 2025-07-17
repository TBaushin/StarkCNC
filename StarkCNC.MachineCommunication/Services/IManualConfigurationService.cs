namespace StarkCNC.MachineCommunication.Services
{
    public interface IManualConfigurationService
    {
        public bool Connected { get; }

        public Task ConnectAsync();

        public Task WriteAsync<T>(T value, string to);

        public Task<T> ReadAsync<T>(string from);
    }
}
