IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[PriceQuote_GetDWVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[PriceQuote_GetDWVersions]
GO

	CREATE PROCEDURE [dbo].[PriceQuote_GetDWVersions]

@ModelId INT,

@DealerId INT

AS

--Author:Rakesh Yadav ON 22 Jun 2015

--Desc: Fetch dealer version and corresponding carwale version id whoes prices are avilable in NewCarShowroomPrices

BEGIN

	SELECT DISTINCT DWV.DWVersionName AS Text,DWV.CWVersionId AS Value,DWV.ID AS DWValue FROM 

	TC_DealerVersions DWV WITH (NOLOCK)

	JOIN CarVersions CV WITH (NOLOCK) ON DWV.CWVersionId=CV.ID

	JOIN NewCarShowroomPrices NCP WITH (NOLOCK) ON CV.ID=NCP.CarVersionId

	WHERE DWV.DWModelId=@ModelId AND DWV.DealerId=@DealerId

	AND DWV.IsDeleted=0 AND CV.IsDeleted=0 AND CV.New=1 AND NCP.IsActive=1

END