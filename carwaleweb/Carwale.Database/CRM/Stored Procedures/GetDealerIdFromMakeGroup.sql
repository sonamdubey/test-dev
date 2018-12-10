IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[GetDealerIdFromMakeGroup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[GetDealerIdFromMakeGroup]
GO

	


--Summary							: Distinct DealerId based on MakeGroup Id
--Author							: Dilip V. 06-Nov-2012
--Modification						: 1. 
CREATE PROCEDURE [CRM].[GetDealerIdFromMakeGroup]
@MakeGroupId NUMERIC(18,0)
AS 
 BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT DealerId 
	FROM NCS_RMDealers AS NRD WITH (NOLOCK)
		INNER JOIN NCS_RManagers NRM WITH (NOLOCK) ON NRD.RMId = NRM.Id 
		AND NRM.MakeGroupId = @MakeGroupId
		
END