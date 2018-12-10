IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_GetMaskingNumbers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_GetMaskingNumbers]
GO

	
-- =============================================
-- Author:		Sumit Kate
-- Create date: 04 Apr 2016
-- Description: Proc to get assigned and unassigned masking numbers for a dealer
-- EXEC [BW_GetMaskingNumbers] 4
-- =============================================
CREATE PROCEDURE [dbo].[BW_GetMaskingNumbers] @DealerId INT
AS
BEGIN
	DECLARE @CityId INT
	SELECT @CityId = CityId FROM Dealers WITH(NOLOCK) WHERE ID = @DealerId
	SELECT MN.MM_SellerMobileMaskingId AS Value, MN.MaskingNumber AS TEXT, 1 As Assigned
	FROM MM_SellerMobileMasking MN WITH(NOLOCK)
	INNER JOIN Dealers D WITH(NOLOCK) ON D.ID = MN.ConsumerId
	WHERE ConsumerId = @DealerId
	UNION ALL
	SELECT ID AS Value, MaskingNumber AS Text, 0 As Assigned FROM MM_AvailableNumbers WITH(NOLOCK) 
	WHERE CityID = @CityId  
END
