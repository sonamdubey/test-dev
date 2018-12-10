IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[AC].[valuationmodels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [AC].[valuationmodels]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
/*
	EXEC [ac].[valuationmodels] 'ma',2011
*/
CREATE PROCEDURE [ac].[valuationmodels]
	-- Add the parameters for the stored procedure here
	@FirstTwoLetters varchar(5),
	@Year INT = null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	DECLARE @V TABLE (ID INT,y INT)
	INSERT INTO @V SELECT DISTINCT CarVersionId,CarYear FROM CarValues
	SELECT ac.RSC_ExceptSpaces(n) n,l,v FROM (
	SELECT DISTINCT CMA.Name n, CMA.Name l, y, '-1' v,1 s FROM @V V
	INNER JOIN CarVersions CV WITH(NOLOCK) ON V.ID = CV.ID
	INNER JOIN CarModels CM WITH(NOLOCK) ON CV.CarModelId = CM.ID
	INNER JOIN CarMakes CMA WITH(NOLOCK) ON CM.CarMakeId = CMA.ID
	UNION
	SELECT DISTINCT CMA.Name + ' ' + CM.Name n, CMA.Name + ' ' + CM.Name l,y, '-1' v,2 s FROM @V V
	INNER JOIN CarVersions CV WITH(NOLOCK) ON V.ID = CV.ID
	INNER JOIN CarModels CM WITH(NOLOCK) ON CV.CarModelId = CM.ID
	INNER JOIN CarMakes CMA WITH(NOLOCK) ON CM.CarMakeId = CMA.ID
	UNION
	SELECT DISTINCT CM.Name n, CMA.Name + ' ' + CM.Name l,y, '-1' v,3 s FROM @V V
	INNER JOIN CarVersions CV WITH(NOLOCK) ON V.ID = CV.ID
	INNER JOIN CarModels CM WITH(NOLOCK) ON CV.CarModelId = CM.ID
	INNER JOIN CarMakes CMA WITH(NOLOCK) ON CM.CarMakeId = CMA.ID
	UNION
	SELECT DISTINCT CMA.Name + ' ' + CM.Name + ' ' + cv.Name n,
	CMA.Name + ' ' + CM.Name + ' ' + cv.Name l,
	CA.CarYear y,
	CMA.Name + ':' +  CONVERT(varchar, CMA.ID) + '|' + CM.MaskingName + ':' +  CONVERT(varchar, CM.ID) + '|' + cv.Name + ':' +  CONVERT(varchar, CV.ID) v,
	4 s
	FROM CarValues CA WITH(NOLOCK)
	INNER JOIN CarVersions CV WITH(NOLOCK) ON CA.CarVersionId = CV.ID
	INNER JOIN CarModels CM WITH(NOLOCK) ON CV.CarModelId = CM.ID
	INNER JOIN CarMakes CMA WITH(NOLOCK) ON CM.CarMakeId = CMA.ID
	UNION
	SELECT DISTINCT CM.Name + ' ' + cv.Name n,
		CMA.Name + ' ' + CM.Name + ' ' + cv.Name l,
		CA.CarYear y,
		CMA.Name + ':' +  CONVERT(varchar, CMA.ID) + '|' + CM.MaskingName + ':' +  CONVERT(varchar, CM.ID) + '|' + cv.Name + ':' +  CONVERT(varchar, CV.ID) v,
		5 s
	FROM CarValues CA WITH(NOLOCK)
	INNER JOIN CarVersions CV WITH(NOLOCK) ON CA.CarVersionId = CV.ID
	INNER JOIN CarModels CM WITH(NOLOCK) ON CV.CarModelId = CM.ID
	INNER JOIN CarMakes CMA WITH(NOLOCK) ON CM.CarMakeId = CMA.ID
	) T WHERE y = @Year AND n LIKE @FirstTwoLetters+'%' ORDER BY s,l ASC
END
