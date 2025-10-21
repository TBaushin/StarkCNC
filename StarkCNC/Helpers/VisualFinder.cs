using System.Windows;
using System.Windows.Media;

namespace StarkCNC.Helpers;

public static class VisualFinder
{
    public static IEnumerable<T> FindVisualChildren<T>(DependencyObject parent) where T : DependencyObject
    {
        int childCount = VisualTreeHelper.GetChildrenCount(parent);

        for (int i = 0; i < childCount; i++)
        {
            var child = VisualTreeHelper.GetChild(parent, i);
            if (child is T t)
                yield return t;

            foreach (var childOfChild in FindVisualChildren<T>(child))
                yield return childOfChild;
        }
    }

    public static T? FindVisualParent<T>(DependencyObject child) where T : DependencyObject
    {
        var parent = VisualTreeHelper.GetParent(child);

        while (parent is not null && !(parent is T))
        {
            parent = VisualTreeHelper.GetParent(parent);
        }

        return parent as T;
    }
}