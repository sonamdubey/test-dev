IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[fnSplitCSVMAx]') 
    AND xtype IN (N'FN', N'IF', N'TF')
)
    DROP FUNCTION [dbo].[fnSplitCSVMAx]
GO

	

-- =============================================
-- Author:		Vicky Lund
-- Create date: 19/05/2016
-- =============================================
CREATE FUNCTION [dbo].[fnSplitCSVMAx] (@NumberList VARCHAR(MAX))
RETURNS @SplitList TABLE (ListMember INT)
AS
BEGIN
	DECLARE @Pointer INT
		,@ListMember VARCHAR(25)

	SET @NumberList = LTRIM(RTRIM(@NumberList))

	IF (RIGHT(@NumberList, 1) != ',')
		SET @NumberList = @NumberList + ','
	SET @Pointer = CHARINDEX(',', @NumberList, 1)

	IF REPLACE(@NumberList, ',', '') <> ''
	BEGIN
		WHILE (@Pointer > 0)
		BEGIN
			SET @ListMember = LTRIM(RTRIM(LEFT(@NumberList, @Pointer - 1)))

			IF (@ListMember <> '')
				INSERT INTO @SplitList (ListMember)
				VALUES (convert(INT, @ListMember))

			SET @NumberList = RIGHT(@NumberList, LEN(@NumberList) - @Pointer)
			SET @Pointer = CHARINDEX(',', @NumberList, 1)
		END
	END

	RETURN
END


