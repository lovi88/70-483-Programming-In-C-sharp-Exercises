using System;
using System.Diagnostics;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ManageProgramFlow
{
    /// <summary>
    /// Exercises for C# multithreading
    /// 
    /// ThreadPool
    /// Because thread creation is a resource intensive job
    /// .Net manages a thread pool to reuse threads
    /// The thread pool internally has a Queue for jobs to do
    /// And it feeds the threads from that queue
    /// It analyses thread usage so it can create new threads (or kill old ones) if necessary
    /// 
    /// - You can’t abort or interrupt a thread from the thread pool.
    /// - You can’t join a thread from the thread pool.To achieve that, you must use other methods to synchronize
    /// - Threads from the thread pool are reused when they finish their work, whereas the normal threads are destroyed.
    /// - You can’t control the priority of a thread from the thread pool.
    /// 
    /// Because threads are being reused, they also reuse their local state.
    /// Queuing a work item to a thread pool can be useful, but it has its shortcomings. There is no
    /// built-in way to know when the operation has finished and what the return value is.
    /// 
    /// This why the TPL is a newer and more common way of multithreading nowadays.
    ///  
    /// Books:
    /// Mcsd-certification-toolkit-exam-70-483 - Chapter 7
    /// Exam Ref 70-483 Objective 1.1: Implement multithreading and asynchronous processing
    /// 
    /// </summary>
    [TestClass]
    public class ImplementMultithreadingThreadPool
    {

        [TestMethod]
        public void ThreadPoolBasic00()
        {
            //Act
            Debug.WriteLine($"Number of processor threads of the computer: {Environment.ProcessorCount}{Environment.NewLine}");

            Debug.WriteLine("WorkerThreads are for CPU bound calculations");
            Debug.WriteLine($"QueueUserWorkItem method is used for using worker threads{Environment.NewLine}");

            Debug.WriteLine("completionPortThreads are for I/O bound operations");
            Debug.WriteLine($"The UnsafeQueueNativeOverlapped method is used for overlapped async I/O operations {Environment.NewLine}");

            ShowThreadPoolStatistics();

            var newMinMaxThreadNum = 3;
            Debug.WriteLine($"{Environment.NewLine}Setting the Min and Max Thread number of the thread pool to: {newMinMaxThreadNum}{Environment.NewLine}");

            ThreadPool.SetMinThreads(newMinMaxThreadNum, newMinMaxThreadNum);
            ThreadPool.SetMaxThreads(newMinMaxThreadNum, newMinMaxThreadNum);

            ShowThreadPoolStatistics();
        }
        
        [TestMethod]
        public void ThreadPoolBasic01()
        {
            //Arrange
            const int testState = 55;

            //Act
            ThreadPool.QueueUserWorkItem(state =>
            {
                Debug.WriteLine($"Worker Thread from Thread pool, ThreadName: {Thread.CurrentThread.Name}" +
                                $" ThreadId: {Thread.CurrentThread.ManagedThreadId}" +
                                $" IsBackground: {Thread.CurrentThread.IsBackground}");

                Debug.WriteLine($"input state: {state}");

                _autoResetEvent.Set();
            }, testState);


            //Thread pool jobs are background threads and there is 
            //no common way to know when a job is finished so I used an AutoResetEvent signal
            Debug.WriteLine("main waits for thread pool job");
            var isJobFinished = _autoResetEvent.WaitOne(1000);

            //Assert
            Assert.IsTrue(isJobFinished);
        }

        readonly AutoResetEvent _autoResetEvent = new AutoResetEvent(false);

        private static void ShowThreadPoolStatistics()
        {
            int workerThreads;
            int completionPortThreads;

            ThreadPool.GetMinThreads(out workerThreads, out completionPortThreads);

            Debug.WriteLine($"minWorkerThreads: {workerThreads} completionPortThreads: {completionPortThreads}");

            ThreadPool.GetMaxThreads(out workerThreads, out completionPortThreads);

            Debug.WriteLine($"maxWorkerThreads: {workerThreads} completionPortThreads: {completionPortThreads}");

            ThreadPool.GetAvailableThreads(out workerThreads, out completionPortThreads);

            Debug.WriteLine($"avaibleWorkerThreads: {workerThreads} completionPortThreads: {completionPortThreads}");
        }
    }
}
