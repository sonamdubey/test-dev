IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetAllVersionsDeals-v16_6_2]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetAllVersionsDeals-v16_6_2]
GO

	-- =============================================
-- Author:		<Sourav>
-- Create date: <29/04/2016>
-- Description:	Get MAxDiscount for all versions of a given  modelid and cityid for deals
-- Modified Date: 09-06-2016
-- Modification Description - Added fuelType and transmission Type.
-- Exec [dbo].[vwAdv_GetMaxDiscountForAllVersions] 566,1
-- Modified by Purohith guguloth on 18th july
-- Modification added a column StockId in the selection space
-- =============================================
CREATE PROCEDURE [dbo].[vwAdv_GetAllVersionsDeals-v16_6_2]
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
							,FuelType      
							,Transmission
							,StockId        -- Modified by Purohith guguloth on 18th july
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
					,FuelType
					,Transmission AS TransmissionType
					,StockId                             -- Modified by Purohith guguloth on 18th july
					FROM CTE WITH(NOLOCK) WHERE RowNum =1
					ORDER BY FinalOnRoadPrice

END



/****** Object:  StoredProcedure [dbo].[GetCompareCarsWidget_v16_7_1]    Script Date: 7/18/2016 4:06:52 PM ******/
SET ANSI_NULLS ON
