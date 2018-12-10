IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dba].[GetTopExecutedstmt]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dba].[GetTopExecutedstmt]
GO

	create  procedure [dba].[GetTopExecutedstmt]
as
insert into dba.TopExecutedstmt
SELECT TOP 10 
 [Execution count] = execution_count
,[Individual Query] = SUBSTRING (qt.text,qs.statement_start_offset/2, 
         (CASE WHEN qs.statement_end_offset = -1 
            THEN LEN(CONVERT(NVARCHAR(MAX), qt.text)) * 2 
          ELSE qs.statement_end_offset END - qs.statement_start_offset)/2)
,[Parent Query] = qt.text
,DatabaseName = DB_NAME(qt.dbid),
GETDATE() as UpdatedOn
FROM sys.dm_exec_query_stats qs
CROSS APPLY sys.dm_exec_sql_text(qs.sql_handle) as qt
ORDER BY [Execution count] DESC;