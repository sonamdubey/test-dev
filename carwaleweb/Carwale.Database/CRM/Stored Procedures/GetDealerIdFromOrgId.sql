IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[GetDealerIdFromOrgId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[GetDealerIdFromOrgId]
GO

	
--Summary							: Get DealerId Based on Organisation Id
--Impact							: SP : [CRM].[WeeklyReport_Event],
--Author							: Dilip V. 05-Nov-2012
--Modification						: 1. 
CREATE PROCEDURE [CRM].[GetDealerIdFromOrgId]
	@OrganisationId		BIGINT	
AS
BEGIN
	
	SET NOCOUNT ON;
	
	SELECT DISTINCT ND.Id AS DealerId
	FROM NCS_SubDealerOrganization AS NSDO WITH (NOLOCK)
		INNER JOIN NCS_Dealers ND WITH (NOLOCK) ON NSDO.DID = ND.ID
    WHERE NSDO.OID = @OrganisationId AND ND.IsActive = 1
   
END
