IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_InsertProductFeatureCategories]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_InsertProductFeatureCategories]
GO

	
-- PROCEDUTRE TO 

CREATE PROCEDURE [dbo].[Acc_InsertProductFeatureCategories]
	@ProductId				NUMERIC,
	@ProductFeatureCategoryName		VARCHAR(100),
	@FeatureCategoryId			INTEGER OUTPUT
 AS
	
BEGIN
	SELECT Name FROM Acc_ProductFeatureCategories  WHERE Name = @ProductFeatureCategoryName AND ProductId = @ProductId

	IF @@RowCount = 0
	
		BEGIN
			INSERT INTO Acc_ProductFeatureCategories ( ProductId,  Name) VALUES ( @ProductId, @ProductFeatureCategoryName)	
			
			
			SET @FeatureCategoryId = SCOPE_IDENTITY()
			
		END
	ELSE
		SET @FeatureCategoryId = -1
	
END
