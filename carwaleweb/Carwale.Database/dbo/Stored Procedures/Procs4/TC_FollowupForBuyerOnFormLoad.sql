IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FollowupForBuyerOnFormLoad]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FollowupForBuyerOnFormLoad]
GO

	CREATE PROCEDURE [dbo].[TC_FollowupForBuyerOnFormLoad]
(
@CustomerId INT,
@BranchId NUMERIC
)
as
-- table for customer details
SELECT CustomerName,Email,Mobile FROM TC_CustomerDetails WITH(NOLOCK)
WHERE Id=@CustomerId

-- table for Inquiry without stoco
SELECT INQ.CreatedDate 'InquiryDate',WS.BodyType,MinPrice,WS.MaxPrice,WS.FromMakeYear,
		WS.ToMakeYear,WS.FuelType,SR.Source
		FROM TC_BuyerInqWithoutStock WS WITH(NOLOCK)
		INNER JOIN TC_Inquiries INQ WITH(NOLOCK) ON WS.TC_InquiriesId=INQ.TC_InquiriesId		 
		INNER JOIN TC_InquirySource SR WITH(NOLOCK) ON INQ.SourceId=SR.Id
		WHERE INQ.InquiryType=1 AND TC_CustomerId=@CustomerId
		
		
-- table for Inquiries with stock
SELECT (VW.Make + ' ' + VW.Model + ' ' + VW.Version) as 'CarName',SI.ID as 'ProfileId',
		INQ.CreatedDate AS 'InquiryDate' ,SR.Source ,ST.MakeYear,CC.RegistrationPlace,
		CC.AdditionalFuel,ST.Colour,ST.Price,ST.Kms,ST.Id
	FROM TC_Inquiries INQ WITH(NOLOCK) 
	INNER JOIN TC_BuyerInquiries BI WITH(NOLOCK) ON INQ.TC_InquiriesId=BI.TC_InquiriesId
	INNER JOIN	TC_Stock ST WITH(NOLOCK) ON BI.StockId=ST.Id
	INNER JOIN vwMMV VW ON INQ.VersionId=VW.VersionId
	INNER JOIN TC_CarCondition CC WITH(NOLOCK) ON ST.Id=CC.StockId
	LEFT OUTER JOIN SellInquiries SI WITH(NOLOCK) ON ST.Id=SI.TC_StockId
	INNER JOIN TC_InquirySource SR WITH(NOLOCK) ON INQ.SourceId=SR.Id
	WHERE INQ.InquiryType=1 AND INQ.TC_CustomerId=@CustomerId