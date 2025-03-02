using HTTP_Application.Scenes;
using System.Windows;

namespace HTTP_Application;
public enum ThreadStatus
{ 
    Running,
    Stopped,
    Cancelled,
    Completed
}
public partial class MainWindow : Window
{
    public DownloadScene DownloadScene { get; set; }
    public ActiveScene ActiveScene { get; set; }
    public ArchiveScene ArchiveScene { get; set; }
    public MainWindow()
    {
        this.DownloadScene = new DownloadScene(this);
        this.ArchiveScene = new ArchiveScene(this);
        this.ActiveScene = new ActiveScene(this);
        InitializeComponent();
        sceneContainer.Content = this.DownloadScene;
        DataContext = this;
    }
    private void DownloadRadioButtonChecked(object sender, RoutedEventArgs e)
    {
        if (sceneContainer == null) return;
        sceneContainer.Content = this.DownloadScene;
    }
    private void ActiveRadioButtonChecked(object sender, RoutedEventArgs e)
    {
        sceneContainer.Content = this.ActiveScene;
    }
    private void ArchiveRadioButtonChecked(object sender, RoutedEventArgs e)
    {
        sceneContainer.Content = this.ArchiveScene;
    }
    private void Exit(object sender, RoutedEventArgs e)
    {
        this.Close();
    }
}