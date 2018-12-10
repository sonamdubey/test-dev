IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[Forum_InsertStickyThread]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[Forum_InsertStickyThread]
GO

	--Created By : Ravi Koshal
--Script Date: 12/27/2013 3:19:26 PM 
CREATE procedure [cw].[Forum_InsertStickyThread]

@ID NUMERIC,
@ThreadId NUMERIC,
@CatId SMALLINT,
@CreatedBy NUMERIC,
@TempID NUMERIC OUTPUT 

AS
	
BEGIN
	IF @ID = -1
	IF NOT EXISTS (SELECT ID FROM Forum_StickyThreads
		WHERE ThreadId=@ThreadId)
		BEGIN
			INSERT INTO Forum_StickyThreads (ThreadId,CatId,CreatedBy,CreatedDate) 
			VALUES (@ThreadId,@CatId,@CreatedBy,GETDATE())
			SET @TempID = SCOPE_IDENTITY() 
		END
END


