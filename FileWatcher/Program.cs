using System;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace FileWatcher
{
    public delegate void EventHandler(string path, DateTime lastDt);

    class Program
    {
        static object Locker = new object();
        static TaskFactory TFactory = new TaskFactory();
        static string FilePath = @"D:\file.txt";

        static void Main(string[] args)
        {
            if (!File.Exists(FilePath))
            {
                File.Create(FilePath);
            }

            FileWatcher watcher = new FileWatcher(FilePath, Locker);
            TFactory.StartNew(ChangeDigit);
            watcher.OnChange += Changed;
            watcher.OnChanged();
        }

        static void Changed()
        {
            Console.WriteLine("File changed");
        }

        static void ChangeDigit()
        {
            while (true)
            {
                Console.WriteLine("Do you want to change file content to one? Y/N :");
                string input = Console.ReadLine();

                if (input == "y" || input == "Y")
                {
                    lock (Locker)
                    {
                        File.WriteAllText(FilePath, "1");
                    }
                }
                Thread.Sleep(1000);
            }
        }
    }
}
