using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.Configuration;
using StarkCNC.MachineCommunication.Services;
using System.Windows.Media;

namespace StarkCNC.Models
{
    public partial class SqueezeParameters : ObservableObject
    {
        private readonly IManualConfigurationService _manualConfigurationService;

        [ObservableProperty]
        private Color _rearPosition = Colors.DarkRed;

        [ObservableProperty]
        private Color _rearSecondPosition = Colors.DarkRed;
        
        [ObservableProperty]
        private Color _frontPosition = Colors.DarkRed;

        [ObservableProperty]
        private Color _frontSecondPosition = Colors.DarkRed;

        public string ForwardRequestString { get; private set; } = string.Empty;

        public string BackwardRequestString { get; private set; } = string.Empty;

        public string RearPositionRequestString { get; private set; } = string.Empty;

        public string RearSecondPositionRequestString { get; private set; } = string.Empty;

        public string FrontPositionRequestString { get; private set; } = string.Empty;

        public string FrontSecondPositionRequestString { get; private set; } = string.Empty;

        private Task _updateTask;
        private CancellationTokenSource? _cancellationTokenSource;

        public SqueezeParameters(IManualConfigurationService manualConfigurationService, bool autoRunUpdate)
        {
            _manualConfigurationService = manualConfigurationService;

            if (autoRunUpdate)
                StartUpdateTask();
        }

        public void StartUpdateTask()
        {
            if (_updateTask is not null && !_updateTask.IsCompleted && !_updateTask.IsCanceled && !_updateTask.IsFaulted)
            {
                return;
            }

            _cancellationTokenSource?.Dispose();

            _cancellationTokenSource = new CancellationTokenSource();
            var token = _cancellationTokenSource.Token;

            _updateTask = Task.Run(async () =>
            {
                try
                {
                    while (!token.IsCancellationRequested)
                    {
                        await GetRearPosition();
                        await GetRearSecondPosition();
                        await GetFrontPosition();
                        await GetFrontSecondPosition();
                        await Task.Delay(150, token);
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

        public void StopUpdateTask()
        {
            if (_updateTask == null)
                return;

            _cancellationTokenSource?.Cancel();
        }

        [RelayCommand]
        private async Task ForwardStart() => await _manualConfigurationService.WriteAsync<bool>(true, ForwardRequestString);

        [RelayCommand]
        private async Task ForwardCancel() => await _manualConfigurationService.WriteAsync<bool>(false, ForwardRequestString);

        [RelayCommand]
        private async Task BackwardStart() => await _manualConfigurationService.WriteAsync<bool>(true, BackwardRequestString);

        [RelayCommand]
        private async Task BackwardCancel() => await _manualConfigurationService.WriteAsync(false, BackwardRequestString);

        [RelayCommand]
        private async Task GetRearPosition() 
        { 
            var value = await _manualConfigurationService.ReadAsync<bool>(RearPositionRequestString);
            if (value)
                RearPosition = Colors.Green;
            else
                RearPosition = Colors.DarkRed;
        }

        [RelayCommand]
        private async Task GetRearSecondPosition()
        {
            var value = await _manualConfigurationService.ReadAsync<bool>(RearSecondPositionRequestString);
            if (value)
                RearSecondPosition = Colors.Green;
            else
                RearSecondPosition = Colors.DarkRed;
        }

        [RelayCommand]
        private async Task GetFrontPosition()
        {
            var value = await _manualConfigurationService.ReadAsync<bool>(FrontPositionRequestString);
            if (value)
                FrontPosition = Colors.Green;
            else
                FrontPosition = Colors.DarkRed;
        }

        [RelayCommand]
        private async Task GetFrontSecondPosition()
        {
            var value = await _manualConfigurationService.ReadAsync<bool>(FrontSecondPositionRequestString);
            if (value)
                FrontSecondPosition = Colors.Green;
            else
                FrontSecondPosition = Colors.DarkRed;
        }

        public static SqueezeParameters InitializeParameters(IConfigurationSection configurationSection, IManualConfigurationService manualConfigurationService, string sectionName, bool autoRunUpdate)
        {
            var section = configurationSection.GetSection(sectionName);

            return new SqueezeParameters(manualConfigurationService, autoRunUpdate)
            {
                ForwardRequestString = section.GetValue<string>(nameof(ForwardRequestString)) ?? string.Empty,
                BackwardRequestString = section.GetValue<string>(nameof(BackwardRequestString)) ?? string.Empty,
                RearPositionRequestString = section.GetValue<string>(nameof(RearPositionRequestString)) ?? string.Empty,
                RearSecondPositionRequestString = section.GetValue<string>(nameof(RearSecondPositionRequestString)) ?? string.Empty,
                FrontPositionRequestString = section.GetValue<string>(nameof(FrontPositionRequestString)) ?? string.Empty,
                FrontSecondPositionRequestString = section.GetValue<string>(nameof(FrontSecondPositionRequestString)) ?? string.Empty
            };
        }
    }
}
