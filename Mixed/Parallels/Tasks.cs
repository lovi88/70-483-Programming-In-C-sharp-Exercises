using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Parallels
{
    public static class Tasks
    {
        public static void ErrorousTaskAndCleanupTask()
        {
            var t = Task.Run(() => ErrorousTask());
            t.ContinueWith(task => CleanupTask(), TaskContinuationOptions.OnlyOnFaulted);
        }

        private static void ErrorousTask()
        {
            Console.WriteLine("ErrorousTask");
            throw new Exception("ErrorousTask");
        }

        private static void CleanupTask()
        {
            Console.WriteLine("CleanupTask");

        }
    }
}
