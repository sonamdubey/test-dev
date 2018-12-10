IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CRM].[GridQAsubHead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [CRM].[GridQAsubHead]
GO

		CREATE PROCEDURE [CRM].[GridQAsubHead]
	@HeadId  INT
    AS
    BEGIN
		 SELECT CQSH.SubheadName,CQSH.HeadId,OU.UserName AS CreatedBy,CQH.HeadName,Weight,CQSH.CreatedOn,CQH.RoleId, 
         OU1.UserName AS UpdatedBy,CQSH.UpdatedOn,CQSH.Id,CQSH.IsActive ,
         CASE CQSH.Type WHEN '1' THEN 'Numeric' WHEN '2' THEN 'Option' WHEN '3' THEN 'N/A' ELSE '' END Type,CQSH.Type AS TypeValue FROM CRM.QASubhead AS CQSH 
         INNER JOIN CRM.QAHeads AS CQH ON CQH.Id= CQSH.HeadId 
         INNER JOIN OprUsers AS OU ON CQSH.CreatedBy= OU.Id 
         INNER JOIN OprUsers AS OU1 ON CQSH.UpdateBy= OU1.Id 
         WHERE CQSH.HeadId=@HeadId 
         ORDER BY CQSH.CreatedOn DESC	
	END
	
	