using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ManageProgramFlow
{
    /// <summary>
    /// Exercises for C# multithreading
    /// 
    /// Async/Await
    /// 
    /// Method has the async keyword
    /// When called use the await keyword 
    /// Without await there is a compiler warning and the method is called synchronously
    /// 
    /// Async method return type is void, Task or Task<T>
    /// The void methods cannot be awaited
    /// Creating void async method is only allowed for event handlers where it must be void.
    /// Only accepted void async methods are async event handlers
    /// 
    /// Ref and out parameters are not permitted
    /// 
    /// This model is presented in .NET 4.5 (It is an easier way to use the TPL)
    /// (In C# language 5.0)
    /// 
    /// Books:
    /// Mcsd-certification-toolkit-exam-70-483 - Chapter 7
    /// Exam Ref 70-483 Objective 1.1: Implement multithreading and asynchronous processing
    ///  
    /// </summary>
    [TestClass]
    public class AsyncAwait
    {
        [TestMethod]
        public void TestMethod1()
        {
            Console.WriteLine("call");
            FireAndForget();
            Console.WriteLine("after call");

        }




        private void FireAndForget()
        {
            Thread.Sleep(1000);
            Console.WriteLine("async");
        }

    }

    

}
