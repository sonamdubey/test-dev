IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetAdvantageCities-v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetAdvantageCities-v16]
GO

	-- =============================================
-- Author:		Anchl gupta
-- Create date: 12-04-2016
-- Description:	Get all ther cities details having deals
--Exec [dbo].[vwAdv_GetAdvantageCities]
-- =============================================
Create PROCEDURE [dbo].[vwAdv_GetAdvantageCities-v16.4.7]
	@modelId int = 0 
	,@versionId int = 0 
AS
BEGIN
    if @versionId > 0
	select distinct CityId, CityName from   vwLiveDeals WITH (NOLOCK) where VersionId = @versionId Order by CityName
	Else
	Begin
	  if @modelId > 0 
	   select distinct CityId, CityName from   vwLiveDeals WITH (NOLOCK) where ModelId = @modelId Order by CityName
	 else
	   select distinct CityId, CityName from   vwLiveDeals WITH (NOLOCK) Order by CityName 
	End
END

