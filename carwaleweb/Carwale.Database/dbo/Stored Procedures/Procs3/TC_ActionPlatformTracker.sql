IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_ActionPlatformTracker]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_ActionPlatformTracker]
GO

	-- =============================================
-- Author:		<Vivek,,Gupta>
-- Create date: <10-06-2015,,>
-- Description:	Save action Application for photo upload
-- exec TC_ActionPlatformTracker 1,5,2
-- =============================================
CREATE PROCEDURE [dbo].[TC_ActionPlatformTracker] @TC_ActionId SMALLINT
	,@BranchId BIGINT
	,@ActionPlatformId TINYINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT OFF;

	INSERT INTO TC_ActionsPlatformTrack (
		TC_ActionId
		,BranchId
		,ActionPlatformId
		,ActionDate
		)
	VALUES (
		@TC_ActionId
		,@BranchId
		,@ActionPlatformId
		,GETDATE()
		)
END
