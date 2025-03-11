using HTTP_Application.Core;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;
using System.IO;
using System.Windows;
using Microsoft.Win32;


namespace HTTP_Application.Scenes
{
    public partial class ArchiveScene : UserControl
    {
        private ObservableCollection<ThreadHandler> archivedThreads;
        private readonly object locker = new();
        public MainWindow MainWindow { get; set; }
        public ObservableCollection<ThreadHandler> ArchivedThreads
        {
            get => archivedThreads;
            set
            {
                archivedThreads = value;
                BindingOperations.EnableCollectionSynchronization(archivedThreads, locker);
            }
        }
        public ArchiveScene(MainWindow mainWindow)
        {
            InitializeComponent();
            MainWindow = mainWindow;
            ArchivedThreads = new ObservableCollection<ThreadHandler>();
            DataContext = this;
        }

        private void DeleteFile(object sender, RoutedEventArgs e)
        {
            if (ThreadList.SelectedItems.Count == 0) return;
            List<ThreadHandler> toDelete = new();
            foreach(var item in ThreadList.SelectedItems)
            {
                ThreadHandler handler = (ThreadHandler)item;
                if (handler.Status == ThreadStatus.Cancelled) continue;
                toDelete.Add(handler);
            }
            foreach(var item in toDelete)
            {
                if(File.Exists(item.Destination))
                {
                    File.Delete(item.Destination);
                    archivedThreads.Remove(item);
                }
            }
        }
        private void MoveFile(object sender, RoutedEventArgs e)
        {
            if (ThreadList.SelectedItems.Count == 0) return;
            try
            {
                OpenFolderDialog dialog = new();
                bool? success = dialog.ShowDialog();
                string moveTo = "";
                if (success == true) moveTo = dialog.FolderName;
                else return;
                
                foreach (var item in ThreadList.SelectedItems)
                {
                    ThreadHandler handler = (ThreadHandler)item;
                    File.Move(handler.Destination, moveTo + "\\" + Path.GetFileName(handler.Destination));
                    handler.Destination = moveTo + "\\" + Path.GetFileName(handler.Destination);
                }
            }
            catch(Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
    }
}
