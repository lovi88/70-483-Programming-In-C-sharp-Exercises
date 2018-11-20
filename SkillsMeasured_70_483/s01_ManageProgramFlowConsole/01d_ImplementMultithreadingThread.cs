using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using ThreadState = System.Threading.ThreadState;

namespace ManageProgramFlowConsole
{
    static class ImplementMultithreadingThread
    {
        static void Main(string[] args)
        {
            //ThreadBasic01();
            //Thread_MainTheradDoesNotWaitForBackgroundTherad();
            //Thread_TheradMethodWithParameters();
            //Thread_MakeMainTheradToWaitForBackgroundTherad();
            //Thread_Stopping();
            //Thread_LocalData();
            //Thread_ThreadLocal();
            //Thread_NamedDataSlot();
            Thread_SuppressFlowDoesNotEffectCulture();
        }


        public static void ThreadBasic01()
        {
            Thread t = new Thread(MethodToCall);
            t.Start();

            for (int i = 0; i < 50; i++)
            {
                Debug.WriteLine("Main thread is working iteration: {0}", i);
                Thread.Sleep(0);
            }

            t.Join();
        }

        public static void Thread_MainTheradDoesNotWaitForBackgroundTherad()
        {
            //the ThreadStart wrapper is optional around MethodToCall2
            Thread t = new Thread(new ThreadStart(MethodToCall))
            {
                IsBackground = true
            };

            t.Start();

            Debug.WriteLine("Main have finished, there will be some output in the output window " +
                            "from MethodToCall " +
                            "but it will be terminated before it could finish");
        }

        public static void Thread_MakeMainTheradToWaitForBackgroundTherad()
        {
            //the ThreadStart wrapper is optional around MethodToCall2
            Thread t = new Thread(new ThreadStart(MethodToCall))
            {
                IsBackground = true
            };

            t.Start();

            // This line blocks the actual thread (sets the ThreadState.WaitSleepJoin).
            // If the other thread (t) is terminates before the timeout (1000ms) it returns true
            // If the other thread still runs after the timeout the Join returns false

            bool isJoinSuceeded = t.Join(1000);

            Thread tUnstarted = new Thread(MethodToCall);
            try
            {
                tUnstarted.Join();
            }
            catch (ThreadStateException)
            {
                // It throws ThreadStateException if it called in an unstarted thread
                Debug.WriteLine("ThreadStateException occured because Join called on a not started thread");
            }

        }

        private static void Thread_TheradMethodWithParameters()
        {
            //the ParameterizedThreadStart wrapper is optional around MethodToCall2
            Thread t = new Thread(new ParameterizedThreadStart(MethodToCall2));

            t.Start(5);
        }


        private static bool _isStopped = false;
        public static void Thread_Stopping()
        {
            //Not so cultural way or terminating a Thread is with Abort
            Thread t = new Thread(() =>
            {
                Thread.Sleep(1000);
            });


            t.Start();
            Thread.Sleep(500);
            t.Abort();
            Debug.WriteLine(t.ThreadState);



            //A more appreciated way to end a thread is by setting a helper flag.
            t = new Thread(() =>
            {
                while (!_isStopped)
                {
                    Debug.WriteLine("Running...");
                    Thread.Sleep(500);
                }
            });

            t.Start();
            Thread.Sleep(2000);
            _isStopped = true;


            //The terminated thread cannot be restarted and it work can be in an unknown state

            try
            {
                t.Start();
            }
            catch (ThreadStateException e)
            {
                Debug.WriteLine(e.Message);
            }

            Debug.WriteLine("ended");

        }


        [ThreadStatic]
        private static int _threadStatic;

        private static int _notThreadStatic;


        public static void Thread_LocalData()
        {
            _threadStatic = 0;
            _notThreadStatic = 0;

            Action a = delegate
            {
                for (int i = 0; i < 10; i++)
                {
                    _threadStatic++;

                    //not thread safe/atomic
                    _notThreadStatic++;

                    Debug.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId}; _threadStatic: {_threadStatic}; _notThreadStatic: {_notThreadStatic} ");
                    Thread.Sleep(0);
                }
            };

            var t1 = new Thread(new ThreadStart(a));
            var t2 = new Thread(new ThreadStart(a));

            t1.Start();
            t2.Start();

            //The maximum value of the _threadStatic will be 10
            //The maximum value of the _notThreadStatic will be 20
            //warning the ++ operator is not atomic so maybe it will not end up as 20 but less (More about that in ManageMultithreading)

