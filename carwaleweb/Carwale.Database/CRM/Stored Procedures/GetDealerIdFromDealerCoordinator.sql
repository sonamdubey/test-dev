IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[GetDealerIdFromDealerCoordinator]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[GetDealerIdFromDealerCoordinator]
GO

	


--Summary							: Distinct DealerId based on Dealer Coordinator Id
--Author							: Dilip V. 06-Nov-2012
--Modification						: 1. 
CREATE PROCEDURE [CRM].[GetDealerIdFromDealerCoordinator]
@DealerCoordinator NUMERIC(18,0)
AS 
 BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT DealerId 
	FROM CRM_ADM_DCDealers WITH (NOLOCK) 
	WHERE DCID = @DealerCoordinator
		
END