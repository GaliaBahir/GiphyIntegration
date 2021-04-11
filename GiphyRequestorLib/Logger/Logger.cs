using System;
using System.Collections.Concurrent;
using System.IO;
using System.Threading;

namespace GiphyRequestorLib.Logger
{
    public class Logger : IDisposable
	{
		private static readonly object _loggerInstanceLock = new object();
		private static Logger _instance = null;
		private ConcurrentQueue<string> _logWritesQueue;
		private Thread _workingThread;
		private bool _disposed = false;
		public static Logger Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (_loggerInstanceLock)
					{
						if (_instance == null)
						{
							_instance = new Logger();
						}
					}
				}

				return _instance;
			}
		}

		private Logger()
		{
			_logWritesQueue = new ConcurrentQueue<string>();
			_workingThread = new Thread(new ParameterizedThreadStart(ProcessQueue))
			{
				IsBackground = true
			};
			_workingThread.Start();
		}


		private void ProcessQueue(object obj)
		{
			string message = string.Empty;
			while (true)
			{
				if (_logWritesQueue.TryDequeue(out message))
				{
					WriteEntireLog(message);
				}

				Thread.Sleep(70);
			}
		}

		private void AddLogToQueue(string message)
		{
			_logWritesQueue.Enqueue(message);
			Thread.Sleep(70);
		}


		public void WriteToLog(string message)
		{
			if (string.IsNullOrEmpty(message))
				return;

			AddLogToQueue(message);
		}


		public void WriteEntireLog(string message)
		{
			if (string.IsNullOrEmpty(message))
				return;

			string docPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

			// Write the string array to a new file named "WriteLines.txt".
			DateTime date = DateTime.Now;
			string fileName = $"Log_{date.Day}_{date.Month}_{date.Year}.txt";
			using (StreamWriter outputFile = new StreamWriter(Path.Combine(docPath, fileName), true))
			{
				outputFile.WriteLine(message);
			}

		}

		public void Dispose() => Dispose(true);

		protected virtual void Dispose(bool disposing)
		{
			if (_disposed)
			{
				return;
			}

			if (disposing)
			{
				// for disposing resources
				_workingThread?.Abort();
			}

			_disposed = true;
		}

	}
}
