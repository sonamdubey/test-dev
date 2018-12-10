IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[AC].[generic]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [AC].[generic]
GO

	-- =============================================
-- Author:		<Chetan dev >
-- Create date: <05/17/2013>
-- Description:	<Description,,>
/*
	exec [ac].[generic] 'ma','1,2,4',1,1,1
*/
-- =============================================
CREATE PROCEDURE [ac].[generic]
	-- Add the parameters for the stored procedure here
	@FirstTwoLetters VARCHAR(5),
	@TextTypeId VARCHAR(50) = NULL,
	@isNew BIT = NULL,
	@isUsed BIT = NULL,
	@IsPriceExist BIT = NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	SELECT ac.RSC_ExceptSpaces(Name) As n, Value As v, DisplayName As l
		--,IsNew new
		--,IsUsed used
		--,IsPriceExist price
	from ac.SRC_Keywords WITH(NOLOCK)
	WHERE Name like @FirstTwoLetters + '%'
	AND (@TextTypeId IS NULL OR @TextTypeId = '' OR KeywordTypeId IN (SELECT items FROM dbo.SplitTextRS(@TextTypeId,',')))
	AND (@isNew IS NULL OR IsNew = @isNew OR KeywordTypeId IN (7))
	AND (@isUsed IS NULL OR IsUsed = @isUsed OR KeywordTypeId IN (7))
	AND (@IsPriceExist IS NULL OR IsPriceExist = @IsPriceExist OR KeywordTypeId IN (7))
	ORDER BY KeywordTypeId ASC, l ASC
END
