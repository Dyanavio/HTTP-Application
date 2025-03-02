using HTTP_Application.Core;
using System.Collections.ObjectModel;
using System.Reflection.Metadata.Ecma335;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;

namespace HTTP_Application.Scenes
{
    public partial class ActiveScene : UserControl
    {
        private ObservableCollection<ThreadHandler> activeThreads;
        private readonly object locker = new();
        public MainWindow MainWindow { get; set; }
        public ObservableCollection<ThreadHandler> ActiveThreads
        {
            get => activeThreads; 
            set
            {
                activeThreads = value;
                BindingOperations.EnableCollectionSynchronization(activeThreads, locker);
            }
        }
        public ActiveScene(MainWindow mainWindow)
        {
            InitializeComponent();
            
            MainWindow = mainWindow;
            ActiveThreads = new ObservableCollection<ThreadHandler>();
            DataContext = this;
            ThreadList.DataContext = this;
            ThreadList.ItemsSource = ActiveThreads;
        }

        private void Pause(object sender, RoutedEventArgs e)
        {
            if (ThreadList.SelectedItems.Count == 0) return;
            foreach (var item in ThreadList.SelectedItems)
            {
                ThreadHandler handler = (ThreadHandler)item;
                if(handler == null || handler.Status == ThreadStatus.Stopped) continue;

                handler.ManualResetEvent.Reset();
                handler.Status = ThreadStatus.Stopped;
            }
        }
        private void Resume(object sender, RoutedEventArgs e)
        {
            if (ThreadList.SelectedItems.Count == 0) return;
            foreach(var item in ThreadList.SelectedItems)
            {
                ThreadHandler handler = (ThreadHandler)item;
                if (handler == null || handler.Status == ThreadStatus.Running) continue;

                handler.ManualResetEvent.Set();
                handler.Status = ThreadStatus.Running;
            }
        }
        private void Cancel(object sender, RoutedEventArgs e)
        {
            if (ThreadList.SelectedItems.Count == 0) return;
            var selected = new List<ThreadHandler>();
            foreach(var item in ThreadList.SelectedItems)
            {
                ThreadHandler handler = (ThreadHandler)item;
                if(handler == null) continue;

                handler.Cts.Cancel();
                handler.Status = ThreadStatus.Cancelled;
                selected.Add(handler);
            }
            foreach(var item in selected)
            {
                ActiveThreads.Remove(item);
            }
        }
    }
}
