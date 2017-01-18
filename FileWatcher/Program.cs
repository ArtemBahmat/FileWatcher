using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace FileWatcher
{
    public delegate void EventHandler(string path, DateTime lastDt);

    class Program
    {
        public static event EventHandler _watch;

        static void Main(string[] args)
        {
            string path = @"D:\file.txt";
            DateTime dt = File.GetLastWriteTime(path); //= new DateTime(1985, 4, 3);

            if (!File.Exists(path))
            {
                File.Create(path);
            }

            _watch += new EventHandler(OnChanged);
            _watch.Invoke(path, dt);
            Console.ReadLine();
        }

        static void OnChanged(string path, DateTime lastDt)
        {
            while (true)
            {
                DateTime currentDt = File.GetLastWriteTime(path);

                if (currentDt != lastDt)
                {
                    lastDt = currentDt;
                    Console.WriteLine("File was changed. New DateTime: " + currentDt.ToString("yyyy-MM-ddTHH:mm:ss.fff"));//.ToLongDateString());
                }
            }
        }
    }
}
