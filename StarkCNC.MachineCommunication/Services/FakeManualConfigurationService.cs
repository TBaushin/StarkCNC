namespace StarkCNC.MachineCommunication.Services
{
    public class FakeManualConfigurationService : IManualConfigurationService
    {
        public bool Connected => GetRandomBool();

        public async Task ConnectAsync() => await Task.Delay(1000);

        public Task<T?> ReadAsync<T>(string from)
        {
            object? result = typeof(T) switch
            {
                var t when t == typeof(bool) => GetRandomBool(),
                var t when t == typeof(double) => GetRandomDouble(),
                var t when t == typeof(float) => GetRandomFloat(),
                _ => default(T)
            };

            return Task.FromResult((T?)result);
        }

        public async Task WriteAsync<T>(T value, string to) => await Task.Delay(100);

        private static bool GetRandomBool()
        {
            var random = new Random();
            var v = random.Next(2);
            return Convert.ToBoolean(v);
        }

        private static double GetRandomDouble()
        {
            var random = new Random();
            var v = random.NextDouble();
            return v * 100;
        }

        private static float GetRandomFloat() {
            var random = new Random();
            var v = random.NextDouble();
            return Convert.ToSingle(v * 100);
        }
    }
}
