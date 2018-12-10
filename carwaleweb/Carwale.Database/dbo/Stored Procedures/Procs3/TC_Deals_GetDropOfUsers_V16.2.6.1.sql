IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_GetDropOfUsers_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_GetDropOfUsers_V16]
GO

	-----------------------------------------
--Author:Sourav 18 feb 16
--get follw up data
--exec [dbo].[TC_Deals_GetDropOfUsers_V16.2.6.1]
-- Modified by Mukul Bansal on 29th July, 2016
   -- Added pushstatus in select statement
-----------------------------------------
CREATE PROCEDURE [dbo].[TC_Deals_GetDropOfUsers_V16.2.6.1]
AS

BEGIN
	SELECT DI.ID AS [RefId]
	,DI.CustomerName AS [UserName]
		,DI.CustomerMobile AS [UserMobile]
		,DI.CustomerEmail AS [UserEmail]
		,C.NAME AS [UserCity]
		,C.ID AS [CityId]
		,DI.EntryDateTime AS [CreatedOn]
		,DDOU.LastCallTime AS [LastCallTime]
		,DDOU.FollowUpTime
		,DDOU.Comments AS [Comments]
		,TSP.DiscountedPrice AS [OfferPrice]
		,TSP.ActualOnroadPrice AS [ActualPrice]
		,isnull(DL.Organization,'') AS [DealerName]
		,D.ContactEmail AS [DealerEmail]
		,D.ContactMobile AS [DealerMobile]
		,isnull(DL.Address1,'')+isnull(DL.Address2,'') AS [DealerAddress]
		,D.DealerId as [DealerId]
		,V.Make AS [Make]
		,V.Model AS [Model]
		,V.ModelId AS [ModelId]
		,V.Version AS [Version]
		,V.VersionId AS [VersionId]
		,TDS.MakeYear AS [ManufacturingDate]
		,VC.Color AS [Color]
		,TDS.InteriorColor AS [InteriorColor]
		,TDS.Id AS [StockID] 
		,TDS.Offers AS [Offers]
		,DM.Description AS [Disposition]
		,DM.Id as [DispositionId]
		,DI.PushStatus
	FROM DealInquiries DI WITH (NOLOCK)
	INNER JOIN TC_Deals_Stock TDS WITH (NOLOCK) ON DI.StockId = TDS.Id
	INNER JOIN TC_Deals_StockPrices TSP WITH (NOLOCK) ON TSP.TC_Deals_StockId = TDS.ID AND TSP.CityId = DI.CityId
	INNER JOIN TC_Deals_Dealers D WITH (NOLOCK) ON D.DealerId = TDS.BranchId
	INNER JOIN Cities C WITH (NOLOCK) ON C.ID = DI.CityId 
	INNER JOIN Dealers DL WITH (NOLOCK) ON DL.Id = TDS.BranchId
	INNER JOIN vwMMV V WITH (NOLOCK) ON V.VersionId = TDS.CarVersionId
	INNER JOIN VersionColors VC WITH (NOLOCK) ON VC.ID = TDS.VersionColorId
	INNER JOIN TC_Deals_DroppedOffUsersCalls DDOU WITH (NOLOCK) ON DDOU.DealInquiries_Id = DI.ID
	LEFT JOIN DispositionMaster DM WITH (NOLOCK) ON DM.Id=DDOU.DispositionId
	WHERE DDOU.Status=1 AND DI.isPaid = 0  --Modified by Purohith Guguloth
	ORDER BY DDOU.FollowupTime
END

