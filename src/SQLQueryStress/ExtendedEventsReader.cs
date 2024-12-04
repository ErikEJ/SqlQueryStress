using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.SqlServer.XEvent.XELite;

namespace SQLQueryStress;

public class ExtendedEventsReader : IDisposable
{
    private readonly string _connectionString;
    private readonly ConcurrentDictionary<Guid, List<IXEvent>> _events;
    private readonly string _sessionName;
    private readonly CancellationToken _cancellationToken;
    private bool _isDisposed;
    private XELiveEventStreamer _reader;

    public ExtendedEventsReader(string connectionString, CancellationToken cancellationToken,
        ConcurrentDictionary<Guid, List<IXEvent>> events)
    {
        _connectionString = connectionString;
        _sessionName = $"SQLQueryStress_{DateTime.Now:yyyyMMddHHmmss}";
        _cancellationToken = cancellationToken;
        _events = events;
    }

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private static Guid ConvertByteArrayToGuid(byte[] Hex)
    {
        if (Hex.Length == 0) return Guid.Empty;
        return new Guid(Hex);
    }

    private void addEventToDictionary(IXEvent exEvent)
    {
        if (!exEvent.Actions.TryGetValue("context_info", out var context)) return;

        var contextS = ConvertByteArrayToGuid((byte[])context);
        var eventList = _events.AddOrUpdate(contextS, a => new List<IXEvent>(), (a, b) => { return b; });
        eventList.Add(exEvent);
    }

