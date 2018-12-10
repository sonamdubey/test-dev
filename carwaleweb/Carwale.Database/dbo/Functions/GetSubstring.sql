IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetSubstring]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[GetSubstring]
GO

	
-- =============================================
-- Author:		AVISHKAR
-- Create date: 15-12-2011
-- SELECT [dbo].[GetSubstring]('abc12345', 'abc1')
-- =============================================
CREATE FUNCTION [dbo].[GetSubstring]
(
	-- Add the parameters for the function here
	@Sentence varchar(max),@FindSubString varchar(10)
)
RETURNS varchar(max)
AS
BEGIN
	-- Declare the return variable here
	
    DECLARE @RSentence varchar(max)=@Sentence
    DECLARE @RSentencelen int
    SET @RSentencelen= LEN(@FindSubString)
    IF(SUBSTRING(@Sentence,1,@RSentencelen)=@FindSubString)
    BEGIN
	-- Add the T-SQL statements to compute the return value here
	SELECT @RSentence=
    STUFF(@Sentence, 1,@RSentencelen, '')
    END
	-- Return the result of the function
	RETURN @RSentence

END

