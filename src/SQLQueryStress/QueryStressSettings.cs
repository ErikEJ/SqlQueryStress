
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
        public bool CollectIoStats;

        /// <summary>
        ///     Collect time stats?
        /// </summary>
        [DataMember]
        public bool CollectTimeStats;

        /// <summary>
        ///     command timeout
        /// </summary>
        [DataMember]
        public int CommandTimeout;

        /// <summary>
        ///     Connection Timeout
        /// </summary>
        [DataMember]
        public int ConnectionTimeout;

        /// <summary>
        ///     Enable pooling?
        /// </summary>
        [DataMember]
        public bool EnableConnectionPooling;

        /// <summary>
        ///     Force the client to retrieve all data?
        /// </summary>
        [DataMember]
        public bool ForceDataRetrieval;

        /// <summary>
        ///     Cancel active SqlCommands on Cancel? (do not wait for completion)
        /// </summary>
        [DataMember]
        public bool KillQueriesOnCancel;

        /// <summary>
        ///     Connection info for the DB in which to run the test
        /// </summary>
        [DataMember]
        public ConnectionInfo MainDbConnectionInfo;

        /// <summary>
        ///     main query to test
        /// </summary>
        [DataMember]
        public string MainQuery;

        /// <summary>
        ///     Number of iterations to run per thread
        /// </summary>
        [DataMember]
        public int NumIterations;

        /// <summary>
        ///     Number of threads to test with
        /// </summary>
        [DataMember]
        public int NumThreads;

        /// <summary>
        /// Delay
        /// </summary>
        [DataMember]
        public int DelayBetweenQueries;

        /// <summary>
        ///     Connection info for the DB from which to get the paramaters
        /// </summary>
        [DataMember]
        public ConnectionInfo ParamDbConnectionInfo;

        /// <summary>
        ///     mapped parameters
        /// </summary>
        [DataMember]
        public Dictionary<string, string> ParamMappings;

        /// <summary>
        ///     query from which to take parameters
        /// </summary>
        [DataMember]
        public string ParamQuery;

        /// <summary>
        ///     Should the main db and param db share the same settings?
        ///     If so, use main db settings for the params
        /// </summary>
        [DataMember]
        public bool ShareDbSettings;

        public QueryStressSettings()
        {
            ShareDbSettings = true;
            MainQuery = "";
            ParamQuery = "";
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