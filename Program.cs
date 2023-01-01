using System.Data;
using System.Diagnostics;
using System.Net;

namespace AlarmBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            List<int> times = new List<int>(5);

            times.Add(1000);
            times.Add(1000);
            times.Add(1000);

            foreach (var t in times)
            {
                TestSeveralTimer(t);
            }

            Console.ReadLine();
        }

        public static void TestSeveralTimer(int time)
        {
            System.Timers.Timer timer = new System.Timers.Timer();

            timer.Interval = time;
            timer.Elapsed += (sender, e) => Print();
        }

        private static void Print()
        {
            Console.Write("Hi");
        }
    }
}