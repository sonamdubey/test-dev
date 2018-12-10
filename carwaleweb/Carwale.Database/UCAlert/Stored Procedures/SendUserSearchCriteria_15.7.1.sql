IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[SendUserSearchCriteria_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[SendUserSearchCriteria_15]
GO

	-- =============================================
-- Author:		Jugal Singh
-- Create date: 01-08-2014
-- Description:	Set Customer's used car search criteria
-- Modified by: Manish on 05-09-2014 handle null value in case of max kms, budget and car age.
-- Modified By : Sadhana Upadhyay on 11 June 2015
-- Summary : Change data Type for MinBudget and MaxBudget
-- =============================================
CREATE PROCEDURE [UCAlert].[SendUserSearchCriteria_15.7.1]
	-- Add the parameters for the stored procedure here
	 @CustomerId Bigint = -1,
     @Email varchar(100),
     @CityId Smallint = null,
     @MakeId varchar(MAX) = null,
     @modelId varchar(500) = null,
     @FuelTypeId varchar(500) = null,
     @BodyStyleId varchar(500) = null,
     @TransmissionId varchar(50) = null,
     @SellerId varchar(50) = null,
     @MinBudget float = 0,
     @MaxBudget float = NULL,
     @MinKms int = 0,
     @MaxKms int = NULL,
     @MinCarAge int = 0,
     @MaxCarAge int = NULL,
     @NeedOnlyCertifiedCars bit,
     @NeedCarWithPhotos bit,
     @OwnerTypeId VARCHAR(50) = null,
     @AlertFrequency tinyint,
     @alertUrl varchar(MAX),
	 @Status bit = 1 OUTPUT
AS

BEGIN   
	SET NOCOUNT ON;

    BEGIN TRY

		SET @MinBudget = @MinBudget * 100000;
		
		IF (@MaxBudget IS NOT NULL)
		BEGIN
			SET @MaxBudget = @MaxBudget * 100000;
		END
		ELSE IF (@MaxBudget IS NULL)        -----Added by Manish on 05-09-2014
		BEGIN 
		  SET @MaxBudget = 990000000;

		END

		SET @MinKms = @MinKms * 1000;
		
		IF (@MaxKms IS NOT NULL)
		BEGIN
			SET @MaxKms = @MaxKms * 1000;
		END
		ELSE IF (@MaxKms IS  NULL)   -----Added by Manish on 05-09-2014
		BEGIN
		   	SET @MaxKms = 9900000;
		END
		
		IF (@MaxCarAge IS NULL)      -----Added by Manish on 05-09-2014
		BEGIN 
		 SET  @MaxCarAge=150
		END



		INSERT INTO UCAlert.NDUsedCarAlertCustomerList
		(   CustomerId,
			Email,
			CityId,
			MakeId,
			ModelId,
			FuelTypeId,
			BodyStyleId,
			TransmissionId,
			SellerId,
			MinBudget,
			MaxBudget,
		    MinKms,
		    MaxKms,
			MinCarAge,
			MaxCarAge,
			NeedOnlyCertifiedCars,
			NeedCarWithPhotos,
			OwnerTypeId,
			EntryDateTime,
			AlertFrequency,
			alertUrl
		)
		VALUES (@CustomerId,
		        @Email,
				@CityId,
				@MakeId,
				@modelId,
				@FuelTypeId,
				@BodyStyleId,
				@TransmissionId,
				@SellerId,
				@MinBudget,
				@MaxBudget,
				@MinKms,
				@MaxKms,
				@MinCarAge,
				@MaxCarAge,
				@NeedOnlyCertifiedCars,
				@NeedCarWithPhotos,
				@OwnerTypeId,
				GETDATE(),
				@AlertFrequency,
				@alertUrl)
		
		SET @Status = 1	 

	END TRY

	BEGIN CATCH
		SET @Status = 0
	END CATCH

	SELECT @Status	 
	END
