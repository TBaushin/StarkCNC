using System.Windows.Controls;

namespace StarkCNC.Models
{
    public class ViewData
    {
        private Page _page;

        public string Title { get; private set; }

        public string IconGlyph { get; set; }

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
            Page = page;
        }
    }
}
