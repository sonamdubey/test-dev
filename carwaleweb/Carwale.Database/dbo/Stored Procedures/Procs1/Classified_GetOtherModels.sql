IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Classified_GetOtherModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Classified_GetOtherModels]
GO

	-- =============================================
-- Author:		<kush kumar>
-- Create date: <31/10/2012>
-- Description:	<Procedure to return other model details>
-- Modified By : Ashish G. Kamble on 11 Feb 2013
-- Added : makename, modelname, and cityname for url rewritting
-- Modified By : Ashish G. Kamble on 6 Jan 2014, Added order by clause to get random result set.
-- Modified By : Akansha Srivastava on 12.2.2014
  -- Description : Added MaskingName Column

-- =============================================

CREATE PROCEDURE   [dbo].[Classified_GetOtherModels]
	-- Input Parameters
	@CityId BigInt,
	@ModelId BigInt,
	@ProfileId VarChar(50) = NULL
	AS
	BEGIN 
		SELECT TOP 3 ProfileId , (MakeName + ' ' + ModelName + ' ' + VersionName) AS CarName , MakeName, ModelName, CMO.MaskingName, C.Name AS CityName , 
		Price , Kilometers , Year(MakeYear) AS MakeYear , PhotoCount,isnull(LL.HostURL, '')+FrontImagePath AS ImagePath 
		FROM LiveListings LL WITH(NOLOCK)
		LEFT JOIN Cities AS C ON C.ID = LL.CityId
		Inner Join CarModels CMO on LL.ModelId=CMO.ID
		WHERE CityId = @CityId And ModelId = @ModelId And ProfileId <> @ProfileId
		ORDER BY NEWID()	-- Added By Ashish G. Kamble on 6 Jan 2014
	END

