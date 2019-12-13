using System;
using System.Threading;
namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World From Docker");
            System.Threading.Thread.Sleep(Timeout.Infinite);
        }
    }
}
