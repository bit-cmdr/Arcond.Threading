using System;
using System.Threading;
using System.Windows.Forms;
using Arcond.Threading;

namespace PoolProof
{
	public partial class Proof :Form
	{
		private static object _sync = new object();
		private int _max = 1;
		private bool _run = true;
		private Thread _mon;
		private Pool _pool;
		public Proof()
		{
			InitializeComponent();
		}

		private void Proof_Load(object sender, EventArgs e)
		{
			_pool = new Pool(_max);
			this.txtMax.Text = _max.ToString();
			_mon = new Thread(new ThreadStart(ThreadMon));
			_mon.Start();
		}

		private void Proof_FormClosing(object sender, FormClosingEventArgs e)
		{
			_run = false;
			_pool.Dispose();
			_mon.Abort();
		}

		private void btnThread_Click(object sender, EventArgs e)
		{
			int capacity;
			lock (_sync) {
				_max = int.Parse(this.txtMax.Text);
				capacity = int.Parse(this.txtSpin.Text);

				Enable(false);
				_pool.SetMaximumThreads(_max);
			}

			for (int i = 0; i < capacity; i++) {
				int current = i;
				_pool.Enqueue(new ParameterizedThreadStart(Worker), current);
			}
		}

		private void ThreadMon()
		{
			while (_run) {
				this.lblAvail.Invoke(new Action<int, int>(UpdateLabels), _pool.RunningThreads, _pool.AvailableThreads);
				this.btnThread.Invoke(new Action<bool>(Enable), _pool.AvailableThreads == _max);
			}
		}

		private void Enable(bool isEnabled)
		{
			this.txtMax.Enabled = isEnabled;
			this.txtSpin.Enabled = isEnabled;
			this.btnThread.Enabled = isEnabled;
		}

		private void UpdateLabels(int running, int avail)
		{
			this.lblAvailThreads.Text = avail.ToString();
			this.lblRunningThreads.Text = running.ToString();
		}

		private void Worker(object state)
		{
			int item = (int)state;
			string str = String.Format("Processing item [{0}] on Thread ID [{1}]\r\n", item, Thread.CurrentThread.ManagedThreadId);

			txtOutput.Invoke(new Action<string>(UpdateOutput), str);
			Thread.Sleep(5000);

			str = String.Format("Finished processing item [{0}] on Thread ID [{1}]\r\n", item, Thread.CurrentThread.ManagedThreadId);
			txtOutput.Invoke(new Action<string>(UpdateOutput), str);
		}

		private void UpdateOutput(string msg)
		{
			this.txtOutput.Text = String.Concat(this.txtOutput.Text, msg);
		}
	}
}
