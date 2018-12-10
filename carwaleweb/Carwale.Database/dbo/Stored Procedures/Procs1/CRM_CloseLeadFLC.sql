IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_CloseLeadFLC]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_CloseLeadFLC]
GO

	

-- =============================================
-- Author:		Deepak
-- Create date: 24th Apr 2012
-- Description:	This proc will close the lead at verification stage in pool
-- =============================================
CREATE PROCEDURE [dbo].[CRM_CloseLeadFLC]
	@LeadId			Numeric,
	@ClosedBy		INT,
	@Reason			VarChar(250)
AS
BEGIN
	
	BEGIN TRANSACTION
	
	UPDATE CRM_Leads SET LeadStageId = 3, LeadStatusId = 4, Owner = 13, UpdatedOn = GETDATE(), LeadVerifier = 13 WHERE ID = @LeadId
	UPDATE CRM_Customers SET IsActive = 0 WHERE ID IN(SELECT CNS_CustId FROM CRM_Leads WHERE ID = @LeadId)
	INSERT INTO CRM_SystemClosedLeads(LeadId, ClosedOn, ClosedBy, Reason) VALUES(@LeadId, GETDATE(), @ClosedBy, @Reason)
	
	COMMIT TRANSACTION
END


