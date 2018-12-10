IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[sysProcessesLogs]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[sysProcessesLogs]
GO

	-- =============================================
-- Author:		<Kundan Dombale>
-- Create date: <24 May 2016 >
-- Description:	<Used to capture information of sys.sysprocess system table>
-- =============================================
CREATE PROCEDURE  sysProcessesLogs
	 
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	INSERT INTO carwale_com.dba.sysProcessesLogs( [AsOnDate]
	                                      ,[spid]
										  ,[kpid]
										  ,[blocked]
										  ,[waittype]
										  ,[waittime]
										  ,[lastwaittype]
										  ,[waitresource]
										  ,[dbid]
										  ,[uid]
										  ,[cpu]
										  ,[physical_io]
										  ,[memusage]
										  ,[login_time]
										  ,[last_batch]
										  ,[ecid]
										  ,[open_tran]
										  ,[status]
										  ,[sid]
										  ,[hostname]
										  ,[program_name]
										  ,[hostprocess]
										  ,[cmd]
										  ,[nt_domain]
										  ,[nt_username]
										  ,[net_address]
										  ,[net_library]
										  ,[loginame]
										  ,[context_info]
										  ,[sql_handle]
										  ,[stmt_start]
										  ,[stmt_end]
										  ,[request_id]
						)
						 SELECT 
						  GetDate()
						  ,[spid]
						  ,[kpid]
						  ,[blocked]
						  ,[waittype]
						  ,[waittime]
						  ,[lastwaittype]
						  ,[waitresource]
						  ,[dbid]
						  ,[uid]
						  ,[cpu]
						  ,[physical_io]
						  ,[memusage]
						  ,[login_time]
						  ,[last_batch]
						  ,[ecid]
						  ,[open_tran]
						  ,[status]
						  ,[sid]
						  ,[hostname]
						  ,[program_name]
						  ,[hostprocess]
						  ,[cmd]
						  ,[nt_domain]
						  ,[nt_username]
						  ,[net_address]
						  ,[net_library]
						  ,[loginame]
						  ,[context_info]
						  ,[sql_handle]
						  ,[stmt_start]
						  ,[stmt_end]
						  ,[request_id]
					  FROM [master].[sys].[sysprocesses]
   
END
