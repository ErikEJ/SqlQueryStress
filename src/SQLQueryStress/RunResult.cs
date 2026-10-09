using System;
using System.Runtime.Serialization;

namespace SQLQueryStress
{
    [Serializable]
    [DataContract]
    public sealed record RunResult
    {
        [DataMember(Order = 1)]
        public Guid TestId { get; init; }

        [DataMember(Order = 2)]
        public string StartTime { get; init; } = string.Empty;

        [DataMember(Order = 3)]
        public double ElapsedTime { get; init; }

        [DataMember(Order = 4)]
        public int Threads { get; init; }

        [DataMember(Order = 5)]
        public int Iterations { get; init; }

        [DataMember(Order = 6)]
        public int CompletedIterations { get; init; }

        [DataMember(Order = 7)]
        public int Delay { get; init; }

        [DataMember(Order = 8)]
        public double AvgCpuSeconds { get; init; }

        [DataMember(Order = 9)]
        public double AvgActualSeconds { get; init; }

        [DataMember(Order = 10)]
        public double AvgClientSeconds { get; init; }

        [DataMember(Order = 11)]
        public double AvgLogicalReads { get; init; }

        [DataMember(Order = 12)]
        public int ExceptionCount { get; init; }
    }
}
