using System;
using System.Threading;

namespace Arcond.Threading.Objects
{
    internal class Task
    {
        /// <summary>
        /// Created a new instance of Task with the specified Thread and Parameter
        /// </summary>
        /// <param name="spawn">System.Threading.Thread</param>
        /// <param name="parameter">object</param>
        public Task(Thread spawn, object parameter)
        {
            Spawn = spawn;
            Parameter = parameter;
        }

        /// <summary>
        /// Creates a new instance of Task with the specified Thread
        /// </summary>
        /// <param name="spawn">System.Threading.Thread</param>
        public Task(Thread spawn)
            : this(spawn, null)
        { }

        /// <summary>
        /// Gets the Thread
        /// </summary>
        public Thread Spawn { get; private set; }

        /// <summary>
        /// Gets the Parameter
        /// </summary>
        public object Parameter { get; private set; }

        /// <summary>
        /// Gets whether the Thread should use System.Threading.ThreadStart or System.Threading.ParameterizedThreadStart
        /// </summary>
        public bool IsParameterized
        {
            get { return Parameter != null; }
        }
    }
}
