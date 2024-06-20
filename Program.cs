namespace AlarmBot
{
    internal class Program
    {
        public static readonly int A_DAY_MILLISECONDS = 86400000;
        public static readonly int DEFAULT_LIST_COUNT = 64;

        static void Main(string[] args)
        {
            BrandManager.On();
            MessageManager.On();

            Console.ReadLine();
        }
    }
}