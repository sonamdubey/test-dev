IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_MoveItems]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_MoveItems]
GO

	
-- PROCEDUTRE TO 

CREATE PROCEDURE [dbo].[Acc_MoveItems]
	@CategoryId			NUMERIC,
	@ItemId			NUMERIC
 AS
	
BEGIN
	INSERT INTO Acc_ItemCategory(CategoryId, ItemId) VALUES(@CategoryId, @ItemId)
END
