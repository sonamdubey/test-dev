IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_GetProposalStatus]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_GetProposalStatus]
GO

	CREATE PROCEDURE [dbo].[ESM_GetProposalStatus]
AS
BEGIN
	SELECT 
		CAST(EPS.Status AS VARCHAR) +' % -' + EPS.Description AS Text,
		EPS.Id AS Value,
		EPS.Description,
		EPS.Status AS Probablity
	FROM 
		ESM_ProposalStatus EPS
	ORDER BY Status
END