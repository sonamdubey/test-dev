IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_GetNewCarDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_GetNewCarDealers]
GO

	-- =============================================
-- Author	:	Sachin Bharti(11th July 2014)
-- Description	:	Get New Car Dealers
-- =============================================
CREATE PROCEDURE [dbo].[NCD_GetNewCarDealers]
	
AS
BEGIN
	
	SET NOCOUNT ON;
	SELECT NDS.ID, NDS.Name 
	FROM Dealer_NewCar NDS(NOLOCK)
	INNER JOIN NCD_Dealers ND(NOLOCK) ON NDS.Id = ND.DealerID AND ND.TCDealerId IS NOT NULL AND ND.TCDealerId <> -1 AND ND.TCDealerId <> 0
	ORDER BY Name
END
