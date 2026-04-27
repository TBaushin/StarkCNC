using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.Extensions.Configuration;
using StarkCNC.Models;
using StarkCNC.Services;
using System.Windows.Media.Media3D;

namespace StarkCNC.ViewModels;

public class VisualizationViewModel : ViewModelBase
{
    private readonly IBendingModelsLoadingService _bendingModelsLoadingService;

    public VisualizationViewModel(IBendingModelsLoadingService bendingModelsLoadingService)
    {
        _bendingModelsLoadingService = bendingModelsLoadingService;

        LoadModels();
    }

    public ModelVisual3D GetModels()
    {
        return _bendingModelsLoadingService.GetModelVisual3D();
    }

    public ModelVisual3D GetPipe()
    {
        return _bendingModelsLoadingService.Pipe;
    }

    public void UpdatePositions(double consolePosX, double bendRotationX, double carriagePosY, double height, double clampPosX, double pressPosX)
    {
        _bendingModelsLoadingService.UpdatePositions(consolePosX, bendRotationX, carriagePosY, height, clampPosX, pressPosX);
    }

    public Dictionary<string, double> GetDefaults()
    {
        return _bendingModelsLoadingService.GetDefault();
    }

    private void LoadModels()
    {
        ICollection<LoadingModel>? loadingModels = App.Configuration.GetSection("ModelsPath").Get<ICollection<LoadingModel>>();
        if (loadingModels is null)
            return;
        foreach (var item in loadingModels)
        {
            _bendingModelsLoadingService.Load(item.Path, item.Type);
        }
    }
}