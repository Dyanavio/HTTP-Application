using HTTP_Application.Core;
using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows.Data;


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
    }
}
