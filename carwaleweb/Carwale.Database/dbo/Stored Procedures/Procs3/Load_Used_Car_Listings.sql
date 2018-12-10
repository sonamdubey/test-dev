IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Load_Used_Car_Listings]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Load_Used_Car_Listings]
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
-- =============================================
CREATE PROCEDURE [dbo].[Load_Used_Car_Listings]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	SELECT TOP 4 MakeName, -- Modified by Jugal 30-07-2014 to change Top 10 from Top 4 to show only 4 cars
			ModelName, 
			CMO.MaskingName,
			VersionName, 
			ProfileId, 
			VersionId, 
			CityName, 
			Price, 
			Kilometers, 
			Year(MakeYear) MakeYear, 
			FrontImagePath,
			LL.HostURL
	FROM LiveListings LL WITH(NOLOCK)
	Inner Join CarModels CMO WITH(NOLOCK) on LL.ModelId=CMO.ID
	WHERE	ShowDetails = 1 And DATALENGTH(LL.FrontImagePath)>0
	ORDER BY EntryDate DESC     
END




