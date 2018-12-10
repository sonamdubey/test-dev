IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_InsertProductFeatures]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_InsertProductFeatures]
GO

	



CREATE   PROCEDURE [dbo].[Acc_InsertProductFeatures]
	@FeatureCategoryId			NUMERIC,
	@FeatureName				VARCHAR(100),
	@ValueType				SmallInt,
	@Priority				SmallInt,
	@IsVariant				Bit,
	@ProductFeatureId			INTEGER OUTPUT
 AS
	
BEGIN
	SELECT Name FROM Acc_ProductFeatures  WHERE Name = @FeatureName AND FeatureCategoryId = @FeatureCategoryId
		
	IF @@RowCount = 0
	
		BEGIN
			INSERT INTO Acc_ProductFeatures ( FeatureCategoryId,  Name, ValueType, IsVariant, Priority) 
			VALUES ( @FeatureCategoryId, @FeatureName, @ValueType, @IsVariant, @Priority)	
			
			SET @ProductFeatureId = SCOPE_IDENTITY()
		END
	ELSE
		SET @ProductFeatureId = -1
	
END
