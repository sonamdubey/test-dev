IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_SaveWarrantyInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_SaveWarrantyInquiry]
GO

	--==========================================================================================
-- Author      : Suresh Prajapati
-- Created On  : 24th Feb, 2015

-- Summary     : Proc to save and update vehicle details

-- Modified by : Suresh Prajapati on 11th Mar, 2015
-- Description : To save engine number and VIN

-- Modified By : Ashwini Todkar on 12 March 2014
-- description : added extra parameter @ProductType  to identify rsa and warranty inquiries

-- Modified by : Suresh Prajapati on 20th Mar, 2015
-- Description : To save CarFitted information of cng and lpg 
--===========================================================================================
CREATE PROCEDURE [dbo].[Absure_SaveWarrantyInquiry]
	-- Add the parameters for the stored procedure here
	@VersionId INT = NULL
	,@WarrantyType TINYINT = NULL
	,@WarrantyPrice FLOAT = NULL
	,@WarrantyInquiryId INT = NULL OUTPUT
	,@RegNo VARCHAR(50) = NULL
	,@EngineNumber VARCHAR(50) = NULL --Added by Suresh Prajapati
	,@VIN VARCHAR(50) = NULL --Added by Suresh Prajapati
	,@Kilometers INT = NULL
	,@FuelType SMALLINT = NULL
	,@CarFittedWith TINYINT = NULL
	,@RegistrationDate DATETIME = NULL
	,@ProductType TINYINT -- Added by Ashwini
AS
BEGIN
	IF (
			@WarrantyInquiryId IS NULL
			OR @WarrantyInquiryId = - 1
			) /* This is a new warranty request */
	BEGIN
		INSERT INTO Absure_WarrantyInquiries (
			WarrantyType
			,WarrantyPrice
			,VersionId
			,RegistrationNo
			,EngineNumber
			,VIN
			,RegistrationDate
			,FuelType
			,CarFittedWith
			,Kilometers
			,EntryDate
			,ProductType
			)
		VALUES (
			@WarrantyType
			,@WarrantyPrice
			,@VersionId
			,@RegNo
			,@EngineNumber
			,@VIN
			,@RegistrationDate
			,@FuelType
			,@CarFittedWith
			,@Kilometers
			,GETDATE()
			,@ProductType
			)

		SET @WarrantyInquiryId = SCOPE_IDENTITY();
	END
	ELSE /* Updating vehicle details */
	BEGIN
		UPDATE Absure_WarrantyInquiries
		SET WarrantyType = @WarrantyType
			,WarrantyPrice = @WarrantyPrice
			,VersionId = @VersionId
			,RegistrationDate = @RegistrationDate
			,RegistrationNo = @RegNo
			,EngineNumber = @EngineNumber
			,VIN = @VIN
			,FuelType = @FuelType
			,CarFittedWith = @CarFittedWith
			,Kilometers = @Kilometers
		WHERE Id = @WarrantyInquiryId
	END
			--SET @WarrantyInquiryId = SCOPE_IDENTITY();
END

