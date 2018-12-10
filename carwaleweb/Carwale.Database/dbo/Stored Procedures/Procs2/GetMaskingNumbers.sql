IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetMaskingNumbers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetMaskingNumbers]
GO

	-- =============================================
-- Author:		Ashwini Todkar
-- Create date: 9 July 2015
-- Description: Proc to get assigned and unassigned masking numbers for a dealer
-- Modified By : Vaibhav k. On 24th June 2016, Get service provider name in available masking number.
-- EXEC GetMaskingNumbers 18584, 1
-- Modified By : Mihir Chheda On 16 Aug 2016, Get masking numbers based on states as well.
-- =============================================
CREATE PROCEDURE [dbo].[GetMaskingNumbers] @DealerId INT,
@CityId INT,
@StateId INT = NULL -- Mihir Chheda On 16 Aug 2016

AS
BEGIN

	SELECT CAST(MN.MM_SellerMobileMaskingId AS varchar) AS Value, MN.MaskingNumber AS TEXT, 1 As Assigned
	FROM MM_SellerMobileMasking MN WITH(NOLOCK)
	INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = MN.ConsumerId
	WHERE ConsumerId = @DealerId
	UNION ALL
	-- SELECT ID AS Value, MaskingNumber AS Text, 0 As Assigned FROM MM_AvailableNumbers
	SELECT MaskingNumber AS Value, MaskingNumber + (CASE ServiceProvider WHEN 1 THEN ' - Knowlarity' WHEN 2 THEN ' - Solutions Infini' END) AS Text , 0 As Assigned
	FROM MM_AvailableNumbers WITH(NOLOCK) 
	WHERE CityID = ISNULL(@CityId,CityId) AND StateId =ISNULL(@StateId,StateId) -- Mihir Chheda On 16 Aug 2016
	
END


