IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DBLogicalConnections]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DBLogicalConnections]
GO

	
-- =============================================
-- Author:		Avishkar
-- Create date: 28-6-2016
-- Description:	Return No Of Logical connections 
-- Modified by Manish on 14-07-2016 for include total connection as well.
-- =============================================
CREATE PROCEDURE [dbo].[DBLogicalConnections]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	Declare @cnt int
	
	
	select hostname as Hostname,COUNT(kpid) as 'Logical Connections'
	into #tempConnections
	from sys.sysprocesses
	where spid>50
	group by hostname
	
	
	insert into #tempConnections
	select 'Total Connections' as Hostname, COUNT(kpid) as 'Logical Connections'
	from sys.sysprocesses
	
	--select *
	--into #Connections
	--from #tempConnections
	--where Hostname='Total Connections'
	--and [Logical Connections] >800

	
	select *
	into #Connections
	from #tempConnections
	where Hostname!='Total Connections' and  [Logical Connections] >90
	
	select  @cnt = count(*)
	from #Connections
	
	if 	(@cnt >0)
	Begin
		insert into #Connections
		select 'Total Connections' as Hostname, COUNT(kpid) as 'Logical Connections'
		from sys.sysprocesses

		select * from #Connections
    end
	

END

