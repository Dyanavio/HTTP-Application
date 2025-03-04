using HTTP_Application.Core;
using Microsoft.Win32;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;

namespace HTTP_Application.Scenes
{
    public partial class DownloadScene : UserControl, INotifyPropertyChanged
    {
        private int currentThreads = 0;
        private int threads;
        private string destination = "";
        private string source = "";
        public event PropertyChangedEventHandler? PropertyChanged;
        public int Threads
        {
            get => threads;
            set
            {
                if(threads != value)
                {
                    threads = value;
                    OnPropertyChanged(nameof(Threads));
                }
            }
        }
        public string Destination
        {
            get => destination;
            set
            {
                if (destination != value)
                {
                    destination = value;
                    OnPropertyChanged(nameof(Destination));
                }
            }
        }
        public string Source
        {
            get => source;
            set
            {
                if(source != value)
                {
                    source = value;
                    OnPropertyChanged(nameof(Source));
                }
            }
        }
        public MainWindow MainWindow { get; set; }
       
        public DownloadScene(MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = this;
            //Destination = @"D:\FilesForProject\DestinationFolder\video.mp4";
            //Source = "https://videos.pexels.com/video-files/30772111/13163026_2560_1440_60fps.mp4";
            Threads = 1;
            ThreadNumberTextBox.Text = Threads + "";
            MainWindow = mainWindow;
        }
        private void ChooseDestination(object sender, RoutedEventArgs e)
        {
            OpenFolderDialog dialog = new();
            bool? success = dialog.ShowDialog();
            if (success == true) Destination = dialog.FolderName;
        }

        private void Start(object sender, RoutedEventArgs e)
        {
            if (currentThreads < Threads)
            {
                currentThreads++;
                CancellationTokenSource cts = new CancellationTokenSource();

                ThreadHandler th = new ThreadHandler() { Source = Source, Destination = Destination, Status = ThreadStatus.Running, Cts = cts };
                MainWindow.ActiveScene.ActiveThreads.Add(th);

                Thread thread = new(() => ActivateThread(th));
                thread.Start();
            }
            else MessageBox.Show("Too much of active threads");
        }
        private async void ActivateThread(ThreadHandler th)
        {
            await th.Start();
            currentThreads--;
            MainWindow.ArchiveScene.ArchivedThreads.Add(th);
            MainWindow.ActiveScene.ActiveThreads.Remove(th);
        }
        private void OnPropertyChanged(string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }
}
