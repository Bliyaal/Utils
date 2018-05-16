using System;
using System.Diagnostics;
using System.Threading;

namespace Utils
{
	/// <summary>
	/// Provides a set of methods and properties that you can use to accurately measure the performances of a function.
	/// </summary>
	public class PerformanceTest
	{
		/// <summary>
		/// Handler to the <see cref="TestComplete"/> event
		/// </summary>
		/// <param name="executionTime"></param>
		public delegate void TestCompleteHandler(TimeSpan executionTime);

		/// <summary>
		/// Event raised when the executing function finished its job
		/// </summary>
		public event TestCompleteHandler TestComplete;

		/// <summary>
		/// Function to be tested
		/// </summary>
		private readonly Action _testFunction;

		/// <summary>
		/// Stopwatch used to calculate the execution time
		/// </summary>
		private Stopwatch _executionTimer;

		/// <summary>
		/// Indicating if the function should be executed in its own thread
		/// </summary>
		private readonly bool _threaded;

		/// <summary>
		/// Separate thread used to execute the function
		/// </summary>
		private readonly Thread _executionThread;

		/// <summary>
		/// The duration of the execution
		/// </summary>
		public TimeSpan ExecutionTime
		{
			get
			{
				if (_executionTimer == null)
				{
					throw new InvalidOperationException("The test was not executed.");
				}
				return _executionTimer.Elapsed;
			}
		}

		/// <summary>
		/// Creates a new test
		/// </summary>
		/// <param name="action">Function to be executed</param>
		public PerformanceTest(Action action)
			: this(action,
				   false)
		{ }

		/// <summary>
		/// Creates a new test
		/// </summary>
		/// <param name="action">Function to be executed</param>
		/// <param name="autoStart">Value indicating if the test should start immediately</param>
		public PerformanceTest(Action action,
							   bool autoStart)
			: this(action,
				   autoStart,
				   false)
		{ }

		/// <summary>
		/// Creates a new test
		/// </summary>
		/// <param name="action">Function to be executed</param>
		/// <param name="autoStart">Value indicating if the test should start immediately</param>
		/// <param name="separateThread">Value indicating if the function should be executed in its own thread</param>
		public PerformanceTest(Action action,
							   bool autoStart,
							   bool separateThread)
			: this(action,
				   autoStart,
				   separateThread,
				   ThreadPriority.Normal)
		{ }

		/// <summary>
		/// Creates a new test
		/// </summary>
		/// <param name="action">Function to be executed</param>
		/// <param name="autoStart">Value indicating if the test should start immediately</param>
		/// <param name="separateThread">Value indicating if the function should be executed in its own thread</param>
		/// <param name="priorityLevel">Separate execution thread priority level</param>
		public PerformanceTest(Action action,
							   bool autoStart,
							   bool separateThread,
							   ThreadPriority priorityLevel)
		{
			_testFunction = action ?? throw new ArgumentNullException(nameof(action));
			_threaded = separateThread;

			if (separateThread)
			{
				_executionThread = new Thread(RunTest)
				{
					IsBackground = true,
					Priority = priorityLevel
				};
			}
			if (autoStart)
			{
				Execute();
			}
		}

		/// <summary>
		/// Creates a new test and starts it immediately
		/// </summary>
		/// <param name="executionFunction">Function to be executed</param>
		/// <returns>The new performance test</returns>
		public static PerformanceTest ExecutetNew(Action executionFunction)
		{
			return ExecutetNew(executionFunction,
							   false);
		}

		/// <summary>
		/// Creates a new test and starts it immediately
		/// </summary>
		/// <param name="executionFunction">Function to be executed</param>
		/// <param name="separateThread">Value indicating if the function should be executed in its own thread</param>
		/// <returns>The new performance test</returns>
		public static PerformanceTest ExecutetNew(Action executionFunction,
												  bool separateThread)
		{
			return ExecutetNew(executionFunction,
							   separateThread,
							   ThreadPriority.Normal);
		}

		/// <summary>
		/// Creates a new test and starts it immediately
		/// </summary>
		/// <param name="executionFunction">Function to be executed</param>
		/// <param name="separateThread">Value indicating if the function should be executed in its own thread</param>
		/// <param name="priorityLevel">Separate execution thread priority level</param>
		/// <returns>The new performance test</returns>
		public static PerformanceTest ExecutetNew(Action executionFunction,
												  bool separateThread,
												  ThreadPriority priorityLevel)
		{
			return new PerformanceTest(executionFunction,
									   true,
									   separateThread,
									   priorityLevel);
		}

		/// <summary>
		/// Starts the execution
		/// </summary>
		public void Execute()
		{
			if (_threaded)
			{
				_executionThread.Start();
			}
			else
			{
				RunTest();
			}
		}

		/// <summary>
		/// Starts the stopwatch and execute the function.
		/// </summary>
		private void RunTest()
		{
			_executionTimer = Stopwatch.StartNew();
			_testFunction.Invoke();
			_executionTimer.Stop();

			TestComplete?.Invoke(_executionTimer.Elapsed);
		}
	}
}
