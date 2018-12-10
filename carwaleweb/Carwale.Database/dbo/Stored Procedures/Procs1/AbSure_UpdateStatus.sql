IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_UpdateStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_UpdateStatus]
GO

	
-- =============================================
-- Author:		Deepak Tripathi
-- Create date: 27th Apr 2015
-- Description:	To Update AbSure Status
-- modifier 1 : Ruchira Patil  on 3rd sept 2015 (allowed to update status when it changes from surveyor assigned to agency assigned)
-- Modified by : Nilima More on 24th sept 2015(Doubtfull status can change to accpte,reject)
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_UpdateStatus] 
	@AbSure_CarDetailsId	NUMERIC,
	@Status					INT,
	@ModifiedBy				INT = -1,
	@IsUpdated				BIT = NULL OUTPUT,
	@PreviousStatus			INT = NULL,
	@IsTriggerCall			BIT = 0
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	SET @IsUpdated = 0
	
	DECLARE @IsUpdateAllowed BIT = 0
		
	-- Previous Status is not given, get it
	IF @IsTriggerCall = 0
		BEGIN
			SELECT @PreviousStatus = Status FROM AbSure_CarDetails WHERE ID = @AbSure_CarDetailsId
		END
	
	IF ISNULL(@PreviousStatus,0) <= 0 AND ISNULL(@Status,0) IN(1,2,3,4,5,6) -- No Status Yet Can Change to Agency Assigned, Surveyor Assigned, SurveyDone, Rejected, Cancelled, Accepted 
		SET @IsUpdateAllowed = 1 
	ELSE IF @PreviousStatus = 1 AND ISNULL(@Status,0) IN(2, 4, 7) -- SurveyDone Can Change to Rejected, Accepted, Certificate Expired
		SET @IsUpdateAllowed = 1 
	ELSE IF @PreviousStatus = 2 -- Rejected No Updation Possible
		SET @IsUpdateAllowed = 0
	ELSE IF @PreviousStatus = 3 AND ISNULL(@Status,0) = 1 -- Cancelled, In ideal case No Updation Possible, incase cancellation is done and from offline mode surveyor submits inspection
		SET @IsUpdateAllowed = 1
	ELSE IF @PreviousStatus = 4 AND ISNULL(@Status,0) IN(7, 8) -- Accepted Can Change to Certificate Expired, Warranty Activated
		SET @IsUpdateAllowed = 1
	ELSE IF @PreviousStatus = 5 AND ISNULL(@Status,0) IN(1,2,3,4,6) -- Surveyor Assigned Can Change to SurveyDone, Rejected, Cancelled, Accepted, Agency
		SET @IsUpdateAllowed = 1
	ELSE IF @PreviousStatus = 6 AND ISNULL(@Status,0) IN(1,2,3,4,5) -- Agency Assigned Can Change to Surveyor Assigned, SurveyDone, Rejected, Cancelled, Accepted
		SET @IsUpdateAllowed = 1
	ELSE IF @PreviousStatus = 7 -- Certificate Expired No Updation Possible
		SET @IsUpdateAllowed = 0
	ELSE IF @PreviousStatus = 8 -- Warranty Activated No Updation Possible
		SET @IsUpdateAllowed = 0
	ELSE IF @PreviousStatus = 9 AND ISNULL(@Status,0) IN(1,2,4,9) -- Doubtful Status can be changed to Survey Done, Rejected, accepted and doubtful (if needed).
		SET @IsUpdateAllowed = 1

	IF @IsUpdateAllowed = 1
		BEGIN
			UPDATE AbSure_CarDetails 
			SET Status = @Status, 
					SurveyDate			= CASE ISNULL(@Status,0) WHEN 1 THEN GETDATE() ELSE SurveyDate END,
					RejectedDateTime	= CASE ISNULL(@Status,0) WHEN 2 THEN GETDATE() ELSE RejectedDateTime END,
					CancelledOn			= CASE ISNULL(@Status,0) WHEN 3 THEN GETDATE() ELSE CancelledOn END,
					FinalWarrantyDate	= CASE ISNULL(@Status,0) WHEN 4 THEN GETDATE() ELSE FinalWarrantyDate END
			WHERE ID = @AbSure_CarDetailsId
			SET @IsUpdated = 1
		END
	ELSE
		SET @IsUpdated = 0
		
	INSERT INTO AbSure_StatusChangeLog(AbSure_CarDetailsId, Status, PreviousStatus, ModifiedBy, IsModified) VALUES(@AbSure_CarDetailsId, @Status, @PreviousStatus, @ModifiedBy, @IsUpdateAllowed)
END

----------------------------------------------------------------------


