using System.Windows.Input;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace Client.Avalonia.Controls.Common;

public partial class HeaderBar : UserControl
    {
        public static readonly StyledProperty<string> TitleProperty =
            AvaloniaProperty.Register<HeaderBar, string>(nameof(Title), "Dashboard");
            
        public static readonly StyledProperty<string> PathProperty =
            AvaloniaProperty.Register<HeaderBar, string>(nameof(Path), "Übersicht");
            
        public static readonly StyledProperty<bool> HasNotificationsProperty =
            AvaloniaProperty.Register<HeaderBar, bool>(nameof(HasNotifications), false);
            
        public static readonly StyledProperty<ICommand> SearchCommandProperty =
            AvaloniaProperty.Register<HeaderBar, ICommand>(nameof(SearchCommand));
            
        public static readonly StyledProperty<ICommand> NotificationsCommandProperty =
            AvaloniaProperty.Register<HeaderBar, ICommand>(nameof(NotificationsCommand));
            
        public static readonly StyledProperty<ICommand> HelpCommandProperty =
            AvaloniaProperty.Register<HeaderBar, ICommand>(nameof(HelpCommand));
            
        public string Title
        {
            get => GetValue(TitleProperty);
            set => SetValue(TitleProperty, value);
        }
        
        public string Path
        {
            get => GetValue(PathProperty);
            set => SetValue(PathProperty, value);
        }
        
        public bool HasNotifications
        {
            get => GetValue(HasNotificationsProperty);
            set => SetValue(HasNotificationsProperty, value);
        }
        
        public ICommand SearchCommand
        {
            get => GetValue(SearchCommandProperty);
            set => SetValue(SearchCommandProperty, value);
        }
        
        public ICommand NotificationsCommand
        {
            get => GetValue(NotificationsCommandProperty);
            set => SetValue(NotificationsCommandProperty, value);
        }
        
        public ICommand HelpCommand
        {
            get => GetValue(HelpCommandProperty);
            set => SetValue(HelpCommandProperty, value);
        }
        
        public HeaderBar()
        {
            InitializeComponent();
        }
        
        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }