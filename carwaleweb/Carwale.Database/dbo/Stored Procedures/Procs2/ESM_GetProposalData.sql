IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_GetProposalData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_GetProposalData]
GO

	-- =============================================
-- Author:		Amit Yadav
-- Create date: 20th August,2015
-- =============================================
CREATE PROCEDURE [dbo].[ESM_GetProposalData]
	@ClientId AS  NUMERIC(18,0)
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT ESP.id,
		   ESP.Title AS Proposal, 
		   ESB.BrandName AS Brand,
		   ESP.ProposedAmount AS Amount,
		   EON1.OrgName AS Agency,		   
		   ESP.Remark AS Remark, 
		   ISNULL(ESP.Probability,0) AS ProbabilityId,
		   ISNULL(EPS.Status,0) AS Probability,
		   ISNULL(OU.UserName,'') AS Name,
		   ISNULL(SUM(ESP.ProposedAmount),0) AS TotalAmount,
		   ESP.OrgId AS OrgId,
		   ESP.ClientId AS ClientId
		   
		   FROM
		   ESM_Proposal AS ESP WITH(NOLOCK)
		   LEFT JOIN ESM_Brands AS ESB WITH(NOLOCK) ON ESB.id = ESP.BrandId
		   LEFT JOIN ESM_Users AS EU WITH(NOLOCK) ON EU.UserId=ESP.ESMUserId
		   LEFT JOIN OprUsers AS OU WITH(NOLOCK) ON OU.Id=EU.UserId
		   LEFT JOIN ESM_OrganizationName AS EON1 WITH(NOLOCK) ON EON1.id = ESP.OrgId
		   LEFT JOIN ESM_ProposalStatus AS EPS WITH(NOLOCK) ON EPS.Id =ESP.Probability

		   WHERE ESP.ClientId = @ClientId 
		   AND ESP.IsDeleted IS NULL OR ESP.IsDeleted <> 1
		   Group by ESP.id,ESP.Title,ESB.BrandName,ESP.ProposedAmount,EON1.OrgName,ESP.Remark,ESP.Probability,OU.UserName,ESP.ProposedAmount,ESP.OrgId,ESP.ClientId,EPS.Status
END
