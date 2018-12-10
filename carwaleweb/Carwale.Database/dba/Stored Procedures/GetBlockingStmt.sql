IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dba].[GetBlockingStmt]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dba].[GetBlockingStmt]
GO

	CREATE procedure [dba].[GetBlockingStmt]
as
INSERT INTO dba.BlockingStmt
([lock type],
[database],
[blk object],
[lock req],
[waiter sid],
[wait time],
waiter_batch,
waiter_stmt,
[blocker sid],
blocker_stmt
)
SELECT t1.resource_type                              AS 'lock type', 
       Db_name(resource_database_id)                 AS 'database', 
       t1.resource_associated_entity_id              AS 'blk object', 
       t1.request_mode                               AS 'lock req', 
       --- lock requested 
       t1.request_session_id                         AS 'waiter sid', 
       t2.wait_duration_ms                           AS 'wait time', 
       -- spid of waiter   
       (SELECT [text] 
        FROM   sys.dm_exec_requests AS r -- get sql for waiter 
               CROSS apply sys.Dm_exec_sql_text(r.sql_handle) 
        WHERE  r.session_id = t1.request_session_id) AS 'waiter_batch', 
       (SELECT Substring(qt.text, r.statement_start_offset / 2, 
               ( CASE 
                   WHEN 
                       r.statement_end_offset = -1 THEN Len( 
               CONVERT(nvarchar(max), qt.text)) * 
                                                        2 
                                                                    ELSE 
                 r.statement_end_offset 
                                                                  END - 
               r.statement_start_offset ) / 2)
        
        FROM   sys.dm_exec_requests AS r 
               CROSS apply sys.Dm_exec_sql_text(r.sql_handle) AS qt 
        
        WHERE  r.session_id = t1.request_session_id) AS 'waiter_stmt', 
       -- statement blocked 
       t2.blocking_session_id                        AS 'blocker sid', 
       -- spid of blocker 
       (SELECT [text] 
        FROM   sys.sysprocesses AS p -- get sql for blocker 
               CROSS apply sys.Dm_exec_sql_text(p.sql_handle) 
        WHERE  p.spid = t2.blocking_session_id)      AS 'blocker_stmt' 

FROM   sys.dm_tran_locks AS t1 
       INNER JOIN sys.dm_os_waiting_tasks AS t2 
               ON t1.lock_owner_address = t2.resource_address; 