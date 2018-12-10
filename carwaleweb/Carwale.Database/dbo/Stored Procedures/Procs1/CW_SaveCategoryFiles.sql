IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CW_SaveCategoryFiles]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CW_SaveCategoryFiles]
GO

	-- =============================================
-- Author      : Chetan Navin
-- Create date : 23rd Nov 2015
-- Description : To save uploaded file path details	
-- Modifier  : Kartik Rathod on 12 oct 2016,added UpdatedBy field in insert and update queries
-- =============================================
CREATE PROCEDURE [dbo].[CW_SaveCategoryFiles]
	-- Add the parameters for the stored procedure here
	@FileId         INT = NULL , 
	@CategoryId     TINYINT,
	@ItemId         INT,
	@OriginalPath	VARCHAR(250),
	@UpdatedBy INT = NULL,
	@RetId          INT OUTPUT
AS
BEGIN
	SET NOCOUNT ON;
    -- Insert statements for procedure here
	IF(@FileId IS NULL)
	BEGIN
		INSERT INTO CW_Files (CategoryId,ItemId,CreatedOn,OriginalPath,IsUploaded,CreatedBy)
		VALUES(@CategoryId,@ItemId,GETDATE(),@OriginalPath,0,@UpdatedBy)

		SET @RetId = SCOPE_IDENTITY()
	END
	ELSE
	BEGIN

		UPDATE CW_Files SET IsUploaded = 1,UploadedOn = GETDATE(),OriginalPath = @OriginalPath,UpdatedBy = @UpdatedBy
		WHERE Id = @FileId

		SET @RetId = @@ROWCOUNT

		IF(@RetId > 0 AND @CategoryId=1) --CategoryId = 1|DealerWebsite
		BEGIN		   
				  
			UPDATE	Microsite_ModelBrochures
			SET		GeneratedKey = CASE WHEN @OriginalPath IS NULL THEN GeneratedKey ELSE @OriginalPath END,					
					UpdatedOn = getdate(),UpdatedBy = @UpdatedBy,IsReplicated=1,IsActive=0
			WHERE	Id = @ItemId
		END
	END
END
