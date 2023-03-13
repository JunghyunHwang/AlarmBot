using Org.BouncyCastle.Asn1.Sec;
using System.Data;
using System.Diagnostics;
using System.Net;

namespace AlarmBot
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Bot.On();
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