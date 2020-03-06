using System;

namespace SUnit.Runners
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                foreach (string arg in args)
                {
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.WriteLine(arg);
                    Runner runner = new Runner(arg);
                    runner.Run();
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                Console.WriteLine(ex.StackTrace);
            }

            Console.ReadLine();
        }
    }
}
