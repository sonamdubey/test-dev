IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[vwAdv_GetBestVersionDealsByModel]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[vwAdv_GetBestVersionDealsByModel]
GO

	


-- =============================================
-- Author:		<Harshil Mehta>
-- Create date: <03/03/2016>
-- Description:	Get MAxDiscount for each version by giving city and modelid
 --EXEC [vwAdv_GetBestVersionDealsByModel-v16.3.4] 1,1
CREATE PROCEDURE  [dbo].[vwAdv_GetBestVersionDealsByModel] 
	 @ModelId INT
	,@CityId INT
AS
BEGIN

With CTE 
AS
(
	Select  VersionId,Savings,CityId,StockCount,MaskingName, offers,
	ROW_NUMBER() OVER( partition BY versionid ORDER BY savings desc) rno 
	from vwLiveDeals WITH(NOLOCK) where ModelId = @ModelId 
	and (CityId=@CityId or @CityId IS NULL) 

)
select VersionId,Savings,CityId,StockCount,MaskingName, offers from CTE WITH(NOLOCK) where rno=1
END


