IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[NCD_GetLeadPanelDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[NCD_GetLeadPanelDealer]
GO

	-- =============================================
-- Author	:	Sachin Bharti(24th Sep 2014)
-- Description	:	Get lead panel dealer from Dealer_NewCar 
--					table for Dealer Locator Page
-- =============================================
CREATE PROCEDURE [dbo].[NCD_GetLeadPanelDealer]
	
	@Dealer_NewCarId	INT

AS
BEGIN
	
	SET NOCOUNT ON;

	SELECT 
		D.Organization ,
		D.ID
	FROM 
		Dealer_NewCar DNC(NOLOCK) 
		INNER JOIN Dealers D(NOLOCK) ON D.ID = DNC.LeadPanelDealerId
		AND DNC.Id = @Dealer_NewCarId
    
END
