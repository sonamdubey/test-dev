IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DLS_SaveFollowup]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DLS_SaveFollowup]
GO

	

CREATE PROCEDURE [dbo].[DLS_SaveFollowup]
	@UpId			NUMERIC,
	@CWCustomerId	NUMERIC,
	@UpdatedOn		DATETIME,
	@UpdatedBy		NUMERIC,
	@lastComment	VARCHAR(150),
	@Comments		VARCHAR(25),
	@NextCallDate	DATETIME,
	@ActionId		INT,
	@FollowupLeadType SMALLINT,
	@CurrentId		NUMERIC OUTPUT
 AS
	
BEGIN
	SET @CurrentId = -1
	IF @UpId = -1
		BEGIN
			
			INSERT INTO DLS_Followups
			(
				CWCustomerId, UpdatedBy, LastComment, 
				Comments, NextCallDate, ActionId, FollowupLeadType
			) 
			VALUES
			( 
				@CWCustomerId, @UpdatedBy, @LastComment, 
				@Comments + @LastComment, @NextCallDate, @ActionId, @FollowupLeadType
			)
			
			SET @CurrentId = SCOPE_IDENTITY() 
		END
	ELSE
		BEGIN 
			UPDATE DLS_Followups 
			SET UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy, 
				LastComment = @LastComment, NextCallDate = @NextCallDate,
				ActionId = @ActionId, Comments = Comments + '<BR>' + @Comments + @LastComment
			WHERE Id = @UpId
			
			SET @CurrentId =@UpId
		END
	
		
	
END


