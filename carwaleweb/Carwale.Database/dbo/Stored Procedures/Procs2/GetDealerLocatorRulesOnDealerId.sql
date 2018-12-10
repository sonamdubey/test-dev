IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealerLocatorRulesOnDealerId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealerLocatorRulesOnDealerId]
GO

	
-- =============================================
-- Author:		Vinayak
-- Create date: <25/07/2016>
-- Description:	Get Dealer Locator Rules
-- exec [dbo].[GetDealerLocatorRulesOnDealerId] 20570
-- =============================================
create PROCEDURE [dbo].[GetDealerLocatorRulesOnDealerId] @DealerId INT
	
AS
BEGIN
	SET NOCOUNT ON;

	SELECT D.StateId,D.CityId,TDM.MakeId,DNC.DealerId as DealerId--,TDM.MakeId
	FROM DealerLocatorConfiguration DNC WITH (NOLOCK) 
	INNER JOIN Dealers D WITH (NOLOCK) ON D.ID = DNC.DealerId
	INNER JOIN TC_DealerMakes TDM WITH (NOLOCK) ON TDM.DealerId = D.ID
	WHERE DNC.DealerId = @DealerId
END

