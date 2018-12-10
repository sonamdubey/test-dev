IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_SaveInspectionDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_SaveInspectionDetails]
GO

	
-- =============================================
-- Author      : Kartik Rathod
-- Create date : 30th July 2015
-- Description : To capture the time of sync photo,data for inspection.
-- Modifier 1  : Ruchira Patil on 3rd Aug 2015 (added 2 parameters @InspectionStartTime,@InspectionEndTime)
-- =============================================

CREATE PROC [dbo].[Absure_SaveInspectionDetails] 
	
	@CarId					INT,
	@TC_UserId				INT = NULL,
	@PhotoSyncStartTime		DATETIME = NULL,
	@PhotoSyncCompleteTime	DATETIME = NULL,
	@DataSyncStartTime		DATETIME = NULL,
	@DataSyncCompleteTime	DATETIME = NULL,
	@InspectionStartTime	DATETIME = NULL,
	@InspectionEndTime		DATETIME = NULL
AS
BEGIN

	IF EXISTS(SELECT Absure_InspectionDetailsId FROM Absure_InspectionDetails WHERE CarId = @CarId)
	BEGIN
		UPDATE Absure_InspectionDetails
		SET
			TC_UserId = @TC_UserId,
			PhotoSyncStartTime = @PhotoSyncStartTime,
			PhotoSyncCompleteTime = @PhotoSyncCompleteTime,
			DataSyncStartTime = @DataSyncStartTime,
			DataSyncCompleteTime = @DataSyncCompleteTime,
			ModifiedOn = GETDATE()
		WHERE CarId = @CarId
	END
	ELSE
	BEGIN
		INSERT INTO Absure_InspectionDetails(CarId,Entrydate,InspectionStartTime,InspectionEndTime)
		VALUES (@CarId,GETDATE(),@InspectionStartTime,@InspectionEndTime)
	END
END
