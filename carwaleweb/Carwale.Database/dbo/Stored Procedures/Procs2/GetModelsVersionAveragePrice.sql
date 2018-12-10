IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetModelsVersionAveragePrice]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetModelsVersionAveragePrice]
GO

	
-- =============================================
-- Author	:	Sachin Bharti
-- Create date: 29/06/2016
-- Description:	Get average prices of all the versions based on modelId
-- exec GetPQByModelCity 862,1
-- =============================================

CREATE PROCEDURE [dbo].[GetModelsVersionAveragePrice]
	 @ModelId INT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT CV.ID AS VersionId
		,CV.NAME AS VersionName
		,CN.AvgPrice AS AveragePrice
	FROM Con_NewCarNationalPrices CN(NOLOCK)
	INNER JOIN CarVersions CV WITH (NOLOCK) ON CV.ID = CN.VersionId --AND CV.IsDeleted = 0 
	WHERE CV.CarModelId = @ModelId
	ORDER BY AveragePrice DESC
END



