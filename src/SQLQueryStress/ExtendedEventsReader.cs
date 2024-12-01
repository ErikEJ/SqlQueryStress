using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.XEvent.XELite;

namespace SQLQueryStress
{
    public class ExtendedEventsReader : IDisposable
    {
        private readonly string _connectionString;
        private readonly string _sessionName;
        private XELiveEventStreamer _reader;
        private bool _isDisposed;
        private CancellationTokenSource _cts;

        public event EventHandler<XEventData> OnEventReceived;

        private readonly ConcurrentDictionary<Guid, List<IXEvent>> _events = new();


        static Guid ConvertByteArrayToGuid(byte[] Hex)
        {
            return new Guid(Hex);
        }
        private void addEventToDictionary(IXEvent exEvent)
        {
            if (!exEvent.Actions.TryGetValue("context_info", out object context)) return;

            var contextS = ConvertByteArrayToGuid((byte[])context);
            var eventList = _events.AddOrUpdate(contextS, (a) => new(), (a,b) => { return b; });
            eventList.Add(exEvent);



        }

        public ExtendedEventsReader(string connectionString)
        {
            _connectionString = connectionString;
            _sessionName = $"SQLQueryStress_{DateTime.Now:yyyyMMddHHmmss}";
            _cts = new CancellationTokenSource();
        }

        public async  Task StartSession()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();

                // Create XE session
                var createSessionSql = $@"
                IF EXISTS (SELECT * FROM sys.server_event_sessions WHERE name = '{_sessionName}')
                    DROP EVENT SESSION [{_sessionName}] ON SERVER;

                CREATE EVENT SESSION [{_sessionName}] ON SERVER 
                ADD EVENT sqlserver.sql_batch_completed(
                    ACTION(sqlserver.database_name,
                           sqlserver.sql_text,
                           sqlserver.username,
                           sqlserver.context_info)
                    WHERE ([package0].[equal_boolean]([sqlserver].[is_system],(0))))
                ADD TARGET package0.event_file(SET filename=N'SQLQueryStress.xel')
                WITH (MAX_MEMORY=4096 KB,
                      EVENT_RETENTION_MODE=ALLOW_SINGLE_EVENT_LOSS,
                      MAX_DISPATCH_LATENCY=1 SECONDS,
                      MAX_EVENT_SIZE=0 KB,
                      MEMORY_PARTITION_MODE=NONE,
                      TRACK_CAUSALITY=OFF,
                      STARTUP_STATE=OFF);

                ALTER EVENT SESSION [{_sessionName}] ON SERVER STATE = START;";

                using (var cmd = new SqlCommand(createSessionSql, conn))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }

            // Initialize XEvent reader
            _reader = new XELiveEventStreamer(_connectionString, _sessionName);

           
        }

        public async Task StopSession()
        {
           // _cts.Cancel();

            using (var conn = new SqlConnection(_connectionString))
            {
                await conn.OpenAsync();
                var dropSessionSql = $@"
                IF EXISTS (SELECT * FROM sys.server_event_sessions WHERE name = '{_sessionName}')
                BEGIN
                    ALTER EVENT SESSION [{_sessionName}] ON SERVER STATE = STOP;
                    DROP EVENT SESSION [{_sessionName}] ON SERVER;
                END";

                using (var cmd = new SqlCommand(dropSessionSql, conn))
                {
                    await cmd.ExecuteNonQueryAsync();
                }
            }
        }

        public async Task ReadEventsLoop()
        {
            try
            {
                while (!_cts.Token.IsCancellationRequested)
                {
                    Task readTask = _reader.ReadEventStream(() =>
                    {
                        Debug.WriteLine("Connected to session");
                        return Task.CompletedTask;
                    },
                       xevent =>
                       {
                           addEventToDictionary(xevent);
                           return Task.CompletedTask;
                       },
                       _cts.Token);

                    await readTask;

                }
            }
            catch (OperationCanceledException)
            {
                // Normal cancellation, ignore
            }
            catch (Exception ex)
            {
                // Log or handle error
                System.Diagnostics.Debug.WriteLine($"Error in XEvent reader: {ex}");
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_isDisposed)
            {
                if (disposing)
                {
                    _cts?.Cancel();
                    _cts?.Dispose();
                   // _reader?.Dispose();
                    Task.Run(() => StopSession()).Wait();
                }
                _isDisposed = true;
            }
        }
    }

    public class XEventData
    {
        public string EventName { get; set; }
        public DateTime Timestamp { get; set; }
        public TimeSpan Duration { get; set; }
        public string DatabaseName { get; set; }
        public string SqlText { get; set; }
        public string Username { get; set; }
    }
} 