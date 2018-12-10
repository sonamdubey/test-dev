IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_InsertItemsFeatures]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_InsertItemsFeatures]
GO

	



CREATE   PROCEDURE [dbo].[Acc_InsertItemsFeatures]
	@ItemId		NUMERIC,
	@FeatureId	NUMERIC,
	@BooleanValue	Bit,
	@NumericValue	Numeric,
	@DecimalValue	Decimal(18,2),
	@TextValue	VarChar(100),
	@ValueType	SmallInt,
	@Status		NUMERIC OUTPUT
 AS
	
BEGIN
	
	SELECT Id FROM  Acc_ItemsFeatures WHERE ItemId = @ItemId  AND FeatureId = @FeatureId

	IF  @@ROWCOUNT =  0
		BEGIN
			INSERT INTO Acc_ItemsFeatures
			( 
				ItemId, FeatureId, BooleanValue, NumericValue, DecimalValue, TextValue, ValueType
			)
			VALUES
			(
				@ItemId, @FeatureId, @BooleanValue, @NumericValue, @DecimalValue, @TextValue, @ValueType
			)
			SET @Status = 1
			
		END
	ELSE
		BEGIN

			UPDATE  Acc_ItemsFeatures SET
			
				ItemId = @ItemId, FeatureId = @FeatureId, BooleanValue = @BooleanValue,
				NumericValue = @NumericValue, DecimalValue = @DecimalValue, TextValue = @TextValue
			WHERE
				ItemId = @ItemId  AND FeatureId = @FeatureId

			SET @Status = 0
		END
END
