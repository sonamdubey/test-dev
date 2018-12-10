IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetStockPerformance]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetStockPerformance]
GO

	
-- ===============================================================================================
-- Author		: Yuga Hatolkar
-- Create date	: 24th Dec, 2015
-- Description	: Get Best or Under Performed Stocks.
-- Modified By  : Khushaboo Patil on 29/04/2016 fetch stock prices and carphotos count
--TC_GetStockPerformance 5,1
-- ==================================================================================================
CREATE PROCEDURE [dbo].[TC_GetStockPerformance]
@BranchId INT,
@BestPerforming INT

AS
BEGIN

	IF @BestPerforming = 1
	BEGIN
		SELECT TOP 5 (SELECT COUNT(DISTINCT TC_InquiriesLeadId) FROM TC_BuyerInquiries WITH (NOLOCK) WHERE StockId =TS.Id) Views,
		(SELECT DATEDIFF(DAY, EntryDate ,GETDATE()) FROM TC_Stock WITH (NOLOCK) WHERE Id = TS.Id) StockAgeInDays,
		TS.Id AS StockId,TS.Price AS StockPrices, -- Modified By  : Khushaboo Patil on 29/04/2016 fetch stock prices and carphotos count
		VW.Car,
		(
			SELECT COUNT(DISTINCT TC_InquiriesLeadId) 
			FROM TC_BuyerInquiries WITH(NOLOCK)		
			WHERE StockId = TS.Id AND TC_InquirySourceId = 1
		) AS LeadsFromCarwale,
		(
			SELECT COUNT(TC.TC_CallsId) 
			FROM TC_BuyerInquiries BI WITH(NOLOCK)
			INNER JOIN TC_Stock TSK WITH(NOLOCK) ON TSK.Id = BI.StockId
			INNER JOIN TC_InquiriesLead TIL WITH(NOLOCK) ON TIL.TC_InquiriesLeadId = BI.TC_InquiriesLeadId
			INNER JOIN TC_Lead TL WITH(NOLOCK) ON TL.TC_LeadId = TIL.TC_LeadId
			INNER JOIN tc_calls  TC WITH(NOLOCK) ON TC.TC_LeadId = TL.TC_LeadId
			WHERE TSK.Id = TS.Id) AS FollowUps,
		(
			SELECT COUNT(DISTINCT ID) 
			FROM TC_CarPhotos CP WITH(NOLOCK) 
			WHERE CP.StockId = TS.Id AND CP.IsActive = 1
		) CarPhotos
		FROM TC_Stock TS WITH (NOLOCK)
		INNER JOIN vwMMV VW WITH(NOLOCK) ON TS.VersionId = VW.VersionId		
		WHERE BranchId = @BranchId AND StatusId=1  AND IsActive = 1 AND IsApproved = 1 
		ORDER BY Views DESC
	END

	ELSE
	BEGIN
		SELECT TOP 5 (
						SELECT COUNT(DISTINCT TC_InquiriesLeadId)
						FROM TC_BuyerInquiries WITH (NOLOCK) 
						WHERE StockId =TS.Id) Views,
						(
							SELECT DATEDIFF(DAY, EntryDate ,GETDATE())
							FROM TC_Stock WITH (NOLOCK) 
							WHERE Id = TS.Id
						) StockAgeInDays,
						TS.Id AS StockId,TS.Price AS StockPrices,
		VW.Car,
		( 
				SELECT COUNT(DISTINCT TC_InquiriesLeadId) 
				FROM TC_BuyerInquiries WITH(NOLOCK)		
				WHERE StockId = TS.Id AND TC_InquirySourceId = 1) AS LeadsFromCarwale,
		(
		SELECT COUNT(TC.TC_CallsId) FROM TC_BuyerInquiries BI WITH(NOLOCK)
		INNER JOIN TC_Stock TSK WITH(NOLOCK) ON TSK.Id = BI.StockId
		INNER JOIN TC_InquiriesLead TIL WITH(NOLOCK) ON TIL.TC_InquiriesLeadId = BI.TC_InquiriesLeadId
		INNER JOIN TC_Lead TL WITH(NOLOCK) ON TL.TC_LeadId = TIL.TC_LeadId
		INNER JOIN tc_calls TC WITH(NOLOCK) ON TC.TC_LeadId = TL.TC_LeadId
		WHERE TSK.Id = TS.Id) AS FollowUps,
		(        SELECT COUNT(DISTINCT ID) 
		         FROM TC_CarPhotos CP WITH(NOLOCK) WHERE CP.StockId = TS.Id AND CP.IsActive = 1
		) CarPhotos
				FROM TC_Stock TS WITH (NOLOCK)
				INNER JOIN vwMMV VW WITH(NOLOCK) ON TS.VersionId = VW.VersionId		
				WHERE BranchId = @BranchId AND StatusId=1  AND IsActive = 1 AND IsApproved = 1 
		ORDER BY Views
	END

END

