IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[BindQAHead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[BindQAHead]
GO

	CREATE PROCEDURE [CRM].[BindQAHead]

	@QARolesId	INT
	
	AS 
	BEGIN
		SELECT CQH.HeadName,CQH.RoleId,OU.UserName as CreatedBy,QR.Name,CQH.TotalWeight,CQH.CreatedOn,
        OU1.UserName AS UpdatedBy,CQH.UpdatedOn,CQH.Id,CQH.IsActive  FROM CRM.QAHeads AS CQH
        INNER JOIN CRM.QARoles AS QR ON QR.Id= CQH.RoleId
        INNER JOIN OprUsers AS OU ON CQH.CreatedBy= OU.Id
        INNER JOIN OprUsers AS OU1 ON CQH.UpdatedBy= OU1.Id
        WHERE CQH.RoleId=@QARolesId
        ORDER BY CQH.CreatedOn DESC
	END