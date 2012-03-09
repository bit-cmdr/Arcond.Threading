using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Arcond.Threading.Objects;

namespace Arcond.Threading
{
    public class Pool :IDisposable
    {
        private const int DEFAULT_MAX = 10;
        private const int ABSOLUTE_MAX = 1024;

        private readonly object _sync = new object();
        private volatile Queue<Task> _tasks;
        private volatile Dictionary<int, Thread> _threads;
        private volatile int _maxThreads;
        private volatile bool _isRunning;
        private bool _isDisposed;
        private Thread _taskMaster;
        private Thread _threader;
        ~Pool()
        {
            Dispose(false);
        }

        /// <summary>
        /// Creates a new instance of Pool with the specified maximum number of threads
        /// </summary>
        /// <param name="max">Maximum number of threads, must be a number between 1 and 1024</param>
        public Pool(int max)
        {
            SetMaximumThreads(max);
            Restart();
            SpinUpTasks();
            SpinUpThreads();
            _isDisposed = false;
        }

        /// <summary>
        /// Creates a new instance of Pool with the default number of threads [10]
        /// </summary>
        public Pool()
            : this(DEFAULT_MAX)
        { }

        /// <summary>
        /// Gets the number of running threads
        /// </summary>
        public int RunningThreads
        {
            get { return _threads.Count; }
        }

        /// <summary>
        /// Gets the number of free threads in the pool
        /// </summary>
        public int AvailableThreads
        {
            get { return _maxThreads - _threads.Count; }
        }

        /// <summary>
        /// Gets the maximum number of threads possible for the pool
        /// </summary>
        public int MaximumThreads
        {
            get { return _maxThreads; }
        }

        /// <summary>
        /// Sets the maximum number of threads possible for the pool
        /// </summary>
        /// <param name="max">Maximum number of threads, must be a number between 1 and 1024</param>
        public void SetMaximumThreads(int max)
        {
            lock (_sync) {
                if (max > 0) {
                    if (max <= ABSOLUTE_MAX) _maxThreads = max;
                    else _maxThreads = ABSOLUTE_MAX;
                } else {
                    _maxThreads = DEFAULT_MAX;
                }
            }
        }

        /// <summary>
        /// Kills all threads spawned by the pool that are still running and disposes of the objects
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Ensures that the pool is running
        /// </summary>
        public void Restart()
        {
            _isRunning = true;
        }

        /// <summary>
        /// Kills all threads spawned by the pool that are still running
        /// Use Restart() to re-use the pool without having to instantiate it again
        /// </summary>
        public void KillAll()
        {
            _isRunning = false;
            KillTasks();
            KillThreads();
            KillTaskMaster();
            KillThreader();
        }

        /// <summary>
        /// Adds a parameterized thread start with it's parameter to the queue
        /// </summary>
        /// <param name="start">void (object) target</param>
        /// <param name="parameter">object: parameter for thread to work on</param>
        public void Enqueue(ParameterizedThreadStart start, object parameter)
        {
            Enqueue(null, start, parameter);
        }

        /// <summary>
        /// Adds a thread start to the queue
        /// </summary>
        /// <param name="start">void () target</param>
        public void Enqueue(ThreadStart start)
        {
            Enqueue(start, null, null);
        }

        private void Enqueue(ThreadStart start, ParameterizedThreadStart paramStart, object parameter)
        {
            SpinUp();

            Thread thread;
            if (start != null) thread = new Thread(start);
            else thread = new Thread(paramStart);

            var task = new Task(thread, parameter);
            lock (_sync) {
                _tasks.Enqueue(task);
            }
        }

        private void Dispose(bool isDisposing)
        {
            if (!_isDisposed) {
                if (isDisposing) {
                    // Place holder for unmanaged objects
                }

                KillAll();
                _isDisposed = true;
            }
        }

        private void SpinUp()
        {
            SpinUpTasks();
            SpinUpThreads();
            SpinUpTaskMaster();
            SpinUpThreader();
        }

        private void SpinUpTasks()
        {
            if (_tasks == null) {
                lock (_sync) {
                    if (_tasks == null) _tasks = new Queue<Task>();
                }
            }
        }

        private void SpinUpThreads()
        {
            if (_threads == null) {
                lock (_sync) {
                    if (_threads == null) _threads = new Dictionary<int, Thread>();
                }
            }
        }

