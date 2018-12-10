IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UP_InsertTags]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UP_InsertTags]
GO

	

CREATE PROCEDURE [dbo].[UP_InsertTags]
	@PhotoId			BIGINT,
	@TagName			VARCHAR(200)
	
 AS
	DECLARE @TagId NUMERIC

BEGIN


	BEGIN
		SELECT @TagId=TagId FROM PhotoTags WHERE TagName = @TagName
		
		IF @TagId IS NULL
		
			BEGIN

				INSERT INTO PhotoTags ( TagName ) 
				VALUES( @TagName )
			
				Set @TagId = Scope_Identity()	
			END

		INSERT INTO UP_Tags ( PhotoId,  TagId ) 
		VALUES( @PhotoId, @TagId )

	END
			
END
