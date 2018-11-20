using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Reflection
{
    public class Program
    {
        static void Main(string[] args)
        {

        }

        public static void AssemblyPrinter()
        {
            var executingAssembly = Assembly.GetExecutingAssembly();
            var callingAssembly = Assembly.GetCallingAssembly();
            var entryAssembly = Assembly.GetEntryAssembly();

            Console.WriteLine("executingAssembly: "+executingAssembly.FullName);
            Console.WriteLine("callingAssembly: "+callingAssembly.FullName);
            Console.WriteLine("entryAssembly: " + entryAssembly.FullName);

        }
    }
}