        private void SpinUpThreader()
        {
            if (_threader == null) {
                lock (_sync) {
                    if (_threader == null) _threader = new Thread(new ThreadStart(ManageThreads));
                }
            }

            if (_threader.ThreadState != ThreadState.Running
                && _threader.ThreadState != ThreadState.AbortRequested
                && _threader.ThreadState != ThreadState.WaitSleepJoin) {
                lock (_sync) {
                    if (_threader.ThreadState != ThreadState.Running
                        && _threader.ThreadState != ThreadState.AbortRequested
                        && _threader.ThreadState != ThreadState.WaitSleepJoin) {
                        try {
                            _threader.Start();
                        } catch (ThreadStateException) {
                            // Whoops, shouldn't have called that
                        } catch (OutOfMemoryException) {
                            KillAll();  // Shut down all threads cap'n she cannit take much more!
                        }
                    }
                }
            }
        }

        private void SpinUpTaskMaster()
        {
            if (_taskMaster == null) {
                lock (_sync) {
                    if (_taskMaster == null) _taskMaster = new Thread(new ThreadStart(ManageTasks));
                }
            }

            if (_taskMaster.ThreadState != ThreadState.Running
                && _taskMaster.ThreadState != ThreadState.AbortRequested
                && _taskMaster.ThreadState != ThreadState.WaitSleepJoin) {
                lock (_sync) {
                    if (_taskMaster.ThreadState != ThreadState.Running
                        && _taskMaster.ThreadState != ThreadState.AbortRequested
                        && _taskMaster.ThreadState != ThreadState.WaitSleepJoin) {
                        try {
                            _taskMaster.Start();
                        } catch (ThreadStateException) {
                            // Whoops, shouldn't have called that
                        } catch (OutOfMemoryException) {
                            KillAll();  // Shut down all threads cap'n she cannit take much more!
                        }
                    }
                }
            }
        }

        private void KillTasks()
        {
            if (_tasks != null && _tasks.Count > 0) {
                lock (_sync) {
                    if (_tasks != null && _tasks.Count > 0) _tasks.Clear();
                }
            }
        }

        private void KillThreads()
        {
            if (_threads != null && _threads.Count > 0) {
                lock (_sync) {
                    if (_threads != null && _threads.Count > 0) {
                        foreach (var thread in _threads.Values) {
                            thread.Interrupt();
                            thread.Abort();
                        }

                        _threads.Clear();
                    }
                }
            }
        }

        private void KillTaskMaster()
        {
            if (_taskMaster != null) {
                lock (_sync) {
                    if (_taskMaster != null) {
                        _taskMaster.Interrupt();
                        _taskMaster.Abort();
                        _taskMaster.Join();
                    }
                }
            }
        }

        private void KillThreader()
        {
            if (_threader != null) {
                lock (_sync) {
                    if (_threader != null) {
                        _threader.Interrupt();
                        _threader.Abort();
                        _threader.Join();
                    }
                }
            }
        }

        private void ManageTasks()
        {
            while (_isRunning) {
                if (_tasks != null && _tasks.Count > 0) {
                    bool canProcess = false;
                    if (_threads.Count < _maxThreads) {
                        lock (_sync) {
                            canProcess = _threads.Count < _maxThreads;
                        }
                    }

                    if (canProcess) {
                        Task current = null;
                        lock (_sync) {
                            if (_tasks != null && _tasks.Count > 0) current = _tasks.Dequeue();
                        }

                        if (current != null) {
                            if (current.IsParameterized) current.Spawn.Start(current.Parameter);
                            else current.Spawn.Start();

                            lock (_sync) {
                                if (_threads != null) _threads.Add(current.Spawn.ManagedThreadId, current.Spawn);
                            }
                        }
                    }
                }
            }
        }

        private void ManageThreads()
        {
            while (_isRunning) {
                if (_threads != null && _threads.Count > 0) {
                    IEnumerable<Thread> threads;
                    lock (_sync) {
                        threads = _threads.Values;
                    }

                    List<int> targets = new List<int>();
                    if (threads != null && threads.Count() > 0) {
                        targets.AddRange(
                            threads.Where(t => t.ThreadState == ThreadState.Aborted || t.ThreadState == ThreadState.Stopped)
                                .Select(t => t.ManagedThreadId));
                    }

                    if (targets.Count > 0) {
                        foreach (int id in targets) {
                            if (_threads.ContainsKey(id)) {
                                lock (_sync) {
                                    if (_threads.ContainsKey(id)) _threads.Remove(id);
                                }
                            }
                        }
                    }
                }
            }
        }
    }
}
