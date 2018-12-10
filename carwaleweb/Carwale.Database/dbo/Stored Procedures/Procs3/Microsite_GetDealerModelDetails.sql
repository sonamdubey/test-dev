IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_GetDealerModelDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_GetDealerModelDetails]
GO

	
-- =============================================

-- Author:		<HARSH PATEL>

-- Create date: <1 JUNE 2015>

-- Description:	<TO FETCH MODELS DETAILS OF A DEALER>
-- Modified by: Rakesh yadav on 15 jul 2015 CarMakes,CarModels,CarVersions join is replaced by view vwMicrositeMMV
--Modified by:Rakesh Yadav on 11 Aug 2015, fetch hostUrl and OriginalImgPath from TC_DealerModels else from CarModels(ie from vwMicrositeMMV) 
-- =============================================

CREATE PROCEDURE [dbo].[Microsite_GetDealerModelDetails]

	

	@CarMakeId INT,

	@DealerId INT,

	@CityId INT = 10, -- default city as Delhi

	@type TINYINT = 1



AS

BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from

	-- interfering with SELECT statements.

	SET NOCOUNT ON;



    IF(@type = 1)

	BEGIN



		SELECT DM.ID AS Value,
		CASE
			WHEN (DM.DWModelName IS NOT NULL) 
			THEN DM.DWModelName 
			ELSE CMO.Name 
		END 
		Text 

		FROM TC_DealerModels DM WITH (NOLOCK) JOIN CarModels CMO WITH (NOLOCK) ON DM.CWModelId = CMO.ID AND CMO.IsDeleted = 0

		WHERE DM.IsDeleted = 0 AND DM.DealerId = @DealerId AND CMO.CarMakeId = @CarMakeId

		ORDER BY Text ASC



	END



	ELSE IF(@type = 2)

	BEGIN

		SELECT CASE 
		WHEN (DM.DWModelName IS NOT NULL)
			THEN DM.DWModelName
		ELSE (vmm.Model)
		END ModelName
	,MIN(PQN.PQ_CategoryItemValue) AS ExShowroomPrice ,vmm.ModelId AS CWModelId ,DM.ID AS DWModelId
	,CASE 
		WHEN (
				DM.HostUrl IS NOT NULL
				AND DM.ImgName IS NOT NULL
				AND DM.ImgPath IS NOT NULL
				)
			THEN 'http://' + DM.HostUrl + DM.ImgPath + DM.ImgName
		ELSE ('http://' + vmm.HostURL + vmm.LargePic)
		END ModelImgUrl
		,CASE
			WHEN DM.HostUrl IS NOT NULL AND DM.OriginalImgPath IS NOT NULL
			THEN DM.HostUrl 
			ELSE vmm.HostURL
		END HostUrl
		,CASE
			WHEN DM.HostUrl IS NOT NULL AND DM.OriginalImgPath IS NOT NULL
			THEN DM.OriginalImgPath
			ELSE vmm.OriginalImgPath
		END OriginalImgPath
	,vmm.Make AS MakeName,CT.NAME AS CityName,BS.Id AS BodyStyleId,BS.BodyStyleName AS BodyStyleName
FROM TC_DealerModels DM WITH (NOLOCK)
JOIN vwMicrositeMMV vmm WITH (NOLOCK) ON vmm.ModelId=DM.CWModelId
JOIN CW_NewCarShowroomPrices PQN WITH (NOLOCK) ON PQN.CarVersionId = vmm.VersionId
JOIN PQ_CategoryItems CI WITH (NOLOCK) ON CI.Id = PQN.PQ_CategoryItem
JOIN Cities CT WITH (NOLOCK) ON CT.ID = PQN.CityId
LEFT JOIN Microsite_ModelBodyStyles BS WITH (NOLOCK) ON DM.DWBodyStyleId = BS.Id	AND BS.IsActive = 1
WHERE vmm.MakeId = @CarMakeId	AND CI.CategoryId = 3	AND PQN.CityId = @CityId AND DM.DealerId = @DealerId	AND DM.IsDeleted = 0
GROUP BY vmm.ModelId	,DM.DWModelName	,DM.HostUrl	,DM.ImgName	,DM.ImgPath	,DM.ID	,CT.NAME	,vmm.Model,vmm.HostURL	,vmm.LargePic	,vmm.Make	,BS.Id	,BS.BodyStyleName,DM.OriginalImgPath,vmm.OriginalImgPath
ORDER BY ExShowroomPrice ASC

	

	END

END
