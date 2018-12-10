IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CalculateSortScoreForIndividual]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[CalculateSortScoreForIndividual]
GO

	-- =============================================
-- Author:		Supriya Bhide
-- Create date: 24 June 2016
-- Description:	Calculates sortscore for an individual car with given params
-- =============================================
CREATE FUNCTION [dbo].[CalculateSortScoreForIndividual]
(
	@IsPremium BIT,
	@CarScore FLOAT,
	@NewScore FLOAT,
	@PhotoCount INT	
)
RETURNS FLOAT
AS
BEGIN
	-- Declare the return variable here
	DECLARE @maxBucketValue INT = 15,
			@SortScoreNew FLOAT

	-- Add the T-SQL statements to compute the return value here
	SET @SortScoreNew = (CASE(@IsPremium)
							WHEN 1 THEN 3
							WHEN 0 THEN 1
						END) + ISNULL(@NewScore,ABS(@CarScore) - FLOOR(ABS(@CarScore)))
								+ CASE WHEN @PhotoCount > 0 THEN 0 ELSE -@maxBucketValue END

	-- Return the result of the function
	RETURN @SortScoreNew
END
