IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_InsertUpdateScoreBoardMetric]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_InsertUpdateScoreBoardMetric]
GO

	

-- =============================================
-- Author	:	Sachin Bharti(11th May 2015)
-- Description	:	Procedure used to bind business unit with products
-- =============================================
CREATE PROCEDURE [dbo].[DCRM_InsertUpdateScoreBoardMetric]
	
	@MetricId		INT = NULL,
	@MetricName		VARCHAR(200) = NULL,
	@BusinessUnitId	INT = NULL,
	@InquiryPointId	INT = NULL,
	@TargetType		TINYINT = NULL,
	@IsActive		BIT = NULL,
	@UpdatedBy		INT = NULL,
	@IsUpdated		BIT OUTPUT

AS
	BEGIN
	
		SET @IsUpdated = 0
		IF @MetricId IS NULL
			BEGIN
				INSERT	INTO DCRM_ExecScoreBoardMetric
								( MetricName,	BusinessUnitId,	InquiryPointId, TargetType,IsActive, AddedBy, AddedOn	)
							VALUES
								( @MetricName,	@BusinessUnitId, @InquiryPointId, @TargetType,@IsActive, @UpdatedBy, GETDATE()	)
				IF @@ROWCOUNT = 1
					SET @IsUpdated = 1
			END
		ELSE IF (@MetricId IS NOT NULL AND @MetricId > 0)
			BEGIN
				UPDATE 
					DCRM_ExecScoreBoardMetric	SET MetricName = @MetricName,
												UpdatedBy = @UpdatedBy,
												UpdatedOn = GETDATE()
				WHERE
					Id = @MetricId

				IF @@ROWCOUNT = 1
					SET @IsUpdated = 1
			END
	END

