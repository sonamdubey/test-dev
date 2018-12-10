IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveVerificationCarLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveVerificationCarLog]
GO

	

CREATE PROCEDURE [dbo].[CRM_SaveVerificationCarLog]

	@TeamId				Numeric,
	@LeadId				Numeric,
	@cbdId				Numeric,
	@versionId			Numeric,
	@DealerId			Numeric,
	@IsPD				Bit,
	@IsTD				Bit,
	@IsPQ				Bit,
	@UpdatedBy			Numeric,
	@UpdatedOn			DateTime,
	@Subject			VarChar(500),
	@Comments			VarChar(5000),
	@IsConCall          Bit = NULL,
	@TDDate				DATETIME = NULL,
	@Source             TINYINT = 1,
	@Id                 BIGINT = NULL OUTPUT ,
	@IsFinance			BIT = NULL,
	@IsOffer			BIT = NULL,
	@IsUrgent			BIT = NULL
				
 AS
		DECLARE @ExistSubject VarChar(500)
		DECLARE @CallId Numeric
		
BEGIN
		
		IF @TDDate IS NULL
			SET @TDDate = GETDATE()
			
		INSERT INTO CRM_VerificationLog
			(LeadId, CBDId, DealerId, VersionId, IsPDRequired, IsPQRequired, IsTDRequired, IsConCall, UpdatedOn, UpdatedBy, Comments,TDDate,Source,IsFinance,IsOffer,IsUrgent)
		VALUES(@LeadId, @cbdId, @DealerId, @versionId, @IsPD, @IsPQ, @IsTD, @IsConCall, @UpdatedOn, @UpdatedBy, @Comments ,@TDDate,@Source,@IsFinance,@IsOffer,@IsUrgent)

		SET @Id = SCOPE_IDENTITY()
		
		if @Subject <> ''
			BEGIN	
				SELECT TOP 1 @CallId = ISNULL(CallId, -1), @ExistSubject = subject 
				FROM CRM_CallActiveList WITH(NOLOCK)
				WHERE LeadId = @LeadId AND CallerId = @TeamId
				AND CallType IN(3,4) ORDER BY CallId DESC
		       
				IF @CallId <> -1
					BEGIN
						SET @Subject = @Subject + ' ' + @ExistSubject
						PRINT @Subject
						UPDATE CRM_Calls
						SET Subject = @Subject
						WHERE Id = @CallId
		                
						UPDATE CRM_CallActiveList
						SET Subject = @Subject
						WHERE CallId = @CallId
					END
			END
END















