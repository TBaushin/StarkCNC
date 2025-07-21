using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;

namespace StarkCNC.Models
{
    internal partial class DriveParameters : ObservableObject
    {
        private readonly IConfigurationSection _configuration;

        [ObservableProperty]
        private double speed = 0;

        [ObservableProperty]
        private double coordinate = 0;

        [ObservableProperty]
        private double relativeDisplacement = 0;

        public string ForwardRequestString { get; private set; } = string.Empty;
        
        public string BackwardRequestString { get; private set; } = string.Empty;
        
        public string ActualCoordinateRequestString { get; private set; } = string.Empty;

        public string ActualRelativeDisplacementRequestString { get; private set; } = string.Empty;

        public string ResetRequestString { get; private set; } = string.Empty;

        public string SpeedRequestString { get; private set; } = string.Empty;

        public DriveParameters(IConfigurationSection configuration, string sectionName)
        {
            _configuration = configuration;

            InitializeRequestString(sectionName);
        }

        public void Reset()
        {
            Speed = 0;
            Coordinate = 0;
            RelativeDisplacement = 0;
        }

        private void InitializeRequestString(string sectionName)
        {
            var section = _configuration.GetSection(sectionName);

            ForwardRequestString = section.GetValue<string>(nameof(ForwardRequestString)) ?? string.Empty;
            BackwardRequestString = section.GetValue<string>(nameof(BackwardRequestString)) ?? string.Empty;
            ActualCoordinateRequestString = section.GetValue<string>(nameof(ActualCoordinateRequestString)) ?? string.Empty;
            ActualRelativeDisplacementRequestString = section.GetValue<string>(nameof(ActualRelativeDisplacementRequestString)) ?? string.Empty;
            ResetRequestString = section.GetValue<string>(nameof(ResetRequestString)) ?? string.Empty;
            SpeedRequestString = section.GetValue<string>(nameof(SpeedRequestString)) ?? string.Empty;
        }
    }
}
