IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_AP_AutoRemoveStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_AP_AutoRemoveStock]
GO

	-- =============================================
-- Author:		<Vivek,,Gupta>
-- Create date: <Create Date,26-06-2015,>
-- Description:	Removing stock from live which are not available in Dealer's panel
-- =============================================
CREATE PROCEDURE [dbo].[TC_AP_AutoRemoveStock]
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	/*
	--Disable Stocks, Removed from Autobiz and is live on CarWale
	UPDATE SellInquiries SET StatusId=2 
	WHERE 
	StatusId = 1
	AND
	ID IN 
	(
		SELECT SI.ID AS InquiryId
		FROM LiveListings LL WITH (NOLOCK), SellInquiries SI WITH (NOLOCK), TC_Stock TS WITH (NOLOCK)
		WHERE SI.SourceId = 2 AND LL.SellerType = 1 AND LL.Inquiryid = SI.ID AND SI.TC_StockId = TS.Id AND (TS.IsActive = 0 OR TS.StatusId IN(2,3,4))
	) 
	
	--Remove Autobiz sync if not on CarWale
	UPDATE TC_Stock SET IsSychronizedCW = 0 WHERE Id IN(
	SELECT TS.Id FROM TC_Stock TS WITH (NOLOCK) LEFT JOIN SellInquiries SI WITH (NOLOCK) ON TS.Id = SI.TC_StockId AND SI.SourceId = 2
	WHERE TS.IsSychronizedCW = 1 AND SI.Id IS NULL)
	
	--Remove Cars with no tc_stockId on CarWale
	UPDATE SellInquiries SET StatusId = 2, PackageExpiryDate = GETDATE()-1, LastUpdated = GETDATE() WHERE ID IN(
	SELECT Id FROM SellInquiries with(NOLOCK)
	WHERE TC_StockId IS NULL AND SourceId = 2 AND StatusId = 1 AND YEAR(EntryDate) >= 2015 AND CONVERT(DATE,PackageExpiryDate) >= CONVERT(DATE,GETDATE()))
	
	--Suspend car if dealer is suspended
	UPDATE TC_Stock 
	SET StatusId=4,IsSychronizedCW=0 
	WHERE Id IN(
			SELECT ST.Id
			FROM TC_Stock ST  with(NOLOCK)
			INNER JOIN SellInquiries SI  with(NOLOCK) ON ST.Id=SI.TC_StockId AND SI.SourceId = 2
			INNER JOIN Dealers AS D with(NOLOCK) ON D.ID = SI.DealerId
			WHERE  SI.StatusId=1 AND D.Status = 1 AND CONVERT(DATE,SI.PackageExpiryDate) >= CONVERT(DATE,GETDATE())
			)*/
	 
END
