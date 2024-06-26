﻿namespace AlarmBot
{
    internal class Program
    {
        public static readonly int A_DAY_MILLISECONDS = 1000 * 60 * 60 * 24;
        public static readonly int DEFAULT_LIST_COUNT = 64;

        static void Main(string[] args)
        {
            BrandManager.On();
            MessageManager.On();

            bool bRun = true;

            while (bRun)
            {
                if (Console.ReadLine() == "Stop")
                {
                    bRun = false;
                }
            }
        }
    }
}