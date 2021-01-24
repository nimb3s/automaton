using System;
using System.Reflection;
using System.Threading.Tasks;

namespace Nimb3s.Automaton.Samples.CreateJobConsoleApp
{
    class Program
    {
        static async Task Main()
        {
            var assemblyName = Assembly.GetExecutingAssembly().GetName().Name;

            Console.Title = assemblyName;

            Console.WriteLine("Press any key to exit");
            Console.ReadKey();
        }
    }
}
