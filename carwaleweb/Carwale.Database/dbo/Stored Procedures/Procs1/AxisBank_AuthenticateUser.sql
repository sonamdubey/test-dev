IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_AuthenticateUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AxisBank_AuthenticateUser]
GO

	
-- =============================================
-- Author:		Kumar Vikram
-- Create date: 18.12.2013
-- Description:	authenticate user from accessing file reference number
-- exec AxisBank_AuthenticateUser 457,2,0
-- =============================================
 CREATE PROCEDURE [dbo].[AxisBank_AuthenticateUser]
	@FileReferenceNumber VARCHAR(20),
	@CustomerId NUMERIC,
	@IsExist BIT OUTPUT
	AS
BEGIN
	
	SET NOCOUNT ON ;

	SELECT CustomerId 
	FROM AxisBank_CarValuations 
	WHERE customerid in (
		SELECT DISTINCT customerid 
		FROM AxisBank_CarValuations 
		WHERE FileReferenceNumber=@FileReferenceNumber 
		AND customerid <> @CustomerId) 
	AND FileReferenceNumber=@FileReferenceNumber

	IF @@ROWCOUNT > 0
	SET @IsExist = 1
	ELSE
	SET @IsExist = 0
END
