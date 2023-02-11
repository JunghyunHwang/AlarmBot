using System.Data;
using System.Diagnostics;
using System.Net;

namespace AlarmBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bot.Start();
            bool bIsExit = true;

            while (bIsExit)
            {
                if (Console.ReadLine() == "exit")
                {
                    bIsExit = false;
                }
            }
        }
    }
}