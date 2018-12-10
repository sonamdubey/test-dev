IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_GetWarrantyPrice]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_GetWarrantyPrice]
GO

	-- =============================================
-- Author:		Ashwini Dhamankar
-- Create date: Feb 26,2015
-- Description:	To Fetch Warranty Prices
-- Modified By: Ashwini Dhamankar on March 2,2015,Added Parameters @ModelId,@WarrantyTypeId
-- Modified By: Ashwini Dhamankar on March 9,2015,Added Parameters @ServiceTaxValue
-- Modified By: Yuga Hatolkar on March 23rd, 2015, Added Parameter @EligibleModelFor and fetched data respectively.
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_GetWarrantyPrice] 
	@ModelId INT,
	@WarrantyTypeId TINYINT,
	@ServiceTaxValue DECIMAL(18,2) = NULL,
	@EligibleModelFor	TINYINT = 1  -- 1: By default Warranty
AS
BEGIN
	SET NOCOUNT ON;
	SELECT DISTINCT	EM.ModelId ID,V.Model Name,
					--CAST(EM.GoldPrice + (EM.GoldPrice * 12.36/100) As decimal(10, 2)) GoldPrice,
					--CAST(EM.SilverPrice + (EM.SilverPrice * 12.36/100) As decimal(10, 2)) SilverPrice,
					CASE WHEN @WarrantyTypeId = 1 THEN CAST(EM.GoldPrice + (EM.GoldPrice * @ServiceTaxValue/100) As decimal(10, 0)) ELSE
					CASE WHEN @WarrantyTypeId = 2 THEN CAST(EM.SilverPrice + (EM.SilverPrice * @ServiceTaxValue/100) As decimal(10, 0)) END 
					END WarrantyPrice
	FROM			AbSure_EligibleModels EM WITH(NOLOCK)
	INNER JOIN		vwMMV V ON V.ModelId = EM.ModelId
	WHERE			EM.IsActive = 1 AND EM.ModelId = @ModelId
					AND (
						( @EligibleModelFor = 1 AND ISNULL(EM.IsEligibleWarranty,1) = 1) OR 
						( @EligibleModelFor = 2 AND EM.IsEligibleCertification = 1) OR 
						( @EligibleModelFor = 3 AND (EM.IsEligibleCertification = 1 AND EM.IsEligibleWarranty = 1))
					)
END


