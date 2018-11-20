using System;
using System.ComponentModel;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace ManageProgramFlow
{
    [TestClass]
    public class EventBasedAsyncronousPattern
    {
        /// <summary>
        /// Exercises for C# multithreading
        /// 
        /// EventBasedAsyncronousPattern
        /// 
        /// Remarkable implementation: BackgroundWorker
        /// 
        /// This model is presented in .NET 2.0
        /// 
        /// Books:
        /// Mcsd-certification-toolkit-exam-70-483 - Chapter 7
        /// Exam Ref 70-483 Objective 1.1: Implement multithreading and asynchronous processing
        ///  
        /// </summary>
        [TestMethod]
        public void EventBasedAsyncronousProgrammingModel_HappyScenaryo()
        {
            //Arrange 
            var bgWorker = new BackgroundWorker();
            bgWorker.RunWorkerCompleted += BgWorker_RunWorkerCompleted;
            bgWorker.DoWork += DoWork;

            //Act
            bgWorker.RunWorkerAsync();

            //Assert
            
        }

        private void DoWork(object sender, DoWorkEventArgs e)
        {
            Console.WriteLine("DoWork");
        }

        private void BgWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            Console.WriteLine("RunWorkerCompleted");
        }
    }
}
