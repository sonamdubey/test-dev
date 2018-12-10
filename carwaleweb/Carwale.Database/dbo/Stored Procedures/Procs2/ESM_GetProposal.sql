IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_GetProposal]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_GetProposal]
GO

	-- =============================================
-- Author	:	Ajay Singh(8rd August 2015)
-- Description	:	Get data for Repeater 
-- Modifier : Amit Yadav(1st oct 2015)
-- Description : Correction made related to Account Manager
--Modifier  : Ajay Singh(17  nov 2015)
--Description: To Get Order By Lastupdated Date
-- =============================================

CREATE PROCEDURE [dbo].[ESM_GetProposal] 
@IsROReceive AS BIT ,
@OrgId AS INT =NULL,
@Status AS INT = NULL,
@AcntMgrId AS INT = NULL

AS
BEGIN
	SELECT 
		ESP.id,
		ISNULL(EON1.OrgName,'Direct Client') AS AgencyName,
		EON2.OrgName AS OrgName,
		ESP.ClientId AS OrgId, 
		ESP.Title, 
		ESB.BrandName,
		ESP.Remark AS Status,
		ISNULL(EPS.Status,0) AS Probability1,
		ISNULL(ESP.Probability,0) AS Probability, 
		CASE WHEN ISNULL(ESP.LastProbabilityUpdated,0) <= 0 THEN 0 ELSE ESP.LastProbabilityUpdated END  AS LastProbabilityUpdated, 
		ESP.LastUpdatedOn AS UpdatedDate,
		ESP.ExpectedClosingDate, ISNULL(OU.UserName,'') AS Name, 
		ESP.ESMUserId,
		ISNULL(ESP.ProposedAmount,0) AS ProposedAmount
	FROM 
		ESM_Proposal ESP WITH(NOLOCK)
	LEFT JOIN ESM_ProposalStatus EPS(NOLOCK) ON EPS.Id = ESP.Probability
	LEFT JOIN ESM_OrganizationName EON1 WITH(NOLOCK) ON EON1.id = ESP.OrgId
	LEFT JOIN ESM_OrganizationName AS EON2 WITH(NOLOCK) ON EON2.id=ESP.ClientId
	LEFT JOIN ESM_Brands ESB WITH(NOLOCK) ON ESB.id = ESP.BrandId
	LEFT JOIN OprUsers AS OU WITH(NOLOCK) ON OU.Id=EON2.AccountManager  
						
	WHERE 
		((@IsROReceive = 1 AND ESP.Probability >= 100 ) OR (@IsROReceive <> 1 AND ISNULL(ESP.Probability,0) < 100))
		AND (@OrgId IS NULL OR ESP.ClientId = @OrgId)
		AND (@Status IS NULL OR ESP.Probability= @Status)         
	    AND	(@AcntMgrId IS NULL OR EON2.AccountManager = @AcntMgrId)
		AND ESP.IsDeleted IS NULL OR ESP.IsDeleted <> 1
	ORDER BY ESP.LastUpdatedOn DESC,Title

END


