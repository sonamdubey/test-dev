IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetDealerAdditionalDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetDealerAdditionalDetails]
GO
	/* 

EXEC DCRM_GetDealerAdditionalDetails '1'

Created By: Vicky Lund

Created On: 01/10/2015

*/
CREATE PROCEDURE [dbo].[DCRM_GetDealerAdditionalDetails] @DealerId VARCHAR(100)
AS
BEGIN
	SELECT TOP 1 [ID]
		,[CityId]
		,[StateId]
		,[TC_DealerTypeId]
	FROM [dbo].[Dealers] WITH (NOLOCK)
	WHERE ID = @DealerId
END
