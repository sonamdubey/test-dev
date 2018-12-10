IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_UpdateDoubtfulToSurveyDone]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_UpdateDoubtfulToSurveyDone]
GO

	-- =============================================
-- Author:   Yuga Hatolkar
-- Create date: Sept 22, 2016
-- Description:	To Update Car Status from Doubtful to Survey Done.
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_UpdateDoubtfulToSurveyDone]
(
	@AbSure_CarDetailsId	BIGINT	
)
AS
BEGIN	
	
	UPDATE AbSure_CarDetails SET Status = 1, RCImagePending = 0 WHERE Id = @AbSure_CarDetailsId	
	--UPDATE AbSure_DoubtfulCarReasons SET IsActive = 0 WHERE AbSure_CarDetailsId = @AbSure_CarDetailsId

END