            Debug.WriteLine("ended");
        }

        public static ThreadLocal<int> _threadLocal;
        public static void Thread_ThreadLocal()
        {
            _threadLocal = new ThreadLocal<int> { Value = 0 };

            Action a = () =>
            {
                _threadLocal.Value++;
                Debug.WriteLine($"ThreadId: {Thread.CurrentThread.ManagedThreadId}; _threadLocal.Value: {_threadLocal.Value}; ");
            };

            var t1 = new Thread(new ThreadStart(a));
            var t2 = new Thread(new ThreadStart(a));

            t1.Start();
            t2.Start();

            //both t1 and 12 has its own instance in the Value

            Debug.WriteLine("ended");
        }

        public static void Thread_NamedDataSlot()
        {
            //There is an other way to create data slots for threads

            Thread.AllocateNamedDataSlot("Lovi88");

            Action a2 = () =>
            {
                for (int i = 0; i < 5; i++)
                {
                    //setting the data in local storage
                    Thread.SetData(Thread.GetNamedDataSlot("Lovi88"), Thread.CurrentThread.Name);

                    Thread.Sleep(5);

                    //retrieving data
                    var data = Thread.GetData(Thread.GetNamedDataSlot("Lovi88"));
                    Debug.WriteLine($"ThreadName: {Thread.CurrentThread.Name} data: {data}");
                }


            };

            var t3 = new Thread((() => a2())) { Name = "t3" };
            var t4 = new Thread((() => a2())) { Name = "t4" };

            t3.Start();
            t4.Start();

            Debug.WriteLine("ended");
        }

        /// <summary>
        /// When a thread is created, the runtime ensures that the initiating thread’s execution context is flowed to the new thread.
        /// It consumes time so if there is no need for that information in the thread, than it can save execution some time in thread creation
        /// This property gives you access to properties like:
        /// - principal (representing the current security context)
        /// - priority (a value to indicate how the thread should be scheduled by the operating system)
        /// - call context 
        /// - synchronization context
        /// 
        /// - but the thread’s current culture is NOT part of it (CultureInfo associated with the current thread that is used to format dates, times, numbers, currency values, the sorting order of text, casing conventions, and string comparisons)
        /// </summary>
        public static void Thread_SuppressFlowDoesNotEffectCulture()
        {
            if (Thread.CurrentContext.ContextProperties != null)
            {
                foreach (var contextProperty in Thread.CurrentContext.ContextProperties)
                {
                    Debug.WriteLine(contextProperty);
                }
            }

            Action a = () =>
                {
                    Debug.WriteLine($"TreadName: {Thread.CurrentThread.Name}; Culture: {Thread.CurrentThread.CurrentCulture}");
                };

            //changing the thread default culture to help showing the difference.
            Thread.CurrentThread.CurrentCulture = Thread.CurrentThread.CurrentCulture.Name == "hu-HU"
                ? CultureInfo.CreateSpecificCulture("en-US")
                : CultureInfo.CreateSpecificCulture("hu-HU");
            Thread.CurrentThread.Name = "MainThread";

            
            //Showing the actual Culture of the MainThread
            a();

            //Same as the MainThread's
            var t1 = new Thread(() => a()) { Name = "Flow" };
            t1.Start();
            t1.Join();

            ExecutionContext.SuppressFlow();

            //Gets the default culture in the machine
            var tNoFlow = new Thread(() => a()) { Name = "NoFlow" };
            tNoFlow.Start();
            tNoFlow.Join();

            ExecutionContext.RestoreFlow();

            //Same as the MainThread's as well
            var flowAgain = new Thread(() => a()) { Name = "FlowAgain" };
            flowAgain.Start();
            flowAgain.Join();

            Debug.WriteLine("ended");
        }


        static void MethodToCall()
        {
            Debug.WriteLine("MethodToCall called");

            for (int i = 0; i < 50; i++)
            {
                Debug.WriteLine("MethodToCall working iteration: {0}", i);
                Thread.Sleep(0);
            }

            Debug.WriteLine("MethodToCall ended");
        }

        static void MethodToCall2(object num)
        {
            Debug.WriteLine($"MethodToCall called \nnum of iterations: {num}");

            for (int i = 0; i < (int)num; i++)
            {
                Debug.WriteLine("MethodToCall working iteration: {0}", i);
                Thread.Sleep(0);
            }

            Debug.WriteLine("MethodToCall ended");
        }
    }
}
