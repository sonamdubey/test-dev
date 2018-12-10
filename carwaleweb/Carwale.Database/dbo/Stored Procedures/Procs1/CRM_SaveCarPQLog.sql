IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveCarPQLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveCarPQLog]
GO

	
CREATE PROCEDURE [dbo].[CRM_SaveCarPQLog]

	@Id					Numeric,
	@CBDId				Numeric,
	@CreatedBy			Numeric,
	@UpdatedBy			Numeric,
	@PQNRDispositionId	Numeric,
	
	@IsPQRequested		Bit,
	@IsPQCompleted		Bit,
	@IsPQNotRequired	Bit,
	
	@PQRequestDate		DateTime,
	@PQCompleteDate		DateTime,
	@UpdatedOn			DateTime,
	@CurrentId			Numeric OutPut	
				
 AS
	
BEGIN
	SET @CurrentId = -1
	
	IF @Id = -1 AND @IsPQRequested = 1

		BEGIN

			INSERT INTO CRM_CarPQLog
			(
				CBDId, IsPQRequested, PQRequestDate, CreatedBy
			)
			VALUES
			(
				@CBDId, @IsPQRequested, @PQRequestDate, @CreatedBy
			)
			
			SET @CurrentId = SCOPE_IDENTITY()
		
		END

	ELSE
		
		BEGIN
			IF @IsPQRequested = 1
				BEGIN
					UPDATE CRM_CarPQLog 
					SET PQRequestDate = @PQRequestDate, UpdatedBy = @UpdatedBy,
						UpdatedOn = @UpdatedOn
					WHERE Id = @Id
					
					SET @CurrentId = @Id
				END
				
			IF @IsPQCompleted = 1
				BEGIN
					UPDATE CRM_CarPQLog 
					SET IsPQCompleted = 1, IsPQNotRequired = 0, 
						PQCompleteDate = ISNULL(@PQCompleteDate,@UpdatedOn), UpdatedBy = @UpdatedBy,
						UpdatedOn = @UpdatedOn
					WHERE Id = @Id
					
					SET @CurrentId = @Id
				END
			
			IF @IsPQNotRequired = 1
				BEGIN
					UPDATE CRM_CarPQLog 
					SET IsPQCompleted = 0, IsPQNotRequired = 1, 
						PQCompleteDate = ISNULL(@PQCompleteDate,@UpdatedOn), UpdatedBy = @UpdatedBy,
						UpdatedOn = @UpdatedOn, PQNRDispositionId = @PQNRDispositionId
					WHERE Id = @Id
					
					SET @CurrentId = @Id
				END
		END
	
END













