IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_UpdateWarrantyPolicyNumber]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Absure_UpdateWarrantyPolicyNumber]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 20th Mar 2015
-- Description:	To update Warranty PolicyNo  in case of absure.in
-- =============================================
CREATE PROCEDURE [dbo].[Absure_UpdateWarrantyPolicyNumber]
	@CarId		INT,
	@IsDealer	BIT
AS
BEGIN
	DECLARE @PolicyNo VARCHAR(50) 

	INSERT INTO absure_policy(AbSure_CarId) VALUES (@CarId)

	SELECT @PolicyNo = [dbo].[Absure_GenerateWarrantyPolicyNo](@CarId,@IsDealer)
		
	UPDATE AbSure_CarDetails SET PolicyNo = @PolicyNo WHERE Id = @CarId
END