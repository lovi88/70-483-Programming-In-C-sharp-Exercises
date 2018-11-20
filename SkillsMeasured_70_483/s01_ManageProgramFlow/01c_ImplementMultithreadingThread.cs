using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ManageProgramFlow
{
    /// <summary>
    /// Exercises for C# multithreading
    /// 
    /// Thread class
    /// The root of threading in C#
    /// It is the most configurable but the most complex to use
    /// Nowadays the TPL is more commonly used but the TPL uses thread pool witch uses Thread internally.
    /// 
    /// The s01_ManageProgramFlowConsole Console application contains more exercises because of the 
    /// inconsistent results in the TestMethods
    /// 
    /// Books:
    /// Mcsd-certification-toolkit-exam-70-483 - Chapter 7
    /// Exam Ref 70-483 Objective 1.1: Implement multithreading and asynchronous processing
    /// 
    /// </summary>
    [TestClass]
    public class ImplementMultithreadingThread
    {
        [TestMethod]
        public void ThreadBasic01()
        {
            Thread t = new Thread(MethodToCall);
            t.Start();

            for (int i = 0; i < 50; i++)
            {
                Debug.WriteLine("Main thread is working iteration: {0}",i);
                Thread.Sleep(0);
            }

            t.Join();
        }

        /// <summary>
        /// test works differently than a real application, 
        /// see the real results in s01_ManageProgramFlowConsole
        /// </summary>
        [TestMethod]
        public void Thread_MainTheradDoesNotWaitForBackgroundTherad()
        {
            Thread t = new Thread(MethodToCall)
            {
                IsBackground = true
            };
            t.Start();

            Debug.WriteLine("Main have finished, there will be some output in the output window from MethodToCall but it will be terminated before it's end");
        }


        void MethodToCall()
        {
            Console.WriteLine("MethodToCall called");

            for (int i = 0; i < 50; i++)
            {
                Console.WriteLine("MethodToCall working iteration: {0}", i);
                Thread.Sleep(0);
            }

            Console.WriteLine("MethodToCall ended");
        }

        void MethodToCall2(int num)
        {
            Console.WriteLine($"MethodToCall called num of iterations: {num}");

            for (int i = 0; i < num; i++)
            {
                Console.WriteLine("MethodToCall working iteration: {0}", i);
                Thread.Sleep(0);
            }

            Console.WriteLine("MethodToCall ended");
        }
    }
}
