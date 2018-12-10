IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetStringExceptFirst]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[GetStringExceptFirst]
GO

	---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
-- =============================================
-- Author:		Avishkar
-- Create date: 20-12-2011
-- Description:	Return string except first char of the string.
-- To execute select dbo.GetOccurrence('1,2,3,4,5,,',',')
-- =============================================
CREATE FUNCTION [dbo].[GetStringExceptFirst]
(
	-- Add the parameters for the function here
	@String varchar(100)
)
RETURNS varchar(100)
AS
BEGIN
	-- Declare the return variable here
	
    DECLARE @SubString varchar(100)
	SELECT @SubString= substring(@String, 2,LEN(@String))

	-- Return the result of the function
	RETURN @SubString

END

