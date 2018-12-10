IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_UpdateLead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_UpdateLead]
GO

	CREATE PROCEDURE [dbo].[CRM_UpdateLead]

	@Id				Numeric,
	@CustomerId		Numeric,
	@UpdatedOn		DateTime,
	@LeadStageId	SmallInt,
	@Owner			Numeric,
	@Status			Bit OutPut	
				
 AS
	
BEGIN

	SET @Status = 0
	IF @Id <> -1
			
		BEGIN
			UPDATE CRM_Leads SET UpdatedOn = @UpdatedOn, LeadStageId = @LeadStageId,
			Owner = @Owner WHERE Id = @Id
			
			IF @LeadStageId = 3
				BEGIN
					UPDATE CRM_Customers SET IsActive = 0 WHERE ActiveLeadId = @Id --Id = @CustomerId Commented By Deepak on 17th Oct 2013
				END

			SET @Id = 1
		END
			

END













