IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveCarPELog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveCarPELog]
GO

	

CREATE PROCEDURE [dbo].[CRM_SaveCarPELog]

	@Id					Numeric,
	@CBDId				Numeric,
	@CreatedBy			Numeric,
	@UpdatedBy			Numeric,
	@PENRDispositionId	Numeric,
	
	@IsPERequested		Bit,
	@IsPECompleted		Bit,
	@IsPENotRequired	Bit,
	
	@PERequestDate		DateTime,
	@PECompleteDate		DateTime,
	@UpdatedOn			DateTime,
	@CurrentId			Numeric OutPut	
				
 AS
	
BEGIN
	SET @CurrentId = -1
	
	IF @Id = -1 AND @IsPERequested = 1

		BEGIN

			INSERT INTO CRM_CarPELog
			(
				CBDId, IsPERequested, PERequestDate, CreatedBy
			)
			VALUES
			(
				@CBDId, @IsPERequested, @PERequestDate, @CreatedBy
			)
			
			SET @CurrentId = SCOPE_IDENTITY()
		
		END

	ELSE
		
		BEGIN
			IF @IsPERequested = 1
				BEGIN
					UPDATE CRM_CarPELog 
					SET PERequestDate = @PERequestDate, UpdatedBy = @UpdatedBy,
						UpdatedOn = @UpdatedOn
					WHERE Id = @Id
					
					SET @CurrentId = @Id
				END
				
			IF @IsPECompleted = 1
				BEGIN
					UPDATE CRM_CarPELog 
					SET IsPECompleted = 1, IsPENotRequired = 0, 
						PECompleteDate = ISNULL(@PECompleteDate,@UpdatedOn), UpdatedBy = @UpdatedBy,
						UpdatedOn = @UpdatedOn
					WHERE Id = @Id
					
					SET @CurrentId = @Id
				END
			
			IF @IsPENotRequired = 1
				BEGIN
					UPDATE CRM_CarPELog 
					SET IsPECompleted = 0, IsPENotRequired = 1, 
						PECompleteDate = ISNULL(@PECompleteDate,@UpdatedOn), UpdatedBy = @UpdatedBy,
						UpdatedOn = @UpdatedOn, PENRDispositionId = @PENRDispositionId
					WHERE Id = @Id
					
					SET @CurrentId = @Id
				END
		END
	
END














