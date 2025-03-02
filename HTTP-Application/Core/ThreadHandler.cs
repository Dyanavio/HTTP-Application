using System.IO;
using System.Net.Http;
using System.Windows;

namespace HTTP_Application.Core
{
    public class ThreadHandler
    {
        public string? Source { get; set; }
        public string Destination { get; set; }
        public ThreadStatus? Status { get; set; }
        public ManualResetEvent ManualResetEvent { get; set; }
        public CancellationTokenSource Cts { get; set; }
        private CancellationToken token;
        public ThreadHandler() 
        {
            ManualResetEvent = new(true);
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
                    byte[] buffer = await client.GetByteArrayAsync(Source, Cts.Token);
                    ManualResetEvent.WaitOne();

                    token.ThrowIfCancellationRequested();
                    await File.WriteAllBytesAsync(Destination, buffer, Cts.Token);
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
