IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetItemValue]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [CD].[GetItemValue]
GO

	CREATE FUNCTION [CD].[GetItemValue]
	(@itemMasterId INT, @carVersionId INT)
RETURNS VARCHAR(200)
AS
BEGIN
	DECLARE @value VARCHAR(200)
	SET @value = '-1'
	IF EXISTS(SELECT * FROM CD.ItemValues WHERE ItemMasterId = @itemMasterId AND CarVersionId = @carVersionId)
	BEGIN
		SELECT
			@value = COALESCE(CAST(itemValue AS VARCHAR(200)), CAST(userDefinedId AS VARCHAR(200)), CustomText)
		FROM 
			CD.ItemValues WITH(NOLOCK)
		WHERE 
			ItemMasterId = @itemMasterId
		AND
			CarVersionId = @carVersionId
	END
	RETURN @value
END
