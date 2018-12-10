IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_ModelStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_ModelStatus]
GO

	
-- =============================================
-- Author		: Nilima More
-- Created Date : 18th Jan 2016.
-- Description  : To get model Status.
-- EXEC [TC_Deals_ModelStatus] 
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_ModelStatus] 
AS
BEGIN
	SELECT  DISTINCT DMS.TC_ModelId,DSP.CityId,VW.Make,VW.Model
	FROM DCRM_Deals_ModelStatus DMS WITH(NOLOCK)
	INNER JOIN vwAllMMV VW WITH(NOLOCK) ON VW.ModelId= DMS.TC_ModelId
	INNER JOIN TC_Deals_Stock DS WITH(NOLOCK) ON DS.CarVersionId = VW.VersionId 
	INNER JOIN TC_Deals_StockPrices DSP WITH(NOLOCK) ON DSP.TC_Deals_StockId = DS.Id
 
END


--------------------------------------------------------------------------------

