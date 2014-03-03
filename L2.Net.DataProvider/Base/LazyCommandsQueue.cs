using System;
using System.Collections.Generic;
using System.Timers;

namespace L2.Net.DataProvider
{
    /// <summary>
    /// Current <see cref="LazyCommandsQueue&lt;C&gt;"/> event handler.
    /// </summary>
    /// <param name="queue"><see cref="Command"/> objects to be executed.</param>
    public delegate void LazyQueueDumpEventHandler<C>( C[] queue ) where C : Command, new();

    /// <summary>
    /// <see cref="LazyCommandsQueue&lt;C&gt;"/> interface.
    /// </summary>
    /// <typeparam name="C"><see cref="Command"/> objects to be executed on next dump event.</typeparam>
    public interface ILazyCommandsQueue<C> { }

    /// <summary>
    /// Represents lazy commands queue class.
    /// </summary>
    /// <typeparam name="C"><see cref="Command"/> objects to be executed on next dump event.</typeparam>
    public sealed class LazyCommandsQueue<C> : ILazyCommandsQueue<C> where C : Command, new()
    {
        /// <summary>
        /// Lock <see cref="object"/>.
        /// </summary>
        private readonly object m_Lock = new object();

        /// <summary>
        /// Main <see cref="Command"/> objects queue.
        /// </summary>
        private readonly Queue<C> m_Queue;

        /// <summary>
        /// Queue dump interval value.
        /// </summary>
        private TimeSpan m_DumpInterval;

        /// <summary>
        /// Dump <see cref="Timer"/>.
        /// </summary>
        private Timer m_DumpTimer;

        /// <summary>
        /// Raises when current <see cref="LazyCommandsQueue&lt;C&gt;"/> dumps all enqueued <see cref="Command"/> objects.
        /// </summary>
        internal event LazyQueueDumpEventHandler<C> DumpEventHandler;

        /// <summary>
        /// Initializes new instance of <see cref="LazyCommandsQueue&lt;C&gt;"/> class.
        /// </summary>
        /// <param name="dumpInterval">Queue dump interval.</param>
        public LazyCommandsQueue( TimeSpan dumpInterval )
        {
            DumpInterval = dumpInterval;
            m_Queue = new Queue<C>();
        }

        /// <summary>
        /// Enqueues <see cref="Command"/> object to current <see cref="LazyCommandsQueue&lt;C&gt;"/>.
        /// </summary>
        /// <param name="command"><see cref="Command"/> to enqueue.</param>
        public void Enqueue( C command )
        {
            lock ( m_Lock )
                m_Queue.Enqueue(command);
        }

        /// <summary>
        /// Dump <see cref="Timer"/> setup.
        /// </summary>
        private void SetupTimer()
        {
            if ( m_DumpTimer != null )
            {
                if ( m_DumpTimer.Enabled )
                {
                    DumpTimer_Elapsed(null, null);
                    m_DumpTimer.Stop();
                }

                m_DumpTimer.Dispose();
                m_DumpTimer = null;
            }

            m_DumpTimer = new Timer(m_DumpInterval.TotalMilliseconds);
            m_DumpTimer.Elapsed += new ElapsedEventHandler(DumpTimer_Elapsed);
            m_DumpTimer.Start();
        }

        /// <summary>
        /// Executes when dump timer raises Elapsed event.
        /// </summary>
        /// <param name="sender">Method caller.</param>
        /// <param name="e"><see cref="ElapsedEventArgs"/> arguments.</param>
        private void DumpTimer_Elapsed( object sender, ElapsedEventArgs e )
        {
            lock ( m_Lock )
            {
                m_DumpTimer.Stop();

                if ( DumpEventHandler != null )
                {
                    DumpEventHandler(m_Queue.ToArray());
                    m_Queue.Clear();
                }
                else
                    throw new InvalidOperationException("Failed to dump lazy cache queue, handler is not set.");

                m_DumpTimer.Start();
            }
        }

        /// <summary>
        /// Gets or sets <see cref="LazyCommandsQueue&lt;C&gt;"/> dump interval.
        /// </summary>
        public TimeSpan DumpInterval
        {
            get { return m_DumpInterval; }
            set
            {
                if ( m_DumpInterval != value )
                {
                    lock ( m_Lock )
                    {
                        m_DumpInterval = value;
                        SetupTimer();
                    }
                }
            }
        }
    }
}