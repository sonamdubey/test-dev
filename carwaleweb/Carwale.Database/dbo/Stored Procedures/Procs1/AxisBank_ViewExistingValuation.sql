IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_ViewExistingValuation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_ViewExistingValuation]
GO

	

-- =============================================
-- Author:		Kumar Vikram
-- Create date: 16.12.2013
-- Description:	Gets the previous valuations report based on file reference number
-- exec AxisBank_ViewExistingValuation 23,1,1
-- =============================================
 CREATE PROCEDURE [dbo].[AxisBank_ViewExistingValuation]
	@FileReferenceNumber varchar(20),
	@CustomerId NUMERIC,
	@IsAdmin BIT
	AS
BEGIN

	SET NOCOUNT ON;

	IF @IsAdmin= 1 
	BEGIN
		SELECT FileReferenceNumber, 
			   RegistrationNumber,
			   CustomerId,
			   CarYear, 
			   V.Car,
			   CF.FuelType,
			   CarVersionId, 
			   City, 
			   Kms, 
			   CarCondition,
			   RegistrationNumber,
			   ValueExcellent,
			   ValueFair,
			   ValueGood,
			   UserId,
			   LoginId,
			   IsAdmin, 
			   RequestDateTime,
			   V.Make,
			   V.Model,
			   V.Version 
		FROM AxisBank_CarValuations CV WITH(NOLOCK)
		Inner Join vwMMV V on CV.CarVersionId=v.VersionId
		Inner Join AxisBank_Users U on CV.CustomerId=U.UserId
		Inner Join CarFuelType CF on V.CarFuelType=CF.FuelTypeId
		WHERE FileReferenceNumber = @FileReferenceNumber 
		ORDER BY RequestDateTime
	END

	ELSE
	BEGIN
		SELECT FileReferenceNumber, 
			   RegistrationNumber,
			   CustomerId,
			   CarYear, 
			   V.Car,
			   CF.FuelType,
			   CarVersionId, 
			   City, 
			   Kms, 
			   CarCondition,
			   RegistrationNumber,
			   ValueExcellent,
			   ValueFair,
			   ValueGood,
			   UserId,
			   LoginId,
			   IsAdmin, 
			   RequestDateTime,
			   V.Make,
			   V.Model,
			   V.Version
		FROM AxisBank_CarValuations CV 
		Inner Join vwMMV V on CV.CarVersionId=v.VersionId
		Inner Join AxisBank_Users U on CV.CustomerId=U.UserId
		Inner Join CarFuelType CF on V.CarFuelType=CF.FuelTypeId
		WHERE FileReferenceNumber = @FileReferenceNumber AND CustomerId = @CustomerId 
		ORDER BY RequestDateTime
	END

END


---------------------------------------------------------------------------------------------------------------



