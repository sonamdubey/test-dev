IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_SaveSurveyorActivity]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_SaveSurveyorActivity]
GO

	

--========================================================================
-- Author      : Vinay Kumar Prajapati
-- Created On  : 10th Sept, 2015
-- Summary     : To save Surveyor's Online Activity Details....
--========================================================================
CREATE PROCEDURE [dbo].[Absure_SaveSurveyorActivity]
	
	 @SurveyorActivityTable  dbo.SurveyorActivityTable  READONLY       -- @RatingResult dbo.Absure_RatingResult READONLY
	,@SurveyorId INT =NULL
	,@Success BIT = 0 OUTPUT
AS
BEGIN

	INSERT INTO  Absure_SurveyorActivityDetails(SurveyorId,OnlineTime,OfflineTime,EntryDate)
	SELECT @SurveyorId,Online,Offline,GETDATE()
	FROM @SurveyorActivityTable

	SET @Success = 1
END
