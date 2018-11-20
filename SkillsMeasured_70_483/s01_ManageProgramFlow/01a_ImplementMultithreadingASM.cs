using System;
using System.Runtime.CompilerServices;
using System.Threading;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ManageProgramFlow
{
    /// <summary>
    /// Exercises for C# multithreading
    /// 
    /// AsyncronousProgrammingModel (ASM)
    /// Delegates are to handle execution and callbacks
    /// This model is presented in .NET 1.0
    /// 
    /// The parameters for the worker method can be given to the BeginInvoke call
    /// The parameters can be ref or out as well as normal value parameters
    /// The input parameters and the return type represented by the delegate
    /// 
    /// 
    /// Links:
    /// https://msdn.microsoft.com/en-us/library/2e08f6yc(v=vs.110).aspx
    /// 
    /// 
    /// Books:
    /// Mcsd-certification-toolkit-exam-70-483 - Chapter 7
    /// Exam Ref 70-483 Objective 1.1: Implement multithreading and asynchronous processing
    ///  
    /// </summary>
    [TestClass]
    public class ImplementMultithreadingAsm
    {
        #region Older implementations of asynchronous execution

        #region AsyncronousProgrammingModel (ASM)

        [TestMethod]
        public void AsyncronousProgrammingModel_HappyScenario()
        {
            //Arrange 
            var worker = new Helper01(PerformWorkHelper);


            var callbackOnEndOfWork = new AsyncCallback(EndCallback);

            Console.WriteLine("Before BeginInvoke");

            //Act

            //worker called with a bool param that makes it not to throw ex
            worker.BeginInvoke(false, callbackOnEndOfWork, worker);

            //Assert
            Console.WriteLine("After BeginInvoke");

            // The callback is made on a ThreadPool thread. ThreadPool threads
            // are background threads, which do not keep the application running
            // if the main thread ends. Comment out the next line to demonstrate
            // this.
            Thread.Sleep(1000);

        }


        [TestMethod]
        public void AsyncronousProgrammingModel_ExceptionInWorker()
        {
            //Arrange
            var worker = new Helper01(PerformWorkHelper);
            var callbackOnEndOfWork = new AsyncCallback(EndCallback);


            //Act

            //worker called with a bool param that makes it throw ex (catched in around EndInvoke)
            worker.BeginInvoke(true, callbackOnEndOfWork, worker);


            //Assert
            Thread.Sleep(1000);

        }


        [TestMethod]
        public void AsyncronousProgrammingModel_EndInvokeCalledWrong()
        {
            //Arrange
            var worker = new Helper01(PerformWorkHelper);
            var callbackOnEndOfWork = new AsyncCallback(EndCallback);


            //Act
            var result3 = worker.BeginInvoke(false, callbackOnEndOfWork, worker);

            //The thread waits for result3 to execute here.
            ((Helper01)result3.AsyncState).EndInvoke(result3);


            //Assert

            //There is no need for wait or sleep because the EndInvoke blocks until the worker finishes
            //If you do not want to block the main thread than call it from the callback

            //However we need to wait till the callback is accessed
            //An exception resulted on the callback because EndInvoke can be called only once for each async worker
            Thread.Sleep(1000);
        }


        [TestMethod]
        public void AsyncronousProgrammingModel_WaitOne()
        {
            //Arrange
            var worker = new Helper01(PerformWorkHelper);
            var callbackOnEndOfWork = new AsyncCallback(EndCallback);


            //Act

            //worker called with a bool param that makes it throw ex (catched in around EndInvoke)
            var result = worker.BeginInvoke(false, callbackOnEndOfWork, worker);

            //Do some other work in the main thread


            //Assert
            //Wait with the main thread until the worker finishes
            result.AsyncWaitHandle.WaitOne();

            // If you release all references to the wait handle, system resources are freed when garbage collection reclaims the wait handle. 
            // To free the system resources as soon as you are finished using the wait handle, 
            // dispose of it by calling the WaitHandle.Close method. 
            result.AsyncWaitHandle.Close();

            Assert.IsTrue(result.IsCompleted);

            //We still need to wait for the callback
            //It can be done with the help of an AutoResetEvent instance
            _endCallbackStopWaitHandle.WaitOne();
        }


        [TestMethod]
        [ExpectedException(typeof(ArgumentException), 
            "BeginInvoke throws ArgumentException if there are more than one methods in the delegates method set")]
        public void AsyncronousProgrammingModel_ExceptionInBeginInvoke()
        {
            //Arrange
            var workerWithException = new Helper01(PerformWorkHelper);
            workerWithException += PerformWorkHelper;

            var callbackOnEndOfWork = new AsyncCallback(EndCallback);

            //Act

            try
            {
                workerWithException.BeginInvoke(false, callbackOnEndOfWork, workerWithException);
            }
            catch (ArgumentException ex)
            {
                //Exceptions with BeginInvoke catched here
                //The asynchronously called delegates (MulticastDelegates) can only have one subscribed method

                Console.WriteLine(ex.Message);

                throw;
            }

            //Assert
            
        }

        #endregion

        #region Helpers
        
        private delegate int Helper01(bool throwEx);

        private int PerformWorkHelper(bool throwEx)
        {
            Console.WriteLine("PerformWorkHelper called");
            Thread.Sleep(500);

            if (throwEx)
            {
                Console.WriteLine("ex thrown from PerformWorkHelper");
                throw new Exception("Exception in PerformWorkHelper");
            }

            Console.WriteLine("PerformWorkHelper ended");
            return 0;
        }

        // The callback method must have the same signature as the
        // AsyncCallback delegate.
        private void EndCallback(IAsyncResult res)
        {
            Console.WriteLine("EndCallback called");

            var originallyCalledWorkerDelegate = (Helper01)res.AsyncState;

            try
            {
                // Call EndInvoke to retrieve the results if any.
                // When it is in the main thread than it waits for the task completion
                // EndInvoke can be called only once per 
                var result = originallyCalledWorkerDelegate.EndInvoke(res);
                Console.WriteLine("Resulted from PerformWorkHelper: {0}", result);
            }
            catch (Exception ex)
            {
                //you can catch exceptions of the worker process
                Console.WriteLine(ex.Message + " is Catched in EndCallback");
            }


            Console.WriteLine("EndCallback ended");

            _endCallbackStopWaitHandle.Set();
        }

        readonly AutoResetEvent _endCallbackStopWaitHandle = new AutoResetEvent(false);

        #endregion


        #endregion

    }
}
