IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[CD].[IsVersionDataPresent]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [CD].[IsVersionDataPresent]
GO

	CREATE FUNCTION [CD].[IsVersionDataPresent]
	(@carVersionId INT)
RETURNS BIT
AS
BEGIN
	DECLARE @bitVal BIT
	SET @bitVal = 0
	IF EXISTS(SELECT * FROM CD.ItemValues WHERE CarVersionId = @carVersionId)
	BEGIN
		SET @bitVal = 1
	END
	RETURN @bitVal
END
