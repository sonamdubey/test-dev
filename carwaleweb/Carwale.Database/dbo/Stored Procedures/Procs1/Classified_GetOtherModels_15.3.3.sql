IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_GetOtherModels_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_GetOtherModels_15]
GO

	-- =============================================
-- Author:		<kush kumar>
-- Create date: <31/10/2012>
-- Description:	<Procedure to return other model details>
-- Modified By : Ashish G. Kamble on 11 Feb 2013
-- Added : makename, modelname, and cityname for url rewritting
-- Modified By : Ashish G. Kamble on 6 Jan 2014, Added order by clause to get random result set.
-- Modified By : Akansha Srivastava on 12.2.2014
 --Modified by Aditi Dhaybar on 27/2/2015 to get LargePicUrl
  -- Description : Added MaskingName Column
-- Modified By : Purohith Guguloth on 1st june, 2015
  --added condition for the photoCount and changed top's parameter
-- Modified By : Purohith Guguloth on 22nd june, 2015
  --Added "ISNULL(LL.HostURL, '') + LL.ImageUrlMedium As MediumImagePath" in the select statement for 
  --big sized images which we are using in the new deatails page recommended cars section

-- =============================================
CREATE PROCEDURE [dbo].[Classified_GetOtherModels_15.3.3]
	-- Add the parameters for the stored procedure here
	-- Input Parameters
	@CityId Int,
	@ModelId Int,
	@ProfileId VarChar(50) = null,
	-- Added parameters TopCount, PhotoCount for new details page
	@TopCount int=3,  
	@PhotoCount int=0
	AS
	BEGIN 
			SELECT TOP (@TopCount) ProfileId 
			           ,(LL.MakeName + ' ' + LL.ModelName + ' ' + LL.VersionName) AS CarName 
					   , LL.MakeName
					   , LL.ModelName
					   , CMO.MaskingName
					   , C.Name AS CityName 
					   , LL.Price 
					   , LL.Kilometers 
					   , Year(LL.MakeYear) AS MakeYear 
					   , LL.PhotoCount
					   ,isnull(LL.HostURL, '') + LL.FrontImagePath AS ImagePath
					   ,ISNULL(LL.HostURL, '') + LL.ImageUrlMedium As MediumImagePath  --Added by Purohith Guguloth on 22nd june, 2015 for big sized images
					   , LL.EMI
					   , LL.AreaName
					   ,LL.AbsureScore
					   , DL.ActiveMaskingNumber AS MaskingNumber
			FROM LiveListings LL WITH(NOLOCK)
			Inner Join CarModels CMO WITH(NOLOCK)
					ON LL.ModelId=CMO.ID
			LEFT JOIN Cities AS C WITH(NOLOCK)
					ON C.ID = LL.CityId
			LEFT JOIN Dealers DL WITH (NOLOCK) 
					ON LL.DealerId = DL.ID  --Added by Aditi Dhaybar on 27/2/2015 for MAsking Number details
			WHERE LL.CityId = @CityId And LL.ModelId = @ModelId 
			And LL.ProfileId <> @ProfileId 
			AND LL.PhotoCount >= @PhotoCount --Added by Purohith Guguloth on 1st june, 2015 for new details page
			ORDER BY NEWID()	-- Added By Ashish G. Kamble on 6 Jan 2014
		
	END
