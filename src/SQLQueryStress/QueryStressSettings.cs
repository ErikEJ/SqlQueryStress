
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;


namespace SQLQueryStress
{

    [Serializable]
    [DataContract]
    public class QueryStressSettings
    {
        /// <summary>
        ///     Collect I/O stats?
        /// </summary>
        [DataMember]
        public bool CollectIoStats { get; set; }

        /// <summary>
        ///     Collect time stats?
        /// </summary>
        [DataMember]
        public bool CollectTimeStats { get; set; }

        /// <summary>
        ///     command timeout
        /// </summary>
        [DataMember]
        public int CommandTimeout { get; set; }

        /// <summary>
        ///     Connection Timeout
        /// </summary>
        [DataMember]
        public int ConnectionTimeout { get; set; }

        /// <summary>
        ///     Enable pooling?
        /// </summary>
        [DataMember]
        public bool EnableConnectionPooling { get; set; }

        /// <summary>
        ///     Force the client to retrieve all data?
        /// </summary>
        [DataMember]
        public bool ForceDataRetrieval { get; set; }

        /// <summary>
        ///     Cancel active SqlCommands on Cancel? (do not wait for completion)
        /// </summary>
        [DataMember]
        public bool KillQueriesOnCancel { get; set; }

        /// <summary>
        ///     Connection info for the DB in which to run the test
        /// </summary>
        [DataMember]
        public ConnectionInfo MainDbConnectionInfo { get; set; }

        /// <summary>
        ///     main query to test
        /// </summary>
        [DataMember]
        public string MainQuery { get; set; }

        /// <summary>
        ///     Number of iterations to run per thread
        /// </summary>
        [DataMember]
        public int NumIterations { get; set; }

        /// <summary>
        ///     Number of threads to test with
        /// </summary>
        [DataMember]
        public int NumThreads { get; set; }

        /// <summary>
        /// Delay
        /// </summary>
        [DataMember]
        public int DelayBetweenQueries { get; set; }

        /// <summary>
        ///     Connection info for the DB from which to get the paramaters
        /// </summary>
        [DataMember]
        public ConnectionInfo ParamDbConnectionInfo { get; set; }

        /// <summary>
        ///     mapped parameters
        /// </summary>
        [DataMember]
        public Dictionary<string, string> ParamMappings { get; set; }

        /// <summary>
        ///     query from which to take parameters
        /// </summary>
        [DataMember]
        public string ParamQuery { get; set; }

        /// <summary>
        ///     Should the main db and param db share the same settings?
        ///     If so, use main db settings for the params
        /// </summary>
        [DataMember]
        public bool ShareDbSettings { get; set; }

        public QueryStressSettings()
        {
            ShareDbSettings = true;
            MainQuery = string.Empty;
            ParamQuery = string.Empty;
            NumThreads = 1;
            NumIterations = 1;
            ParamMappings = new Dictionary<string, string>();
            ConnectionTimeout = 15;
            CommandTimeout = 0;
            EnableConnectionPooling = true;
            CollectIoStats = true;
            CollectTimeStats = true;
            ForceDataRetrieval = false;
            KillQueriesOnCancel = true;
            MainDbConnectionInfo = new ConnectionInfo(ConnectionTimeout, EnableConnectionPooling, NumThreads * 2);
            ParamDbConnectionInfo = new ConnectionInfo();
        }

        [OnDeserialized]
        private void FixSettings(StreamingContext context)
        {
            ConnectionTimeout = ConnectionTimeout == 0 ? 15 : ConnectionTimeout;
        }
    }

}