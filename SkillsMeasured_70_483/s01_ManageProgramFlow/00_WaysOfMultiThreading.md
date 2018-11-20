# Ways of multithreading
## The two pillars of threading in c#
- The Thread class
  + The root of threading in C# 
  + It is the most configurable but the most complex to use
- The ThreadPool class (it is based on the Thread)
  + Because thread creation is a resource intensive job .Net manages a thread pool to reuse threads

## Techniques based on the two
- [The overview of the different techniques](https://msdn.microsoft.com/en-us/library/jj152938(v=vs.110).aspx)
- Asynchronous Pattern Model (APM)
  + The APM counterpart of this method would expose the BeginRead and EndRead methods
  + a worker delegate to call
  + AsyncCallback is called after the worker ends
  + BeginInvoke starts the worker
  + EndInvoke gets the result
- Event-based Asynchronous Pattern (EAP)
  + The EAP counterpart would expose the following set of types and members:
    * void ReadAsync
    * event ReadCompletedEventHandler
  + BackgroundWorker is one of it's usages (is an abstraction over ThreadPool)
    * If you started the background worker from the UI thread than 
      the completion event will be run in the UI thread. (In case of WPF or WinForms)
    * it is for avoiding InvalidOperationException which is 
      raised when you want to update a control not from the UI thread
    * Solution in Windows Forms
      - To check if you are not in the UI class there is the InvokeRequired property of the Control class
      - The Invoke method of the Control schedules work in the UI thread
    * Solution in WPF
      - Dispatcher.Invoke(()=> RefreshUI)
- Task-based Asynchronous Pattern Model (TAP)
  + Task<int> ReadAsync
  + Uses a single method to represent the initiation and completion of an asynchronous operation.
  + C# implementations of the model:
    * Task Parallel Library (TPL)
    * async/await keywords

## Classes and its most used methods
- Thread
  + Main Static methods:
    * Thread.CurrentThread.CurrentCulture
    * Thread.CurrentThread.Name
    * Thread.Sleep
  + Main non-static methods
    * thread.Start
    * thread.Join
- BackgroundWorker
  + Methods
    * RunWorkerAsync
    * ReportProgress
    * CancelAsync
  + Events
    * DoWork - place of the long running async operation
    * ProgressChanged
    * RunWorkerCompleted - When the operation is done (Eider with success or cancellation or unhandled exception)