    public async Task StartSession()
    {
        using (var conn = new SqlConnection(_connectionString))
        {
            await conn.OpenAsync();

            // Create XE session
            var createSessionSql = $@"
                IF EXISTS (SELECT * FROM sys.server_event_sessions WHERE name = '{_sessionName}')
                    DROP EVENT SESSION [{_sessionName}] ON SERVER;

                CREATE EVENT SESSION [{_sessionName}] ON SERVER
ADD EVENT sqlos.wait_info(
    ACTION(sqlserver.context_info,sqlserver.session_id,sqlserver.transaction_id)
    WHERE ([package0].[equal_boolean]([sqlserver].[is_system],(0)) AND [wait_type]<>'SOS_WORK_DISPATCHER')),
ADD EVENT sqlserver.blocked_process_report(
    ACTION(sqlserver.context_info,sqlserver.session_id,sqlserver.sql_text,sqlserver.transaction_id)),
ADD EVENT sqlserver.lock_cancel(
    ACTION(sqlserver.client_app_name,sqlserver.client_pid,sqlserver.context_info,sqlserver.database_name,sqlserver.nt_username,sqlserver.server_principal_name,sqlserver.session_id,sqlserver.sql_text,sqlserver.transaction_id)),
ADD EVENT sqlserver.lock_deadlock(
    ACTION(sqlserver.client_app_name,sqlserver.client_pid,sqlserver.context_info,sqlserver.database_name,sqlserver.nt_username,sqlserver.server_principal_name,sqlserver.session_id,sqlserver.sql_text,sqlserver.transaction_id)),
ADD EVENT sqlserver.lock_deadlock_chain(
    ACTION(sqlserver.context_info,sqlserver.database_name,sqlserver.session_id,sqlserver.sql_text,sqlserver.transaction_id)),
ADD EVENT sqlserver.lock_escalation(
    ACTION(sqlserver.client_app_name,sqlserver.client_pid,sqlserver.context_info,sqlserver.database_name,sqlserver.nt_username,sqlserver.server_principal_name,sqlserver.session_id,sqlserver.sql_text,sqlserver.transaction_id)),
ADD EVENT sqlserver.lock_timeout_greater_than_0(
    ACTION(sqlserver.client_app_name,sqlserver.client_pid,sqlserver.context_info,sqlserver.database_name,sqlserver.nt_username,sqlserver.server_principal_name,sqlserver.session_id,sqlserver.sql_text,sqlserver.transaction_id)),
ADD EVENT sqlserver.sp_statement_completed(SET collect_statement=(1)
    ACTION(sqlserver.client_app_name,sqlserver.client_pid,sqlserver.context_info,sqlserver.database_name,sqlserver.nt_username,sqlserver.server_principal_name,sqlserver.session_id,sqlserver.sql_text,sqlserver.transaction_id)
    WHERE (([package0].[equal_boolean]([sqlserver].[is_system],(0))))),
ADD EVENT sqlserver.sp_statement_starting(SET collect_statement=(1)
    ACTION(sqlserver.client_app_name,sqlserver.client_pid,sqlserver.context_info,sqlserver.database_name,sqlserver.nt_username,sqlserver.server_principal_name,sqlserver.session_id,sqlserver.sql_text,sqlserver.transaction_id)
   WHERE (([package0].[equal_boolean]([sqlserver].[is_system],(0))))),
ADD EVENT sqlserver.sql_batch_completed(
    ACTION(sqlserver.client_app_name,sqlserver.client_pid,sqlserver.context_info,sqlserver.database_name,sqlserver.nt_username,sqlserver.server_principal_name,sqlserver.session_id,sqlserver.sql_text,sqlserver.transaction_id)
WHERE (([package0].[equal_boolean]([sqlserver].[is_system],(0))))),
ADD EVENT sqlserver.sql_statement_completed(
    ACTION(sqlserver.client_app_name,sqlserver.client_pid,sqlserver.context_info,sqlserver.database_name,sqlserver.nt_username,sqlserver.server_principal_name,sqlserver.session_id,sqlserver.sql_text,sqlserver.transaction_id)
WHERE (([package0].[equal_boolean]([sqlserver].[is_system],(0))))),
ADD EVENT sqlserver.sql_statement_starting(
    ACTION(sqlserver.client_app_name,sqlserver.context_info,sqlserver.database_id,sqlserver.query_hash,sqlserver.query_plan_hash,sqlserver.session_id,sqlserver.sql_text,sqlserver.transaction_id)
WHERE (([package0].[equal_boolean]([sqlserver].[is_system],(0))))),
ADD EVENT sqlserver.xml_deadlock_report(
    ACTION(sqlserver.context_info,sqlserver.server_principal_name,sqlserver.session_id,sqlserver.sql_text,sqlserver.transaction_id))
ADD TARGET package0.ring_buffer
WITH (MAX_MEMORY=4096 KB,EVENT_RETENTION_MODE=ALLOW_SINGLE_EVENT_LOSS,MAX_DISPATCH_LATENCY=30 SECONDS,MAX_EVENT_SIZE=0 KB,MEMORY_PARTITION_MODE=NONE,TRACK_CAUSALITY=ON,STARTUP_STATE=OFF)

                ALTER EVENT SESSION [{_sessionName}] ON SERVER STATE = START;";

            using var cmd = new SqlCommand(createSessionSql, conn);
            await cmd.ExecuteNonQueryAsync();
        }

        // Initialize XEvent reader
        _reader = new XELiveEventStreamer(_connectionString, _sessionName);
    }

    public async Task StopSession()
    {
        // _cts.Cancel();
        Debug.WriteLine("in stop session");

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

        Debug.WriteLine("Stop session Done");
    }

    public async Task ReadEventsLoop()
    {
        try
        {
            while (!_cancellationToken.IsCancellationRequested)
            {
                var readTask = _reader.ReadEventStream(() =>
                    {
                        Debug.WriteLine("Connected to session");
                        return Task.CompletedTask;
                    },
                    xevent =>
                    {
                        addEventToDictionary(xevent);
                        return Task.CompletedTask;
                    },
                    _cancellationToken);

                await readTask;
                Debug.WriteLine("Exited readeventstream");
            }
        }
        catch (OperationCanceledException)
        {
            // Normal cancellation, ignore
        }
        catch (Exception ex)
        {
            // Log or handle error
            Debug.WriteLine($"Error in XEvent reader: {ex.GetType().Name}: {ex}");
        }
    }

    protected virtual void Dispose(bool disposing)
    {
        Debug.WriteLine($"In readerDispose status = {_isDisposed}");
        if (!_isDisposed)
        {
            if (disposing)
                Task.Run(() => StopSession()).Wait();

            _isDisposed = true;
        }
    }
}