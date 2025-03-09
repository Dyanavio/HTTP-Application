using System.ComponentModel;
using System.IO;
using System.Net.Http;
using System.Windows;

namespace HTTP_Application.Core
{
    public class ThreadHandler : INotifyPropertyChanged
    {
        private ThreadStatus? status;
        public string? Source { get; set; }
        public string Destination { get; set; }
        public ThreadStatus? Status
        {
            get => status;
            set
            {
                if(status != value)
                {
                    status = value;
                    OnPropertyChanged(nameof(Status));
                }
            }
        }
        public ManualResetEvent ManualResetEvent { get; set; }
        public CancellationTokenSource Cts { get; set; }
        private CancellationToken token;

        public event PropertyChangedEventHandler? PropertyChanged;

        public ThreadHandler() 
        {
            ManualResetEvent = new(true);
        }
        private void OnPropertyChanged(string? name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
        public async Task Start()
        {
            try
            {
                token = Cts.Token;
                using (HttpClient client = new HttpClient())
                {
                    MessageBox.Show(Destination);

                    token.ThrowIfCancellationRequested();
                    byte[] buffer = await client.GetByteArrayAsync(Source, token);
                    ManualResetEvent.WaitOne();

                    token.ThrowIfCancellationRequested();
                    await File.WriteAllBytesAsync(Destination, buffer, token);
                    ManualResetEvent.WaitOne();

                    await Task.Delay(10000);

                    Status = ThreadStatus.Completed;
                    MessageBox.Show("Download Complete");
                }
            }
            catch(Exception e)
            {
                MessageBox.Show(e.Message);
            }
            finally
            {
                Cts.Dispose();
            }
        }
    }
}
