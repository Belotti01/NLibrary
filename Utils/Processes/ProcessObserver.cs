#nullable enable

using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace NL.Utils.Processes {
	public class ProcessObserver : IDisposable {
		public Process Process { get; protected init; }
		protected Queue<string> _outputStream;
		protected Queue<string> _errorStream;
		protected bool _disposedValue;

		public string[] Output => _outputStream.ToArray();
		public string[] Error => _errorStream.ToArray();

		public bool IsRunning => !Process.HasExited;
		public bool CanReadOutput => IsRunning && Process.StartInfo.RedirectStandardOutput;
		public bool CanReadError => IsRunning && Process.StartInfo.RedirectStandardError;

		public event DataReceivedEventHandler? OutputReceived;
		public event DataReceivedEventHandler? ErrorReceived;


		public static implicit operator ProcessObserver(Process proc)
			=> new(proc);


		public ProcessObserver(Process proc) {
			Process = proc;
			Process.BeginOutputReadLine();
			Process.BeginErrorReadLine();
			Process.EnableRaisingEvents = true;
			Process.OutputDataReceived += Process_OutputDataReceived;
			Process.ErrorDataReceived += Process_ErrorDataReceived;
			_outputStream = new();
			_errorStream = new();
		}

		/// <inheritdoc cref="Process.Start()"/>
		public bool StartProcess() {
			return Process.Start();
		}

		/// <summary>
		///     Delete all lines read up until now for both the output and error streams. 
		/// </summary>
		public void Flush() {
			_outputStream.Clear();
			_errorStream.Clear();
		}


		#region EventHandlers
		private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e) {
			if(e.Data is not null) {
				_outputStream.Enqueue(e.Data);
				var handler = OutputReceived;
				if(handler is not null) {
					handler(sender, e);
				}
			}
		}

		private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e) {
			if(e.Data is not null) {
				_errorStream.Enqueue(e.Data);
				var handler = ErrorReceived;
				if(handler is not null) {
					handler(sender, e);
				}
			}
		}
		#endregion


		#region IDisposable
		protected virtual void Dispose(bool disposing) {
			if(!_disposedValue) {
				if(disposing) {
					Process.Dispose();
					Flush();
				}

				_disposedValue = true;
			}
		}

		public void Dispose() {
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion

	}
}
