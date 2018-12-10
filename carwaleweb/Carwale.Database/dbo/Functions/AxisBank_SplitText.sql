IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AxisBank_SplitText]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[AxisBank_SplitText]
GO

	
CREATE FUNCTION [dbo].[AxisBank_SplitText](@String varchar(8000), @Delimiter char(1))       
returns @temptable TABLE (items varchar(8000))       
as       

BEGIN
	DECLARE @idx INT
	DECLARE @slice VARCHAR(8000)

	SELECT @idx = 1

	IF len(@String) < 1
		OR @String IS NULL
		RETURN

	WHILE @idx != 0
	BEGIN
		SET @idx = charindex(@Delimiter, @String)

		IF @idx != 0
			SET @slice = left(@String, @idx - 1)
		ELSE
			SET @slice = @String

		IF (len(@slice) > 0)
			INSERT INTO @temptable (Items)
			VALUES (convert(numeric(18,0),@slice))

		SET @String = right(@String, len(@String) - @idx)

		IF len(@String) = 0
			BREAK
	END
 
return       
end 
