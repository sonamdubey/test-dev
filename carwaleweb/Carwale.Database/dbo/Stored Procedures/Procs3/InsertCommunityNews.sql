IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertCommunityNews]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertCommunityNews]
GO

	
CREATE PROCEDURE [dbo].[InsertCommunityNews]
	@Id			BIGINT,
	@Title			VARCHAR(200), 
	@Description		VARCHAR(4000),
	@NewsDate		DATETIME,
	@Status		INT OUTPUT		
 AS

BEGIN

	SET @Status = 0

	IF @Id = -1

		BEGIN


			INSERT INTO CommunityNews ( Title, Description, NewsDate ) 
			VALUES( @Title, @Description, @NewsDate )

			SET @Status = 1 
			
			
		END
	ELSE
		
		BEGIN
			UPDATE CommunityNews SET Title=@Title, Description = @Description
			WHERE ID = @Id

			SET @Status = 1 
		END
END
