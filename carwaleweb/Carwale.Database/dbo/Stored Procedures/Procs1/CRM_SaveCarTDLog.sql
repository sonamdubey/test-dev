IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CRM_SaveCarTDLog]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CRM_SaveCarTDLog]
GO

	


--Summary: Fetch Details of Test Drive
--Author:
--Modifier: Dilip V. 22-Mar-2012 (Added TD Direct Completed)
-- Modifier: Deepak Tripathi - 19th Apr 12(If TD Direct Make IsPQRequested 0)
-- Modifier: Amit Kumar - 1st nov 2012(Added comment)

CREATE PROCEDURE [dbo].[CRM_SaveCarTDLog]

	@Id					Numeric,
	@CBDId				Numeric,
	@CreatedBy			Numeric,
	@UpdatedBy			Numeric,
	@TDNPDispositionId	Numeric,
	
	@IsTDRequested		Bit,
	@IsTDCompleted		Bit,
	@ISTDNotPossible	Bit,
	@ISTDDirect			Bit,
	
	@TDRequestDate		DateTime = NULL,
	@TDCompleteDate		DateTime,
	@UpdatedOn			DateTime,
	@CurrentId			Numeric OutPut,	
	@TDComment			Varchar(500) = NULL
				
 AS
	
BEGIN
	SET @CurrentId = -1
	
	IF @TDRequestDate IS NULL
			SET @TDRequestDate = GETDATE()
			
	IF @Id = -1 AND @IsTDRequested = 1
				
		BEGIN

			INSERT INTO CRM_CarTDLog
			(
				CBDId, IsTDRequested, TDRequestDate, CreatedBy, TDComment
			)
			VALUES
			(
				@CBDId, @IsTDRequested, @TDRequestDate, @CreatedBy, @TDComment
			)
			
			SET @CurrentId = SCOPE_IDENTITY()
		
			IF @ISTDDirect = 1
				BEGIN
					UPDATE CRM_CarTDLog 
					SET IsTDCompleted = 0, ISTDNotPossible = 0, IsTDRequested = 0, IsTDDirect = 1, 
						TDCompleteDate = ISNULL(@TDCompleteDate,@UpdatedOn), UpdatedBy = @UpdatedBy,
						UpdatedOn = @UpdatedOn, TDNPDispositionId = @TDNPDispositionId,TDComment = @TDComment
					WHERE Id = @CurrentId
					
				END
		
		END

	ELSE
		
		BEGIN
			IF @IsTDRequested = 1
				BEGIN
					UPDATE CRM_CarTDLog 
					SET TDRequestDate = @TDRequestDate, UpdatedBy = @UpdatedBy,
						UpdatedOn = @UpdatedOn , TDComment = @TDComment
					WHERE Id = @Id
					
					SET @CurrentId = @Id
				END
				
			IF @IsTDCompleted = 1
				BEGIN
					UPDATE CRM_CarTDLog 
					SET IsTDCompleted = 1, ISTDNotPossible = 0, IsTDDirect = 0,
						TDCompleteDate = ISNULL(@TDCompleteDate,@UpdatedOn), UpdatedBy = @UpdatedBy,
						UpdatedOn = @UpdatedOn , TDComment = @TDComment
					WHERE Id = @Id
					
					SET @CurrentId = @Id
				END			
			ELSE IF @ISTDNotPossible = 1
				BEGIN
					UPDATE CRM_CarTDLog 
					SET IsTDCompleted = 0, ISTDNotPossible = 1, IsTDDirect = 0,
						TDCompleteDate = ISNULL(@TDCompleteDate,@UpdatedOn), UpdatedBy = @UpdatedBy,
						UpdatedOn = @UpdatedOn, TDNPDispositionId = @TDNPDispositionId , TDComment = @TDComment
					WHERE Id = @Id
					
					SET @CurrentId = @Id
				END
			ELSE IF @ISTDDirect = 1
				BEGIN
				PRINT ('UPDATE TD DIRECT');
					UPDATE CRM_CarTDLog 
					SET IsTDCompleted = 0, ISTDNotPossible = 0, IsTDRequested = 0, IsTDDirect = 1,
						TDCompleteDate = ISNULL(@TDCompleteDate,@UpdatedOn), UpdatedBy = @UpdatedBy,
						UpdatedOn = @UpdatedOn, TDNPDispositionId = @TDNPDispositionId , TDComment = @TDComment
					WHERE Id = @Id
					
					SET @CurrentId = @Id
				END
		END
	
END




















