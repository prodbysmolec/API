using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using Client.Core.ViewModels;

namespace Client.Avalonia;

public class ViewLocator : IDataTemplate
{

    public Control Build(object data)
    {
        if (data == null)
            return new TextBlock { Text = "Keine Daten vorhanden" };

        var viewModelName = data.GetType().FullName;

        if (viewModelName == null)
            return new TextBlock { Text = "ViewModel-Typ hat keinen Namen" };

        // Transformiere den Namen:
        // 1. Ersetze Namespace (von Core.ViewModels zu Avalonia.Views)
        // 2. Ersetze ViewModel durch View im Klassennamen
        var viewName = viewModelName
            .Replace("Core.ViewModels", "Avalonia.Views")
            .Replace("ViewModel", "View");

        // Versuche den View-Typ zu finden
        var viewType = Type.GetType(viewName);

        // Wenn der Typ nicht gefunden wurde, versuche ihn in geladenen Assemblies zu finden
        if (viewType == null)
        {
            // Durchsuche alle geladenen Assemblies
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                viewType = assembly.GetType(viewName);
                if (viewType != null)
                    break;
            }
        }

        // Wenn der View-Typ gefunden wurde, erstelle eine Instanz
        if (viewType != null)
        {
            try
            {
                var view = (Control)Activator.CreateInstance(viewType);
                view.DataContext = data;
                return view;
            }
            catch (Exception ex)
            {
                return new TextBlock { Text = $"Fehler beim Erstellen der View: {ex.Message}" };
            }
        }

        // Fallback für den Fall, dass kein passender View-Typ gefunden wurde
        return new TextBlock { Text = $"View nicht gefunden: {viewName}" };
    }

    public bool Match(object data)
    {
        // Alle ViewModels abdecken (sie sollten alle von ViewModelBase erben)
        return data is ViewModelBase;
    }
}
