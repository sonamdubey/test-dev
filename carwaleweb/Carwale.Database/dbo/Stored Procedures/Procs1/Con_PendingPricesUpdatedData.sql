IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_PendingPricesUpdatedData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_PendingPricesUpdatedData]
GO

	
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <16/7/2014>
-- Description:	<Get Pending prices updated data after file upload>
-- =============================================
CREATE PROCEDURE [dbo].[Con_PendingPricesUpdatedData]
AS
BEGIN
	WITH Prices AS(
	SELECT UP.ModelId,ISNULL(M.Name,'')+' '+ISNULL(CM.Name,'') AS CarName,CT.Name AS CityName,NCS.LastUpdated,UP.CityId,UP.MakeId,UP.FileName,
	ROW_NUMBER()OVER(PARTITION BY UP.ModelId,UP.CityId ORDER BY LastUpdated) AS Row_no , UP.UploadedDate,OU.UserName AS UploadedBy
	, DATEDIFF(dd,NCS.LastUpdated,UP.UploadedDate) DIFF ,UP.HostUrl+'/pricefiles/'+UP.FileName AS FileLocation
	FROM Con_UploadedPricesFiles AS UP 
	LEFT JOIN NewCarShowroomPrices AS NCS  WITH(NOLOCK) ON UP.ModelId = NCS.CarModelId AND UP.CityId = NCS.CityId
	LEFT JOIN CarModels AS CM WITH(NOLOCK) ON CM.ID = UP.ModelId AND CM.New = 1 AND CM.IsDeleted = 0
	LEFT JOIN CarMakes AS M WITH(NOLOCK) ON M.ID = CM.CarMakeId AND M.New = 1 AND M.IsDeleted = 0
	LEFT JOIN Cities AS CT WITH(NOLOCK) ON CT.ID = UP.CityId
	LEFT JOIN OprUsers AS OU WITH(NOLOCK) ON OU.Id = UP.UploadedBy
	)
	SELECT P.CarName,P.CityName,P.LastUpdated,P.UploadedDate,P.FileName AS UploadedFile,P.UploadedBy, P.CityId,P.ModelId,P.MakeId
	,P.DIFF,P.FileLocation
	FROM Prices P 
	LEFT JOIN Con_UploadedPricesFiles C  ON  P.ModelId = C.ModelId AND P.CityId = C.CityId
	WHERE Row_no = 1 AND (DATEDIFF(dd,P.LastUpdated,C.UploadedDate) IS NULL OR DATEDIFF(dd,P.LastUpdated,C.UploadedDate) > 0)
	ORDER BY C.UploadedDate DESC

END
