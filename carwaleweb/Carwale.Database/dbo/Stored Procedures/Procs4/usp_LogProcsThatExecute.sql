IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[usp_LogProcsThatExecute]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[usp_LogProcsThatExecute]
GO

	
 

 -- =============================================
-- Author:		Kundan Dombale
-- Create date: 29/12/2015
-- Description: insert the logs of cahed procedure info from sys.dm_exec_procedure_stats into ProcLog table
-- =============================================
CREATE PROCEDURE [dbo].[usp_LogProcsThatExecute]
AS 
BEGIN 
   
 INSERT INTO dba.proclog(						
						AsOnDate,
						dbname,
						schemaName,
						Procname,
						CreateDate,
						ModifiedDate,
						cached_time,
						last_execution_time,
						execution_count,
						total_worker_time,
						last_worker_time,
						min_worker_time,
						max_worker_time,
						total_physical_reads,
						last_physical_reads,
						min_physical_reads,
						max_physical_reads,
						total_logical_writes,
						last_logical_writes,
						min_logical_writes,
						max_logical_writes,
						total_logical_reads,
						last_logical_reads,
						min_logical_reads,
						max_logical_reads,
						total_elapsed_time,
						last_elapsed_time,
						min_elapsed_time,
						max_elapsed_time
				
					)

				SELECT  Getdate() as AsOnDate,
						db_name (st.database_id ) as dbname,
						SCHEMA_NAME(schema_id()) as schemaName,
						p.name as ProcName,
						p.create_date,
						p.modify_date as ModifiedDate,
						st.cached_time,
						st.last_execution_time,
						st.execution_count,
						st.total_worker_time,
						st.last_worker_time,
						st.min_worker_time,
						st.max_worker_time,
						st.total_physical_reads,
						st.last_physical_reads,
						st.min_physical_reads,
						st.max_physical_reads,
						st.total_logical_writes,
						st.last_logical_writes,
						st.min_logical_writes,
						st.max_logical_writes,
						st.total_logical_reads,
						st.last_logical_reads,
						st.min_logical_reads,
						st.max_logical_reads,
						st.total_elapsed_time,
						st.last_elapsed_time,
						st.min_elapsed_time,
						st.max_elapsed_time
						FROM sys.procedures AS P
          JOIN sys.dm_exec_procedure_stats AS st ON p.[object_id] = st.[object_id]
       -- WHERE  p.is_ms_shipped = 0 --------- for user created sp's
 end
