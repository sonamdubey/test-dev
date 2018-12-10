IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GenerateWarrantyPolicyNo]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[Absure_GenerateWarrantyPolicyNo]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 20th Mar 2015
-- Description:	To Generate Warranty PolicyNo
--CWGD1510000001
--CW-Carwale
--G-Gold Warranty, Replace with S for Silver Warranty.
--D-Dealer, Replace with R for Retail customers(absure.in)
--15-Year 2015, current year.
--Keep incrementing the serial number irrespective of the source and type of warranty.
-- =============================================
CREATE FUNCTION [dbo].[Absure_GenerateWarrantyPolicyNo]
(
	@AbSure_CarId INT,
	@IsDealer BIT,
	@IsCarTrade BIT
)
RETURNS VARCHAR(50)
AS
BEGIN
	DECLARE @PolicyNo VARCHAR(50)
	,@WarrantyType CHAR
	,@ActivatedType CHAR
	,@SerialNo INT

	IF @IsDealer = 0 --Individual eg. absure.in
	BEGIN
		SELECT @ActivatedType = CASE @IsDealer
				WHEN 0 -- for absure.in
					THEN 'R'
				ELSE 'D'
				END
			,@WarrantyType = CASE FinalWarrantyTypeId
				WHEN 1
					THEN 'G'
				WHEN 2
					THEN 'S'
				END
		FROM AbSure_CarDetails WITH(NOLOCK)
		WHERE ID = @AbSure_CarId
	END
	ELSE --Dealer
	BEGIN
		IF(@IsCarTrade <> 1)
		BEGIN
			SELECT @ActivatedType = CASE @IsDealer
					WHEN 0 -- for absure.in
						THEN 'R'
					ELSE 'D'
					END
				,@WarrantyType = CASE WarrantyTypeId
					WHEN 1
						THEN 'G'
					WHEN 2
						THEN 'S'
					END
			FROM AbSure_ActivatedWarranty WITH(NOLOCK)
			WHERE AbSure_CarDetailsId = @AbSure_CarId
		END
		ELSE
		BEGIN
			SET @WarrantyType =  'G'
			SET @ActivatedType = 'D'
		END
	END
	
	SELECT @SerialNo = PolicyId FROM Absure_Policy WITH(NOLOCK) WHERE AbSure_CarId = @AbSure_CarId AND ISNULL(IsCarTradeWarranty,0) = ISNULL(@IsCarTrade,0)

	SET @PolicyNo = 'CW' + @WarrantyType + @ActivatedType + CONVERT(VARCHAR(2), GETDATE(), 11) + CAST(@SerialNo AS VARCHAR)

	return @PolicyNo
END
