using CommunityToolkit.Mvvm.Input;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace StarkCNC.Models;

public class ViewData
{
    public ICommand NavigationCommand { get; }

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