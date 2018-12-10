IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_ItemCategory_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_ItemCategory_SP]
GO

	

CREATE PROCEDURE [dbo].[Acc_ItemCategory_SP]
	@CategoryId			NUMERIC,
	@ItemId			NUMERIC
 AS
	
BEGIN
	
	SELECT CategoryId FROM Acc_ItemCategory WHERE CategoryId=@CategoryId AND  ItemId = @ItemId 

	IF  @@ROWCOUNT =  0
		BEGIN
			INSERT INTO Acc_ItemCategory
			 ( 
				CategoryId, ItemId
			)
			VALUES
			 (
				@CategoryId, @ItemId
			)
		END
END
