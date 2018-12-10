IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[UCAlert].[SetUsedCarCustomerSearchCriteria]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [UCAlert].[SetUsedCarCustomerSearchCriteria]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 26-12-2011
-- Description:	Set Customer's used car serach criteria
-- Modified By: Ashish G. Kamble on 24/1/2012
-- =============================================
CREATE PROCEDURE [UCAlert].[SetUsedCarCustomerSearchCriteria]
	@CustomerId int = -1,
	@Email Varchar(100),
	@CityId SmallInt,
	@City Varchar(50) = null,	--	give city name
	@CityDistance SmallInt,
	@BudgetId Varchar(max) = null,
	@Budget Varchar(max) = null,	-- give budget comma separated
	@YearId Varchar(50) = null,
	@MakeYear Varchar(100) = null,	-- give makeYear
	@KmsId Varchar(50) = null,
	@Kms Varchar(100) = null,	--  gives kms
	@MakeId Varchar(max) = null,
	@Make Varchar(max) = null,	-- gives make name
	@ModelId Varchar(max) = null,
	@Model Varchar(max) = null,	-- gives model name
	@FuelTypeId Varchar(50) = null,
	@FuelType Varchar(100) = null,	-- gives fuel type
	@BodyStyleId Varchar(50) = null,
	@BodyStyle Varchar(100) = null,	-- gives body style
	@TransmissionId Varchar(50) = null,
	@Transmission Varchar(100) = null,	-- gives Transmission type
	@SellerId Varchar(50) = null,
	@Seller Varchar(50) = null,	-- gives seller type
	@AlertFrequency TinyInt = 1,
	@alertUrl Varchar(MAX),		
	@Status bit = 1 OUTPUT
	
AS
BEGIN   
   
	SET NOCOUNT ON;
    
    BEGIN TRY
		-- Declare all variables
		
		-- Insert data into userCarAlerts
		INSERT INTO UCAlert.UserCarAlerts
		(   CustomerId,
			Email,
			CityId,
			City,
			CityDistance,
			BudgetId,
			Budget,
			YearId,
			MakeYear,
			KmsId,
			Kms,
			MakeId,
			Make,
			ModelId,
			Model,
			FuelTypeId,
			FuelType,
			BodyStyleId,
			BodyStyle,
			TransmissionId,
			Transmission,
			SellerId,--seller type individual or dealer
			Seller,
			EntryDateTime,
			AlertFrequency,
			alertUrl
		)
		 VALUES(@CustomerId,@Email,@CityId,@City,@CityDistance,@BudgetId,@Budget,@YearId,@MakeYear,@KmsId,@Kms,@MakeId,@Make,@ModelId,@Model,@FuelTypeId,@FuelType,@BodyStyleId,@BodyStyle,@TransmissionId,@Transmission,@SellerId,@Seller,GETDATE(),@AlertFrequency,@alertUrl)
		SET @Status = 1	 
	END TRY
	BEGIN CATCH
		SET @Status = 0
	END CATCH
	
	select @Status
		 
	END
