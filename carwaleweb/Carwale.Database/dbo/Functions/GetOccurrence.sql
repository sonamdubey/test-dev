IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetOccurrence]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[GetOccurrence]
GO

	
-- =============================================
-- Author:		Surendra
-- Create date: 15-12-2011
-- Description:	Return the number of occurrences of characters in the string.
-- To execute select dbo.GetOccurrence('1,2,3,4,5,,',',')
-- =============================================
CREATE FUNCTION [dbo].[GetOccurrence]
(
	-- Add the parameters for the function here
	@Sentence varchar(max),@FindSubString varchar(10)
)
RETURNS int
AS
BEGIN
	-- Declare the return variable here
	
    DECLARE @CntOccurance int
	-- Add the T-SQL statements to compute the return value here
	SELECT @CntOccurance=
    (LEN(@Sentence) - LEN(REPLACE(@Sentence, @FindSubString, '')))/LEN(@FindSubString) 

	-- Return the result of the function
	RETURN @CntOccurance

END

