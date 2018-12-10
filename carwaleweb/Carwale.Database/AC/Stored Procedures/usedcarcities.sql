IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[AC].[usedcarcities]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [AC].[usedcarcities]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
/*
	exec [ac].[usedcarcities] 'mu'
*/
CREATE PROCEDURE [ac].[usedcarcities]
	-- Add the parameters for the stored procedure here
	@FirstTwoLetters varchar(5)
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

	SELECT * FROM(
	SELECT KW.Value v,ac.RSC_ExceptSpaces(KW.Name) n,KW.DisplayName l FROM
	(SELECT DISTINCT LL.CityId FROM livelistings LL WITH(NOLOCK)) LL
	INNER JOIN AC.SRC_Keywords KW ON LL.CityId = KW.ReferenceId WHERE KW.KeywordTypeId = 7) T
	WHERE n LIKE @FirstTwoLetters+'%' ORDER BY l ASC
END
