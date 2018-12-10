IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveLoanBasicData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveLoanBasicData]
GO

	CREATE PROCEDURE [dbo].[CRM_SaveLoanBasicData]

	@LoanId					Numeric,
	@LeadId					Numeric,
	@CarVersionId			Numeric,
	@PQId					Numeric,
	@LoanAmount				Numeric,
	@LoanTenure				Numeric,
	@EMI					Numeric,
	@FinancerId				Numeric,
	@UpdatedById			Numeric,
	@CreatedOn				DateTime,
	@UpdatedOn				DateTime,
	@InterestedInId			Numeric,
	@NewLoanId				Numeric OutPut
				
 AS
	DECLARE 
		@LoanDataId		Numeric

BEGIN
	SET @NewLoanId = -1
	IF @LoanId = -1
		BEGIN
			SELECT @LoanDataId = Id FROM CRM_LoanData WHERE LeadId = @LeadId AND FinancerId = @FinancerId
			IF @@ROWCOUNT <> 0

				BEGIN
					INSERT INTO CRM_LoanCarDetails
					(
						LoanDataId, VersionId, PQId, LoanAmount, LoanTenure, EMI, 
						CreatedOn, UpdatedOn, UpdatedBy
					)
					VALUES
					(
						@LoanDataId, @CarVersionId, @PQId, @LoanAmount, @LoanTenure, @EMI,
						@CreatedOn, @UpdatedOn, @UpdatedById
					)
					
					SET @NewLoanId = @LoanDataId
				END
			
			ELSE
				
				BEGIN
					INSERT INTO CRM_LoanData
					(
						LeadId, FinancerId, UpdatedBy, CreatedOn, UpdatedOn
					)
					VALUES
					(
						@LeadId, @FinancerId, @UpdatedById, @CreatedOn, @UpdatedOn
					)
					
					SET @NewLoanId = SCOPE_IDENTITY()

					INSERT INTO CRM_LoanCarDetails
					(
						LoanDataId, VersionId, PQId, LoanAmount, LoanTenure, EMI, 
						CreatedOn, UpdatedOn, UpdatedBy
					)
					VALUES
					(
						@NewLoanId, @CarVersionId, @PQId, @LoanAmount, @LoanTenure, @EMI,
						@CreatedOn, @UpdatedOn, @UpdatedById
					)
				END
		END
	
		IF @NewLoanId <> -1

			BEGIN
				SELECT Priority FROM CRM_ActiveItems 
				WHERE InterestedInId = @InterestedInId AND ItemId = @NewLoanId
				
				IF @@ROWCOUNT = 0
					BEGIN

						INSERT INTO CRM_ActiveItems
							(InterestedInId, ItemId, Priority) 
						VALUES
							(@InterestedInId, @NewLoanId, 5)
					END
			END
END









