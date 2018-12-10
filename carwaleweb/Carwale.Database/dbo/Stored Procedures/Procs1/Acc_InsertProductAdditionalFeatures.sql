IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_InsertProductAdditionalFeatures]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_InsertProductAdditionalFeatures]
GO

	

CREATE PROCEDURE [dbo].[Acc_InsertProductAdditionalFeatures]
	@ProductId				NUMERIC,
	@FeatureName				VARCHAR(100),
	@AdditionalFeatureId			INTEGER OUTPUT
 AS
	
BEGIN
	SELECT Name FROM Acc_ProductAdditionalFeatures  WHERE Name = @FeatureName AND ProductId = @ProductId

	IF @@RowCount = 0
	
		BEGIN
			INSERT INTO Acc_ProductAdditionalFeatures ( ProductId,  Name) VALUES ( @ProductId, @FeatureName)	
			
			
			SET @AdditionalFeatureId = SCOPE_IDENTITY()
			
		END
	ELSE
		SET @AdditionalFeatureId = -1
	
END
