IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[insertCarGalleryCategories]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[insertCarGalleryCategories]
GO

	




CREATE     PROCEDURE [dbo].[insertCarGalleryCategories] 

@Id NUMERIC,
@CategoryName VARCHAR(100),
@IsActive BIT,
@Action VARCHAR(10),
@Status INT OUTPUT		-- Status value 		0- Error		1-Category Exist		2-Saved	3-Deleted

AS
	
BEGIN

	DECLARE @Check NUMERIC
	SET 	@Status = 0

	IF  @Action = 'insert'
		BEGIN
			SELECT @Check = COUNT(*) FROM carGalleryCategories WHERE Name=@CategoryName
			
			IF @Check  =  0 
				BEGIN
					INSERT INTO carGalleryCategories (Name, IsActive)  VALUES (@CategoryName, @IsActive)
					SET	@Status = 2
				END
			ELSE
				SET	@Status = 1
		END

	ELSE IF @Action = 'update'
		BEGIN
			SELECT @Check = COUNT(*) FROM carGalleryCategories WHERE Name=@CategoryName
			
			IF @Check  =  0 
				BEGIN
					UPDATE carGalleryCategories SET Name = @CategoryName WHERE Id = @Id
					SET	@Status = 2
				END
			ELSE
				SET	@Status = 1
		END

	ELSE IF @Action = 'delete'
		BEGIN
			UPDATE carGalleryCategories SET  IsActive = '0'  WHERE Id = @Id
			SET	@Status = 3
		END
	
END
