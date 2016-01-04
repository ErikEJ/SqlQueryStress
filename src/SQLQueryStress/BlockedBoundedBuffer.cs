using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace SQLQueryStress
{
    public class BlockedBoundedBuffer<T>
    {
        private readonly T[] buffer;

        private int readNumber;
        private int writeNumber;
        private int count;

        private BlockedBoundedBuffer()
        {
            //
        }

        public BlockedBoundedBuffer(int BufferSize)
        {
            this.buffer = new T[BufferSize];
            this.readNumber = -1;
            this.writeNumber = -1;
            this.count = 0;
        }

        public T Dequeue()
        {
            while (count == 0)
            {
                lock (this.buffer)
                {
                    Monitor.Wait(this.buffer);
                }
            }

            int read;

            do
            {
                read = Interlocked.Increment(ref readNumber);

                if (read >= buffer.Length)
                {
                    lock (this.buffer)
                    {
                        if (read >= buffer.Length)
                        {
                            Interlocked.Exchange(ref readNumber, 0);
                            Interlocked.Exchange(ref writeNumber, -1);
                            read = 0;
                            Monitor.PulseAll(this.buffer);
                        }
                    }
                }
                else
                    break;

            } while (true);

            Interlocked.Decrement(ref count);

            return (buffer[read]);
        }

        public void Enqueue(T input)
        {
            int write;

            do
            {
                write = Interlocked.Increment(ref writeNumber);
                if (write >= buffer.Length)
                {
                    lock (this.buffer)
                    {
                        Monitor.Wait(this.buffer);
                    }
                }
                else
                    break;
            }
            while (true);

            buffer[write] = input;

            if (Interlocked.Increment(ref count) == 1)
            {
                lock (this.buffer)
                {
                    Monitor.PulseAll(this.buffer);
                }
            }
        }
    }
}
