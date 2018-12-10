IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckGCMRegistration]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckGCMRegistration]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 12-06-2014
-- Description:	Alert GCMRegistration is not NULL on staging server
-- =============================================
CREATE PROCEDURE [dbo].[CheckGCMRegistration]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	CREATE TABLE #GCMRegistrationID(
	RegType varchar(20),
	UserCount int	
	)
	
    INSERT INTO #GCMRegistrationID(RegType,UserCount)
    SELECT 'TC_USERS' as RegType,count(*) as UserCount
	from cwstaging..TC_USERS 
	where GCMRegistrationId is not NULL
	
	UNION

	SELECT 'CarWale' as RegType,count(*) as UserCount
	from  cwstaging.mobile.mobileusers  
	where GCMRegId is not NULL
	
	
    SELECT *
    FROM #GCMRegistrationID
    WHERE UserCount>10

	
	DROP table #GCMRegistrationID
END
