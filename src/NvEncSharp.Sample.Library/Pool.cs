using System;
using System.Collections.Generic;
using System.Threading;

namespace Lennox.NvEncSharp.Sample.Library
{
    internal class Pool<T>
        where T : class
    {
        private readonly int _max;
        private readonly object _lock = new object();
        private readonly List<T> _pool = new List<T>();

        public Pool(int max)
        {
            _max = max;
        }

        public T Get()
        {
            lock (_lock)
            {
                if (_pool.Count == 0) return null;

                var last = _pool[^1];
                _pool.RemoveAt(_pool.Count - 1);
                return last;
            }
        }

        public void Free(ref T obj)
        {
            obj = Interlocked.Exchange(ref obj, null);

            if (obj == null) return;

            lock (_lock)
            {
                // If over max, remove oldest to account for buffer size
                // changes.
                if (_pool.Count > _max)
                {
                    Remove(_pool[0]);
                    _pool.RemoveAt(0);
                }

                _pool.Add(obj);
            }
        }

        public PoolLease<T> Lease(T obj)
        {
            return new PoolLease<T>(obj, this);
        }

        public struct PoolLease<T2> : IDisposable
            where T2 : class
        {
            public T2 Value;

            private readonly Pool<T2> _pool;

            internal PoolLease(T2 value, Pool<T2> pool)
            {
                Value = value;
                _pool = pool;
            }

            public void Dispose()
            {
                _pool.Free(ref Value);
            }
        }

        private static void Remove(T obj)
        {
            if (obj is IDisposable disp)
            {
                disp.Dispose();
            }
        }
    }
}