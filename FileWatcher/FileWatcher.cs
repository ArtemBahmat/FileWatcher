using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;

namespace FileWatcher
{
    class FileWatcher
    {
        TaskFactory TFactory;
        public event Action OnChange;
        private object Locker; 
        private string FilePath { get; set; }
            
        public FileWatcher(string path, object locker)
        {
            TFactory = new TaskFactory();
            FilePath = path;
            Locker = locker;
        }

        public void OnChanged()
        {
            DateTime lastDt = File.GetLastWriteTime(FilePath);
            string text = string.Empty;
            int delay = 10000;
            int nullDigit = 0;
            int oneDigit = 1;

            while (true)
            {
                lock (Locker)
                {
                    text = File.ReadAllText(FilePath);
                }

                if (text == oneDigit.ToString())
                {
                    TFactory.StartNew(() =>
                    {
                        lock (Locker)
                        {
                            File.WriteAllText(FilePath, nullDigit.ToString());
                        }
                        Thread.Sleep(delay);
                        Console.WriteLine($"File was changed to {nullDigit} {delay} milliseconds ago");
                    });
                }

                DateTime currentDt = File.GetLastWriteTime(FilePath);
                if (currentDt != lastDt)
                {
                    OnChange();
                    Console.WriteLine("File was changed. New DateTime: " + currentDt.ToString("yyyy-MM-ddTHH:mm:ss.fff"));
                    lastDt = currentDt;
                }
            }
        }
    }
}
