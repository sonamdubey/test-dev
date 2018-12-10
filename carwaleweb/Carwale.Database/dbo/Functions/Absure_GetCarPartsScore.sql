IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Absure_GetCarPartsScore]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[Absure_GetCarPartsScore]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 3rd April 2015
-- Description:	To get parts score questionwise
-- =============================================
CREATE FUNCTION [dbo].[Absure_GetCarPartsScore] 
(
	@ResponseIds VARCHAR(100),
	@TempScore decimal(8,3)
)
RETURNS decimal(8,3) 
AS
BEGIN
	DECLARE @TempId INT,@IndxResponse INT

	WHILE @ResponseIds <> ''
	BEGIN
		SET @IndxResponse = CHARINDEX(',', @ResponseIds)

		IF @IndxResponse > 0
			BEGIN
				SET @TempId = LEFT(@ResponseIds, @IndxResponse - 1)
				SET @ResponseIds = RIGHT(@ResponseIds, LEN(@ResponseIds) - @IndxResponse)
			END
		ELSE
			BEGIN
				SET @TempId = @ResponseIds
				SET @ResponseIds = ''
			END

		SELECT	@TempScore = CAST((CAST(WeightagePercent AS decimal)/100)*@TempScore AS decimal(8,3))
		FROM	AbSure_QCarPartResponses WITH(NOLOCK)
		WHERE	AbSure_QCarPartResponsesId=@TempId

	END
	
	RETURN @TempScore

END
