IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_SaveCarError]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_SaveCarError]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 9 Dec 2014
-- Description:	AbSure - Save Error against a car matched with VersionId & Reg no. from AbSure_CarScore_CarDetails in table AbSure_CarScore_CarError
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_SaveCarError]
	-- Add the parameters for the stored procedure here
	@AbSure_CarId		NUMERIC(18, 0),
	@ErrorTimeStamp		DATETIME,
	@ErrorCode			VARCHAR(MAX),
	@Description		VARCHAR(MAX),
	@CriticalityIndex	INT,
	@Meaning			VARCHAR(MAX) = NULL,
	@Causes				VARCHAR(MAX) = NULL,
	@Symptoms			VARCHAR(MAX) = NULL,
	@Impact				VARCHAR(MAX) = NULL,
	@Solutions			VARCHAR(MAX) = NULL,
	@Status				BIT = 0 OUTPUT
AS
BEGIN
		
	SET @Status = 0	

	IF @AbSure_CarId IS NOT NULL AND @AbSure_CarId <> -1
	BEGIN
		--Log data of error & other parameters against the above AbSure_CarDetailsId
		INSERT INTO AbSure_CarError 
			(
				AbSure_CarDetailsId, ErrorTimeStamp, ErrorCode, Description, 
				CriticalityIndex, Meaning, Causes, Symptoms, Impact, Solutions
			)
		VALUES
			(
				@AbSure_CarId, @ErrorTimeStamp, @ErrorCode, @Description, 
				@CriticalityIndex, @Meaning, @Causes, @Symptoms, @Impact, @Solutions
			)

		SET @Status = 1
	END
END
