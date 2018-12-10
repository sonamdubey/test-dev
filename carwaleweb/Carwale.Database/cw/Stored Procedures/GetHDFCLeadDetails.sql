IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[GetHDFCLeadDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[GetHDFCLeadDetails]
GO

	
-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <24/09/2012>
-- Description:	<Returns the details of buyers and sellers for the dealers who are HDFC empaneled>
--              EXEC [cw].[GetHDFCLeadDetails] '2012-09-20' , '2012-09-21'
-- =============================================
CREATE PROCEDURE [cw].[GetHDFCLeadDetails] 
	-- Add the parameters for the stored procedure here
	@FromDate DATE,
	@ToDate DATE
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
    --SET @FromDate = @FromDate+'00:00:00.000'
    --SET @ToDate = @ToDate+'23:59:59.999'

	SELECT LL.ProfileId ,DS.Organization AS Dealer,ISNULL(DS.Address1,'')+ ISNULL(DS.Address2,'') AS DealerAddress,
		DS.MobileNo AS DealerMobile,DS.EmailId,LL.StateName AS DealerState,LL.CityName AS DealerCity,
		LL.AreaName AS DealerLocality,LL.MakeName AS Make,LL.ModelName AS Model,VersionName AS Version,LL.MakeYear,LL.Price,
		LL.CalculatedEMI,CU.Name AS BuyerName,CU.Mobile AS BuyerMobile,CU.email AS BuyerEmail,UCP.EntryDate AS RequestDate
	    --IsEligible=CASE ISNULL(LL.CalculatedEMI,0) WHEN 0 THEN 0 ELSE 1 END---,UCP.BuyTime
	FROM SellInquiries SI
		INNER JOIN SellInquiriesDetails AS SID ON SI.ID=SID.SellInquiryId
		--INNER JOIN HDFCDealerRepresentatives HDR ON HDR.DealerId=SI.DealerId
		INNER JOIN LiveListings LL ON LL.Inquiryid=SI.ID AND SellerType=1
		INNER JOIN HDFCUsedCarLoanData UCP ON UCP.SellerInquiryId=SI.ID
		INNER JOIN Dealers DS ON DS.ID=SI.DealerId
		INNER JOIN Customers CU ON CU.Id=UCP.BuyerId 
	WHERE CONVERT(DATE,UCP.EntryDate) BETWEEN @FromDate AND @ToDate
		--AND LL.CalculatedEMI IS  NULL
	ORDER BY UCP.EntryDate
END

