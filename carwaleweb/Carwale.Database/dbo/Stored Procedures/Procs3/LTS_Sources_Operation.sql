IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[LTS_Sources_Operation]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[LTS_Sources_Operation]
GO

	CREATE PROCEDURE [dbo].[LTS_Sources_Operation]
(  
	@ID AS BIGINT = 0,
	@SourceName AS VARCHAR(100) = NULL,   
	@SourceCode AS VARCHAR(50) = NULL,
	@CreatedBy AS BIGINT = 0,
	@UpdatedBy AS BIGINT = 0,
	@UpdatedDate AS DATETIME,
	@ISACTIVE AS BIT = 1,
	@opr AS BIGINT = 0
)  
AS  
BEGIN  
   
IF(@opr = 1 )
	BEGIN
		INSERT INTO 
			   LTS_Sources (Name, ShortCode, CreatedBy, UpdatedDate)  
			   VALUES(@SourceName,@SourceCode,@CreatedBy,@UpdatedDate)  
	END
ELSE IF(@opr = 2 )
	BEGIN
		UPDATE LTS_Sources 
		SET Name = @SourceName, ShortCode =  @SourceCode, UpdatedBy = @UpdatedBy, UpdatedDate = @UpdatedDate, IsActive = @ISACTIVE 
		WHERE ID = @ID
	END
END

