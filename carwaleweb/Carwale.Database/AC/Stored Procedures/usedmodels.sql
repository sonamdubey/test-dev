IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[AC].[usedmodels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [AC].[usedmodels]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
/*
	EXEC [ac].[usedmodels] 'ma'
*/
CREATE PROCEDURE [ac].[usedmodels]
	-- Add the parameters for the stored procedure here
	@FirstTwoLetters varchar(5)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	--SELECT ac.RemoveSpecialCharacters(n) n,l,v FROM
	--(
	--SELECT CM.Name n, cm.Name l, cm.Name + ':' +CONVERT(varchar, cm.ID) + '|:'   v, 1 s FROM
	--(SELECT DISTINCT MakeId FROM livelistings WITH(NOLOCK)) LL
	--INNER JOIN CarMakes CM WITH(NOLOCK) ON LL.MakeId = CM.ID
	--UNION
	--SELECT
	--	LL.MakeName + ' ' + CM.Name n,
	--	LL.MakeName + ' ' + CM.Name l,
	--	LL.MakeName+':'+ CONVERT(varchar, CM.CarMakeId)+'|'+CM.MaskingName+':'+CONVERT(varchar, CM.ID) v,
	--	2 s
	--FROM (SELECT DISTINCT ModelId,MakeName FROM livelistings LL WITH(NOLOCK)) LL
	--INNER JOIN CarModels CM WITH(NOLOCK) ON LL.ModelId = CM.ID
	--UNION
	--SELECT
	--	CM.Name n,
	--	LL.MakeName + ' ' + CM.Name l,
	--	LL.MakeName+':'+ CONVERT(varchar, CM.CarMakeId)+'|'+CM.MaskingName+':'+CONVERT(varchar, CM.ID) v,
	--	2 s
	--FROM (SELECT DISTINCT ModelId,MakeName FROM livelistings LL WITH(NOLOCK)) LL
	--INNER JOIN CarModels CM WITH(NOLOCK) ON LL.ModelId = CM.ID
	--) T WHERE n LIKE @FirstTwoLetters+'%' ORDER BY s,l ASC

	SELECT ac.RSC_ExceptSpaces(n) n,l,v FROM(

	SELECT KW.Name n, KW.DisplayName l, KW.Value v, 1 s FROM
	(SELECT DISTINCT MakeId FROM livelistings WITH(NOLOCK)) LL
	INNER JOIN AC.SRC_Keywords KW WITH(NOLOCK) ON LL.MakeId = KW.ReferenceId
	WHERE KW.KeywordTypeId = 1
	UNION

	SELECT KW.Name n, KW.DisplayName l, KW.Value v,	2 s
	FROM (SELECT DISTINCT ModelId,MakeName FROM livelistings LL WITH(NOLOCK)) LL
	INNER JOIN AC.SRC_Keywords KW WITH(NOLOCK) ON LL.ModelId = KW.ReferenceId
	WHERE KW.KeywordTypeId IN (2,4)

	)T WHERE n LIKE @FirstTwoLetters+'%' ORDER BY s,l ASC
END
