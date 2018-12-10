IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Forum_InsertStickyThread]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Forum_InsertStickyThread]
GO

	CREATE Procedure [dbo].[Forum_InsertStickyThread]

@ID NUMERIC,
@ThreadId NUMERIC,
@CatId SMALLINT,
@CreatedBy NUMERIC,
@CreatedDate DATETIME,
@TempID NUMERIC OUTPUT 

AS
	
BEGIN
	IF @ID = -1
	IF NOT EXISTS (SELECT ID FROM Forum_StickyThreads
		WHERE ThreadId=@ThreadId)
		BEGIN
			INSERT INTO Forum_StickyThreads (ThreadId,CatId,CreatedBy,CreatedDate) 
			VALUES (@ThreadId,@CatId,@CreatedBy,@CreatedDate)
			SET @TempID = SCOPE_IDENTITY() 
		END
END

