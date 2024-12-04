using System;

namespace SQLQueryStress;

public partial class LoadEngine
{
    public class QueryOutput
    {
        public DateTime startTime;
        public DateTime endTime;
        public Guid context;
        public int CpuTime;
        public Exception E;
        public int ElapsedTime;
        public bool Finished;
        public int LogicalReads;
        public TimeSpan Time;
        public int ThreadNumber;
        // Remaining active threads for the load
        public int ActiveThreads;

    }
}