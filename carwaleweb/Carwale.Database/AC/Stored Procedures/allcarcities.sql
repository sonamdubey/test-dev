IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[AC].[allcarcities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [AC].[allcarcities]
GO

	

-- =============================================
-- Author:		Rohan Sapkal
-- Create date: 9-3-2015
-- Description:	Autocomplete suggestion for cities
-- Modified by Rohan 9-4-2015 added clause for Delhi NCR and Mumbai a& Around
-- =============================================
/*
	exec [ac].[allcarcities] 'mu'
*/
CREATE PROCEDURE [ac].[allcarcities]
	-- Add the parameters for the stored procedure here
	@FirstTwoLetters VARCHAR(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- Insert statements for procedure here
	--SELECT * FROM(
	--SELECT  C.ID v, ac.RemoveSpecialCharacters(C.Name) n,C.Name l FROM
	--(SELECT DISTINCT LL.CityId FROM livelistings LL WITH(NOLOCK)) LL
	--INNER JOIN Cities C WITH(NOLOCK) ON LL.CityId = C.ID) T
	--WHERE n LIKE @FirstTwoLetters+'%' ORDER BY l ASC
	SELECT *
	FROM (
		SELECT KW.Value v
			,ac.RSC_ExceptSpaces(KW.NAME) n
			,KW.DisplayName l
		FROM AC.SRC_Keywords KW WITH(NOLOCK)
		WHERE KW.KeywordTypeId = 7
		AND KW.DisplayName <>'Mumbai & Around'	--added by Rohan , 9-4-2015 
		AND KW.DisplayName <>'Delhi NCR'		--
		) T
	WHERE n LIKE @FirstTwoLetters + '%'
	ORDER BY l ASC
END

