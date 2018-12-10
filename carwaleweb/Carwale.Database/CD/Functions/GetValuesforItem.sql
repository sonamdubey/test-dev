IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetValuesforItem]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [CD].[GetValuesforItem]
GO

	CREATE FUNCTION [CD].[GetValuesforItem]
	(@itemMasterId INT)
RETURNS VARCHAR(200)
AS
BEGIN
	DECLARE @values VARCHAR(200)
	SET @values = '';
	IF EXISTS(SELECT * FROM CD.UserDefinedMaster WHERE ItemMasterId = @itemMasterId AND IsActive = 1)
	BEGIN
		SELECT 
			@values = @values + CAST(UserDefinedId AS VARCHAR) + ',' + Name + ',' + CAST(ValueImportance AS VARCHAR) + '|' 
		FROM 
			CD.UserDefinedMaster 
		WHERE 
			ItemMasterId = @itemMasterId
		AND
			IsActive = 1
		ORDER BY ValueImportance
	SET @values = SUBSTRING(@values, 1, LEN(@values)-1)
	END
	RETURN @values
END
