IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[GetColName]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [CD].[GetColName]
GO

	CREATE FUNCTION [CD].[GetColName]
	(@itemMasterId INT)
RETURNS VARCHAR(200)
AS
BEGIN
	DECLARE @value VARCHAR(200)
	SET @value = ''
	BEGIN
		SELECT
			@value = Column_name
		FROM 
			dbo.oldcolmapping WITH(NOLOCK)
		WHERE 
			ItemMasterId = @itemMasterId
	END
	RETURN @value
END
