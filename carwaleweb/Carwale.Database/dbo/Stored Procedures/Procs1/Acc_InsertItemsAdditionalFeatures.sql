IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_InsertItemsAdditionalFeatures]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_InsertItemsAdditionalFeatures]
GO

	

CREATE PROCEDURE [dbo].[Acc_InsertItemsAdditionalFeatures]
	@ItemId			NUMERIC,
	@AdditionalFeaturesId		NUMERIC,
	@Status			NUMERIC OUTPUT
 AS
	
BEGIN
	
	SELECT Id FROM  Acc_ItemsAdditionalFeatures WHERE ItemId = @ItemId  AND AdditionalFeaturesId = @AdditionalFeaturesId

	IF  @@ROWCOUNT =  0
		BEGIN
			INSERT INTO Acc_ItemsAdditionalFeatures
			 ( 
				ItemId, AdditionalFeaturesId
			)
			VALUES
			 (
				@ItemId, @AdditionalFeaturesId
			)
			SET @Status = 1
			
		END
	ELSE
		BEGIN

			UPDATE  Acc_ItemsAdditionalFeatures SET
				ItemId = @ItemId, AdditionalFeaturesId = @AdditionalFeaturesId, IsActive = 1
			WHERE
				ItemId = @ItemId  AND AdditionalFeaturesId = @AdditionalFeaturesId

			SET @Status = 0
		END
END
