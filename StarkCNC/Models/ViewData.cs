using System.Collections.ObjectModel;
using System.Windows.Controls;

namespace StarkCNC.Models;

public class ViewData
{
    private Page _page;

    public string Title { get; set; }

    public string? IconGlyph { get; set; }

    public ObservableCollection<ViewData> Items { get; } = new ObservableCollection<ViewData>();

    public Page Page 
    { 
        get 
        {
            return _page; 
        }
        private set 
        { 
            _page = value;
            Title = _page.Title;
        } 
    }

    public ViewData(Page page)
    {
        _page = page;
        Title = _page.Title;
    }
}