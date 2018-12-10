IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dba].[GetDBServerHWStats]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dba].[GetDBServerHWStats]
GO

	CREATE procedure [dba].[GetDBServerHWStats]
as
INSERT INTO Dba.DBServerHWStats
SELECT cpu_count AS [Logical CPU Count], hyperthread_ratio AS [Hyperthread Ratio],
cpu_count/hyperthread_ratio AS [Physical CPU Count], 
physical_memory_in_bytes/1048576 AS [Physical Memory (MB)], sqlserver_start_time
FROM sys.dm_os_sys_info;