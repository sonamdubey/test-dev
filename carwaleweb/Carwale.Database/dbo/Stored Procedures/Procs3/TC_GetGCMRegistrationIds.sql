IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetGCMRegistrationIds]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetGCMRegistrationIds]
GO
	-- =============================================
-- Author:	Afrose	
-- Create date: 24-11-2015
-- Description:	To get GCM Registration Id's under a common branch for android push notification
-- EXEC TC_GetGCMRegistrationIds 5
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetGCMRegistrationIds]
	-- Add the parameters for the stored procedure here
	@BranchId INT	
AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT TU.GCMRegistrationId FROM TC_USERS TU WITH(NOLOCK)
	WHERE TU.BranchId=@BranchId AND TU.IsActive=1 AND TU.Mobile IS NOT NULL AND TU.GCMRegistrationId IS NOT NULL
    
END

