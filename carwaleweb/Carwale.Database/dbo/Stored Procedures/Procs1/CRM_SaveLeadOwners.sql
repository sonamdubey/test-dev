IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveLeadOwners]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveLeadOwners]
GO

	



CREATE PROCEDURE [dbo].[CRM_SaveLeadOwners]
	@Type			Int,
	@LeadId			Numeric,
	@CBDId			Numeric,
	@CarConsultant		Numeric = null,
	@CustCoordinator	Numeric = null,
	@DealerCoordinator	Numeric = null,
	@ConsultantDate		DateTime = null,
	@CCoordinatorDate	DateTime = null,
	@DCoordinatorDate	DateTime = null,
	@UpdatedOn			DateTIme,
	@CreatedOn			DateTime,
	@UpdatedBy			Numeric
	
 AS
	
BEGIN
		-- Customer Coordinator
		IF @Type = 1
			BEGIN
					UPDATE CRM_LeadCarOwners SET CustCoordinator = @CustCoordinator,
						CCoordinatorDate = @CCoordinatorDate, UpdatedOn = @UpdatedOn, 
						UpdatedBy = @UpdatedBy
					WHERE LeadId = @LeadId AND CBDId = @CBDId
					
					IF @@ROWCOUNT = 0
						BEGIN
							INSERT INTO CRM_LeadCarOwners(LeadId, CBDId, CustCoordinator, 
											CCoordinatorDate, CreatedOn, UpdatedOn, UpdatedBy)
							VALUES(@LeadId, @CBDId, @CustCoordinator, 
											@CCoordinatorDate, @CreatedOn, @UpdatedOn, @UpdatedBy)
						END
			END
				
		-- Car Consultant
		ELSE IF @Type = 2
			BEGIN
				UPDATE CRM_LeadCarOwners SET CarConsultant = @CarConsultant,
					ConsultantDate = @ConsultantDate, UpdatedOn = @UpdatedOn, 
					UpdatedBy = @UpdatedBy
				WHERE LeadId = @LeadId AND CBDId = @CBDId
				
				IF @@ROWCOUNT = 0
					BEGIN
						INSERT INTO CRM_LeadCarOwners(LeadId, CBDId, CarConsultant, 
										ConsultantDate, CreatedOn, UpdatedOn, UpdatedBy)
						VALUES(@LeadId, @CBDId, @CarConsultant, 
										@ConsultantDate, @CreatedOn, @UpdatedOn, @UpdatedBy)
					END
				
			END
			
		-- Dealer Coordinator
		ELSE IF @Type = 3
			BEGIN
				UPDATE CRM_LeadCarOwners SET DealerCoordinator = @DealerCoordinator,
					DCoordinatorDate = @DCoordinatorDate, UpdatedOn = @UpdatedOn, 
					UpdatedBy = @UpdatedBy
				WHERE LeadId = @LeadId AND CBDId = @CBDId
				
				IF @@ROWCOUNT = 0
					BEGIN
						INSERT INTO CRM_LeadCarOwners(LeadId, CBDId, DealerCoordinator, 
										DCoordinatorDate, CreatedOn, UpdatedOn, UpdatedBy)
						VALUES(@LeadId, @CBDId, @DealerCoordinator, 
										@DCoordinatorDate, @CreatedOn, @UpdatedOn, @UpdatedBy)
					END
			END
END
















