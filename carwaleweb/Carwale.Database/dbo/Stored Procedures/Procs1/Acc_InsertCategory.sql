IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_InsertCategory]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_InsertCategory]
GO

	

CREATE PROCEDURE [dbo].[Acc_InsertCategory]
	@CategoryId			VARCHAR(50),
	@CategoryName	 	VARCHAR(100),
	@Status			NUMERIC OUTPUT
 AS
	DECLARE		@CategoryCode		VARCHAR(50),
				@ParentNode		VARCHAR(50),
				@Ancestors		VARCHAR(50),
				@Depth		INT,
				@NextChildNode	INT
BEGIN
	SELECT	@ParentNode		=  Id, 
			@Ancestors 		=  Ancestors , 
			@Depth		= Depth,
			@NextChildNode	= NextChildNode
	
	FROM 		Acc_Categories 
	
	WHERE 	CategoryId = @CategoryId AND IsActive = 1

	IF @@RowCount = 0 -- IF ROWCOUNT IS ZERO THAT MEANS NO ROOT NOTE EXIST. CREATE ROOT NODE FIRST
	
		BEGIN
			SET @Depth = 0
			SET @NextChildNode = 1
			SET @CategoryName = 'Accessories'
			
			INSERT INTO Acc_Categories (CategoryId, CategoryName, NextChildNode, ParentNode, Ancestors, Depth )
			VALUES ( @CategoryId, @CategoryName, @NextChildNode, @ParentNode, @Ancestors, @Depth)
			
			SET @Status = SCOPE_IDENTITY()
		END
	ELSE -- CREATE CHILD OF EXISTING NODE
		BEGIN 
			SET @CategoryCode 	= @CategoryId	+  Convert(VarChar, @NextChildNode)  +'_' 
			SET @Ancestors	= CASE IsNUll(@Ancestors, '0')  WHEN '0' THEN Convert(VarChar, @ParentNode)   ELSE   (@Ancestors + ','+ Convert(VarChar, @ParentNode))  END
			SET @Depth		= @Depth + 1

			Print @Ancestors

			INSERT INTO Acc_Categories (CategoryId, CategoryName, NextChildNode, ParentNode, Ancestors, Depth )
			VALUES ( @CategoryCode, @CategoryName, 1, @ParentNode, @Ancestors, @Depth)	

			UPDATE Acc_Categories SET NextChildNode = @NextChildNode + 1  WHERE CategoryId = @CategoryId
			
			SET @Status = SCOPE_IDENTITY()
		END
END
