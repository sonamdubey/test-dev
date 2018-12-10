IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Load_Used_Car_Listings_15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Load_Used_Car_Listings_15]
GO

	-- =============================================
-- Author:		Akansha Shrivastava
-- Create date: 2/10/2013 10:17:54 PM
-- Description:	To get recently listed used car
-- Modified By: Akansha on 10.4.2014
-- Description : Added Masking Name Column
-- Modified By: Akansha on 25.03.2014
-- Description : Show cars with image only
-- Modified By : Jugal Singh on 24 July 2014
-- Change suggested car count from 10 to 4
-- Change top count from 4 to 20 - Modified By : Akansha Srivastava
-- Modified By: Arushi Pant on 5 Aug 2015
-- Description: Make Car Count dynamic; Read "OriginalImgPath" value instead of "FrontImagePath" value
-- =============================================
-- Sample Usage: [dbo].[Load_Used_Car_Listings_15.8.1] 20
-- Modified by Manish on 30-12-2015 changed condition DATALENGTH(LL.OriginalImgPath)>0 to LL.OriginalImgPath IS NOT NULL
-- =============================================
CREATE PROCEDURE [dbo].[Load_Used_Car_Listings_15.8.1]
   -- Add Input parameters here
   @CarCount INT = 4 -- By default takes count as 4 (Added by Arushi - 5 Aug 2015)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
	SELECT TOP (@CarCount) LL.MakeName, -- Changed by Arushi on 5 Aug 2015 to make car count dynamic
			LL.ModelName, 
			CMO.MaskingName,
			LL.VersionName, 
			LL.ProfileId, 
			LL.VersionId, 
			LL.CityName, 
			LL.Price, 
			LL.Kilometers, 
			Year(LL.MakeYear) MakeYear, 
			LL.OriginalImgPath, -- Changed by Arushi on 6 Aug 2015 to read new image path
			LL.HostURL
	FROM LiveListings LL WITH(NOLOCK)
	INNER JOIN CarModels CMO WITH(NOLOCK) on LL.ModelId=CMO.ID
	WHERE LL.ShowDetails = 1 --And DATALENGTH(LL.OriginalImgPath)>0
	AND  LL.OriginalImgPath IS NOT NULL ---- Condition changed by Manish on 30-12-2015
	ORDER BY LL.EntryDate DESC     
END
