IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[GetModelFromOrgId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[GetModelFromOrgId]
GO

	
--Summary							: Get DISTINCT Model Id and Name From Organisation Id
--Author							: Dilip V. 07-Nov-2012
--Modification						: 1. 
CREATE PROCEDURE [CRM].[GetModelFromOrgId]
	@OrgId	BIGINT
AS 
BEGIN
	SET NOCOUNT ON;
	
	SELECT DISTINCT CMO.Id AS ID, CMO.Name
	FROM CarModels AS CMO 
	INNER JOIN  CarVersions AS CV ON  CV.CarModelId = CMO.ID
	INNER JOIN CRM_CarBasicData AS CBD ON CBD.VersionId=CV.ID
	INNER JOIN CRM_CarDealerAssignment AS CDA ON CDA.CBDId = CBD.Id
	INNER JOIN NCS_SubDealerOrganization AS NSDO ON NSDO.DId = CDA.DealerId
	WHERE CMO.IsDeleted = 0 
	AND NSDO.OId = @OrgId
	ORDER BY CMO.Name
	
END