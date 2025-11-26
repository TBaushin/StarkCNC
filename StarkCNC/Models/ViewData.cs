using CommunityToolkit.Mvvm.Input;
using StarkCNC.Core.Services;
using System.Collections.ObjectModel;

namespace StarkCNC.Models;

public class ViewData
{
    public IRelayCommand NavigationCommand { get; }

    public string Title { get; set; }

    public string? IconGlyph { get; set; }

    public ObservableCollection<ViewData> Items { get; } = new ObservableCollection<ViewData>();

    public ViewData(string title, string? iconGlyph, IRelayCommand navigationCommand)
    {
        Title = title;
        IconGlyph = iconGlyph;
        NavigationCommand = navigationCommand;
    }
}