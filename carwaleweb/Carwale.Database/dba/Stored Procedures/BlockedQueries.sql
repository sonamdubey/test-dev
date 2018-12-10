IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dba].[BlockedQueries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dba].[BlockedQueries]
GO

	CREATE procedure [dba].[BlockedQueries]
as
insert into dba.BlockingQueries
SELECT TOP 20 
 [Average Time Blocked] = (total_elapsed_time - total_worker_time) / qs.execution_count
,[Total Time Blocked] = total_elapsed_time - total_worker_time 
,[Execution count] = qs.execution_count
,[Individual Query] = SUBSTRING (qt.text,qs.statement_start_offset/2, 
         (CASE WHEN qs.statement_end_offset = -1 
            THEN LEN(CONVERT(NVARCHAR(MAX), qt.text)) * 2 
          ELSE qs.statement_end_offset END - qs.statement_start_offset)/2) 
,[Parent Query] = qt.text
,DatabaseName = DB_NAME(qt.dbid),
GETDATE() as UpdatedOn
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) as qt
ORDER BY [Average Time Blocked] DESC;