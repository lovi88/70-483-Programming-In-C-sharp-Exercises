using System;
using System.Threading;

namespace Threading
{
    class Program
    {
        static object lockO = new object();

        static void Main(string[] args)
        {
            //Debugger.Break();

            //Console.WriteLine("Waiting for debugger to attach");
            //while (!Debugger.IsAttached)
            //{
            //    Thread.Sleep(100);
            //}
            //Console.WriteLine("Debugger attached");

            Console.ReadLine();

            for (int i = 0; i < 10; i++)
            {
                ThreadPool.QueueUserWorkItem(Say, "Pisti " + i);
            }


            Thread.Sleep(r.Next(50, 500));

            Console.WriteLine("Main Ended");
            Console.ReadKey();


            Console.WriteLine(ReqursiveAdd(5, 15));
            Console.ReadKey();
        }

        private static int _num;
        static int ReqursiveAdd(int a, int b)
        {
            Monitor.Enter(lockO);
            try
            {
                //Do work
            }
            finally
            {
                Monitor.Exit(lockO);
            }

            // The Monitor usage has an easier usage by lock keyword
            lock (lockO)
            {
                if (a == 0 && b == 0)
                    return _num;

                _num += a;
                ReqursiveAdd(b, 0);
            }
            
            return _num;
        }

        static Random r = new Random();
        static void Say(object message)
        {
            Thread.Sleep(r.Next(5, 100));
            Console.WriteLine(message);
        }
    }

}
