using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.MachineCommunication.Services;
using StarkCNC.Services;
using System.Windows.Media;

namespace StarkCNC.Models
{
    public partial class OutputsParametersTwoButtons : ObservableObject
    {
        private readonly IManualConfigurationService _manualConfigurationService;

        [ObservableProperty]
        private Color _rearPosition = Colors.DarkRed;

        [ObservableProperty]
        private Color _frontPosition = Colors.DarkRed;

        public string ForwardRequestString { get; private set; } = string.Empty;

        public string BackwardRequestString { get; private set; } = string.Empty;

        public string RearPositionRequestString { get; private set; } = string.Empty;

        public string FrontPositionRequestString { get; private set; } = string.Empty;

        private Task? _updateTask;
        private CancellationTokenSource? _cancellationTokenSource;

        public OutputsParametersTwoButtons(IManualConfigurationService manualConfigurationService, bool autoRunUpdate) 
        {
            _manualConfigurationService = manualConfigurationService;

            if (autoRunUpdate)
                StartUpdateTask();
        }

        public void StartUpdateTask()
        {
            if (TaskIsRunning())
                return;

            _cancellationTokenSource?.Dispose();

            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            _updateTask = Task.Run(async () =>
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        await GetRearPosition().ConfigureAwait(false);
                        await GetFrontPosition().ConfigureAwait(false);
                        await Task.Delay(150, token).ConfigureAwait(false);
                    }
                }
                catch (TaskCanceledException)
                {
                    // Нормально: задача отменена
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"UpdateTask error: {ex}");
                }
            }, token);
        }

        private bool TaskIsRunning() =>
            _updateTask is not null && !_updateTask.IsCompleted && !_updateTask.IsCanceled && !_updateTask.IsFaulted;

        public void StopUpdateTask()
        {
            if (_updateTask is null)
                return;

            _cancellationTokenSource?.Cancel();
        }

        [RelayCommand]
        private async Task ForwardStart() =>
            await _manualConfigurationService
                .WriteAsync<bool>(true, ForwardRequestString)
                .ConfigureAwait(false);

        [RelayCommand]
        private async Task ForwardCancel() =>
            await _manualConfigurationService
                .WriteAsync<bool>(false, ForwardRequestString)
                .ConfigureAwait(false);

        [RelayCommand]
        private async Task BackwardStart() =>
            await _manualConfigurationService
                .WriteAsync<bool>(true, BackwardRequestString)
                .ConfigureAwait(false);

        [RelayCommand]
        private async Task BackwardCancel() =>
            await _manualConfigurationService
                .WriteAsync(false, BackwardRequestString)
                .ConfigureAwait(false);

        [RelayCommand]
        private async Task GetRearPosition()
        {
            try
            {
                var result = await _manualConfigurationService
                    .ReadAsync<bool>(RearPositionRequestString)
                    .ConfigureAwait(false);

                if (result)
                    RearPosition = Colors.Green;
                else
                    RearPosition = Colors.DarkRed;
            }
            catch (Opc.Ua.ServiceResultException)
            {
                return;
            }
        }

        [RelayCommand]
        private async Task GetFrontPosition()
        {
            try
            {
                var result = await _manualConfigurationService
                    .ReadAsync<bool>(FrontPositionRequestString)
                    .ConfigureAwait(false);

                if (result)
                    FrontPosition = Colors.Green;
                else
                    FrontPosition = Colors.DarkRed;
            }
            catch (Opc.Ua.ServiceResultException)
            {
                return;
            }
        }

        public static OutputsParametersTwoButtons InitializeParameters(IConfigurationSection configurationSection, IManualConfigurationService manualConfigurationService, string sectionName, bool autoRunUpdate)
        {
            IConfigurationSection? section = null;

            if (configurationSection is not null)
                section = configurationSection.GetSection(sectionName);

            return new OutputsParametersTwoButtons(manualConfigurationService, autoRunUpdate)
            {
                ForwardRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(ForwardRequestString)),
                BackwardRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(BackwardRequestString)),
                RearPositionRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(RearPositionRequestString)),
                FrontPositionRequestString = ConfigurationReaderService.GetRequestStringFromConfiguration(section, nameof(FrontPositionRequestString))
            };
        }
    }
}
