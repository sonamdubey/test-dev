IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_InsertProducts]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_InsertProducts]
GO

	
-- PROCEDUTRE TO 

CREATE PROCEDURE [dbo].[Acc_InsertProducts]
	@ProductName		VARCHAR(100),
	@ProductId		INTEGER OUTPUT
 AS
	
BEGIN
	SELECT  ProductName FROM Acc_Products WHERE ProductName = @ProductName

	IF @@RowCount = 0
	
		BEGIN
			INSERT INTO Acc_Products ( ProductName ) VALUES ( @ProductName)	
			
			
			SET @ProductId = SCOPE_IDENTITY()
		END
	ELSE
		SET @ProductId = -1
	
END
