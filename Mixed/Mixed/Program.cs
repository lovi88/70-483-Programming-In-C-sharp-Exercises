using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mixed
{
    class Program
    {
        static void Main(string[] args)
        {
            ToRefPrint(1.5f);

            Reflection.Program.AssemblyPrinter();

            Parallels.Tasks.ErrorousTaskAndCleanupTask();

            Console.ReadLine();
        }

        private static void ToRefPrint(float amount)
        {
            object amRef = amount;

            int balance = (int)(float)amRef;
            Console.WriteLine(balance);
            
        }
        
    }
}
