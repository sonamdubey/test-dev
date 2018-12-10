IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GenerateRSAPolicyNo]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[Absure_GenerateRSAPolicyNo]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 20th Mar 2015
-- Description:	To Generate RSA PolicyNo
--RCWGD1510000001
--R-RSA
--CW-CarWale
--G-Type of Warranty, Replace with S-Silver, P-Platinum
--D-Dealer, Replace with R for retail customer
--15-Year 2015
--10000001-Serial no. Keep incrementing on every RSA.
-- =============================================
CREATE FUNCTION [dbo].[Absure_GenerateRSAPolicyNo]
(
	@SoldRSAPackagesId INT,
	@IsDealer	BIT
)
RETURNS VARCHAR(50)
AS
BEGIN
	DECLARE @PolicyNo VARCHAR(50)
	,@WarrantyType CHAR
	,@ActivatedType CHAR
	,@SerialNo INT

	SELECT @ActivatedType = CASE @IsDealer
			WHEN 0 -- default dealerId for absure.in
				THEN 'R'
			ELSE 'D'
			END
		,@WarrantyType = CASE 
		WHEN COALESCE(ReqRSAPackageId,TC_AvailableRSAPackagesId) IN( 1, 61)
		THEN 'G'
		WHEN COALESCE(ReqRSAPackageId,TC_AvailableRSAPackagesId) IN (2,62)
			THEN 'S'
		WHEN COALESCE(ReqRSAPackageId,TC_AvailableRSAPackagesId) IN (3,63)
			THEN 'P'
		END
	FROM TC_SoldRSAPackages
	WHERE Id = @SoldRSAPackagesId

	SELECT @SerialNo = PolicyId FROM Absure_RSAPolicy WHERE SoldRSAPackagesId = @SoldRSAPackagesId

	SET @PolicyNo = 'RCW' + @WarrantyType + @ActivatedType + CONVERT(VARCHAR(2), GETDATE(), 11) + CAST(@SerialNo AS VARCHAR)

	return @PolicyNo

END
