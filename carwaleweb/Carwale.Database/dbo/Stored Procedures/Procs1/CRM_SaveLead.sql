IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveLead]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveLead]
GO

	CREATE PROCEDURE [dbo].[CRM_SaveLead]

	@Id				Numeric,
	@CustomerId		Numeric,
	@CreatedOn		DateTime,
	@UpdatedOn		DateTime,
	@LeadStageId	SmallInt,
	@Priority		SmallInt,
	@Owner			Numeric,
	@NewLeadId		Numeric OutPut,
	@IsActiveUpdate	BIT = 1	
				
 AS
	
BEGIN
	SET @NewLeadId = -1
	IF @Id = -1

		BEGIN

			INSERT INTO CRM_Leads
			(
				CNS_CustId, CreatedOn, UpdatedOn, LeadStageId, Priority, Owner
			)
			VALUES
			(
				@CustomerId, @CreatedOn, @UpdatedOn, @LeadStageId, @Priority, @Owner
			)
			
			SET @NewLeadId = SCOPE_IDENTITY()
			
			--Update Lead sceduling Slot 
			--UPDATE CRM_Leads SET SchedulingSlot = (CASE(Id%2) WHEN 0 THEN 0 ELSE 6 END) WHERE ID = @NewLeadId
			--Added one more slot of 10 minute - 20 May 2013
			UPDATE CRM_Leads SET SchedulingSlot = (CASE(Id%3) WHEN 0 THEN 0 WHEN 1 THEN 6 ELSE 10 END) WHERE ID = @NewLeadId
			
			IF @IsActiveUpdate = 1
				BEGIN
					UPDATE CRM_Customers SET IsActive = 1, ActiveLeadId = @NewLeadId, ActiveLeadDate = @CreatedOn
					WHERE ID = @CustomerId
				END
		END

	ELSE
		
		BEGIN 
			UPDATE CRM_Leads SET UpdatedOn = @UpdatedOn, Priority = Priority + @Priority
			WHERE Id = @Id
				
			SET @NewLeadId = @Id
		END
END












