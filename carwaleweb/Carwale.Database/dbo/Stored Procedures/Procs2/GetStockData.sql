IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetStockData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetStockData]
GO

	-- =============================================
-- Author:		Sahil Sharma
-- Create date: 26th oct,2016
-- Description:	get Customer stock data
-- Modified by Prabhudas on 07th Nov,2016 - Added Customer IP field.
-- =============================================
CREATE PROCEDURE [dbo].[GetStockData] 
	 @CustSellInquiryId INT,
	 @ActionType TINYINT,
	 @CarTradeStockId INT OUTPUT,
	 @IsLiveOnCW BIT OUTPUT

AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	-- get cartrade stock id needed in case  of Update or delete
	SET @CarTradeStockId = -1

	--Get cartrade stock id to delete/update stock shared to cartrade
	IF (@ActionType = 2 OR @ActionType = 3)
		BEGIN
			SET @CarTradeStockId = (SELECT CarTradeStockId FROM CustStockShared WITH(NOLOCK) WHERE InquiryId = @CustSellInquiryId);
		END

	-- get CustStockData needed in case  of Create or Update
	SET @IsLiveOnCW = 0;
	IF (@ActionType = 1 OR @ActionType = 2)
	BEGIN
		IF EXISTS(SELECT 1 FROM LiveListings WITH(NOLOCK) WHERE Inquiryid = @CustSellInquiryId AND SellerType=2)
		BEGIN
			SET @IsLiveOnCW = 1;
			SELECT CSI.CustomerId,CSI.CustomerName,CSI.CustomerEmail,
				CSI.CustomerMobile, CUST.Address, LL.StateName, LL.CityName,
				CSI.PinCode, LL.MakeName, LL.ModelName,
				LL.VersionName, CSI.Price, CSI.Color, CSI.Kilometers,
				CSI.MakeYear, CSID.Owners, LL.StateName as ListingStateName, LL.CityName as ListingCityName,
				LL.AreaName, CSI.PinCode as ListingPincode, LL.EntryDate,
				CSID.Insurance, CSID.InsuranceExpiry, CSI.Comments, CSI.CarRegNo,CSI.ShareToCT as OptInOut,
				CSI.ID as inquiryId, AREA.Lattitude, AREA.Longitude,CSI.IPAddress as CustomerIP
			FROM CustomerSellInquiries AS CSI WITH(NOLOCK)
			INNER JOIN LiveListings AS LL WITH(NOLOCK) ON LL.Inquiryid = CSI.ID
			LEFT JOIN CustomerSellInquiryDetails AS CSID WITH(NOLOCK) ON CSI.ID = CSID.InquiryId
			LEFT JOIN Customers AS CUST WITH(NOLOCK) ON CUST.Id = CSI.CustomerId
			LEFT JOIN AREAS AS AREA WITH(NOLOCK) ON LL.AreaId = AREA.ID
			WHERE CSI.Id = @CustSellInquiryId AND  LL.SellerType = 2;
		END
	END
    
END
