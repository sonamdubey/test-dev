IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CWAwards_SaveUserDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CWAwards_SaveUserDetails]
GO

	-- =============================================
-- Author:		Vaibhav K
-- Create date: 16-Jan-2013
-- Description:	Save the Carwale Awards user data into CWAwards & return new Id generated
-- Modifier:	Vaibhav K (8-Feb-2013)
--				If survey id is passed then update else insert
-- EXEC  [dbo].[CWAwards_SaveUserDetails] 
-- =============================================
CREATE PROCEDURE [dbo].[CWAwards_SaveUserDetails] 
	-- Add the parameters for the stored procedure here
	@SurveyIdOld		BIGINT,
	@FirstName			VARCHAR(50),
	@LastName			VARCHAR(50),
	@Email				VARCHAR(50),
	@PhoneNo			VARCHAR(50),
	@CarMakedId			SMALLINT,
	@CarMake			VARCHAR(50),
	@CarModelId			SMALLINT,
	@CarModel			VARCHAR(50),
	@CarVersionId		SMALLINT,
	@CarVersion			VARCHAR(50),
	@CarRegistration	VARCHAR(50),
	@FamiliarityOfCar	VARCHAR(500),
	@Source				SMALLINT = 1,
	--from about car page
	@RecommendPoint		TINYINT,  
	@Mileage			TINYINT,  
	@Economy			TINYINT,  
	@Exterior			TINYINT,  
	@Comfort			TINYINT,  
	@Performance		TINYINT,  
	@ValueForMoney		TINYINT,  
	@SurveyId			BIGINT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @ErrorCode INT

    -- Insert statements for procedure here
    
    --initially set new survey id as -1
    SET @SurveyId = -1
    
    --Check if old survey id is passed then update record else insert new record
    IF @SurveyIdOld <> -1
		BEGIN
			UPDATE CWAwards
				SET FirstName = @FirstName, LastName = @LastName, Email = @Email, PhoneNo = @PhoneNo, CarMakedId = @CarMakedId,
					CarMake = @CarMake, CarModelId = @CarModelId, CarModel = @CarModel, CarVersionId = @CarVersionId,
					CarVersion = @CarVersion, CarRegistration = @CarRegistration, FamiliarityOfCar = @FamiliarityOfCar, Source = @Source,
					--from about car page
					RecommendPoint = @RecommendPoint, Mileage = @Mileage, Economy = @Economy, Exterior = @Exterior,  
					Comfort = @Comfort, Performance = @Performance, ValueForMoney = @ValueForMoney  
			WHERE SurveyId = @SurveyIdOld
			
			SET @SurveyId = @SurveyIdOld
		END
	ELSE
		BEGIN
			--Insert values into the table
			BEGIN TRANSACTION
			BEGIN TRY
			
			INSERT INTO CWAwards 
				(
					FirstName	,LastName, Email, PhoneNo, CarMakedId, CarMake, CarModelId, CarModel, CarVersionId,
					CarVersion,CarRegistration,FamiliarityOfCar, Source,
					--from about car page
					RecommendPoint, Mileage, Economy, Exterior, Comfort, Performance, ValueForMoney  
				)
			VALUES 
				(
					@FirstName	,@LastName, @Email, @PhoneNo, @CarMakedId, @CarMake, @CarModelId, @CarModel, @CarVersionId,
					@CarVersion,@CarRegistration,@FamiliarityOfCar, @Source,
					--from about car page
					@RecommendPoint, @Mileage, @Economy, @Exterior, @Comfort, @Performance, @ValueForMoney  
				)
			COMMIT TRANSACTION
			END TRY
			
			BEGIN CATCH
			  SELECT @ErrorCode=ERROR_NUMBER() 
			  ROLLBACK TRANSACTION
			END CATCH
			
			If (@ErrorCode=2601)
			SET @SurveyId=0
			else 
			SET @SurveyId = SCOPE_IDENTITY()
		END 
END
