IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_LogUserActivities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_LogUserActivities]
GO

	
-- =============================================
-- Author:		Satish Sharma
-- Create date: 18-12-2013
-- Description:	Log user activities
-- =============================================
CREATE PROCEDURE [dbo].[AxisBank_LogUserActivities] -- EXEC AxisBank_UserActivitiesLog 1, 1, GETDATE(), '192.77.1.45'
	-- Add the parameters for the stored procedure here
	@UserId	INT,
	@ActivityTypeId SMALLINT,
	@ActivityDateTime DATETIME,
	@ClientIP VARCHAR(15)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	INSERT INTO AxisBank_UserActivitiesLog(UserId, ActivityTypeId, ActivityDateTime, ClientIP)
	VALUES(@UserId, @ActivityTypeId, @ActivityDateTime, @ClientIP)
END

