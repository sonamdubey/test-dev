IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetAllVersionsDeals]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetAllVersionsDeals]
GO

	-- =============================================
-- Author:		<Sourav>
-- Create date: <29/04/2016>
-- Description:	Get MAxDiscount for all versions of a given  modelid and cityid for deals
-- Exec [dbo].[vwAdv_GetMaxDiscountForAllVersions] 566,1
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_GetAllVersionsDeals]
     @ModelId INT
	,@CityId INT
AS

BEGIN
			WITH CTE AS
			(
					SELECT 
							VersionId
							,FinalOnRoadPrice
							,ActualOnroadPrice
							,Savings
							,Make
							,Model
							,Version
							,ROW_NUMBER() OVER(PARTITION BY VersionId ORDER BY Savings DESC,ActualOnroadPrice DESC) RowNum  
					from vwLiveDeals WITH (NOLOCK)
					where ModelId=@ModelId and CityId=@CityId
		)
		Select Make AS MakeName
					,Model AS ModelName
					,VersionId
					,Version AS VersionName
					,FinalOnRoadPrice AS OfferPrice
					,ActualOnroadPrice AS OnRoadPrice
					,Savings
					FROM CTE WITH(NOLOCK) WHERE RowNum =1
					ORDER BY FinalOnRoadPrice

END