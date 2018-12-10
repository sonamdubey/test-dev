IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_getDealsbyDiscount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_getDealsbyDiscount]
GO

	-- =============================================
-- Author:		Purohith Guguloth	
-- Create date: 16th march, 2016
-- Description:	This SP Gets the cars with top savings from vwlivedeals table of a particular City
-- Modified By: Purohith Guguloth on 29th march, 2016
--                Changed the column in the partition from ModelId to RootId
-- modified BY : Purohith on 4/5/2016 added StockCount column in order by clause added ActualOnRoadPrice order by asc order 
-- Modified by - Mukul Bansal, added savings > 0 condition.
-- =============================================
--exec vwAdv_getDealsbyDiscount 1, 15
CREATE PROCEDURE [dbo].[vwAdv_getDealsbyDiscount]
	@CityId INT = 1,
	@NoOfCars INT 
AS
BEGIN
With CTE
AS
(	
	SELECT 
		Savings,
		CityId,
		CityName,
		Make,
		MakeId,
		Model,
		ModelId,	
		MaskingName,
		ActualOnroadPrice,
		FinalOnRoadPrice,
		StockCount,
		HostURL,
		OriginalImgPath,
		Offers,
		ROW_NUMBER() OVER(PARTITION BY RootID ORDER BY Savings DESC,ActualOnRoadPrice ASC, StockCount DESC) RowNum  -- modified BY : Purohith on 4/5/2016 added StockCount column in order by clause & added ActualOnRoadPrice order by asc order 
 	FROM vwLiveDeals WITH(NOLOCK) 
	WHERE CityId = @CityId AND Savings > 0
)
SELECT TOP (@NoOfCars) * 
FROM CTE WITH(NOLOCK)
WHERE RowNum=1 
Order by (CONVERT(DECIMAL(16,4), Savings)/ActualOnroadPrice )*100 desc

END




