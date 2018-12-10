IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetCustomerDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetCustomerDetails]
GO

	-- Created By:	Surendra
-- Create date: 30 Jan 2012
-- Description:	getting Customer Details
-- Modified By : Tejashree Patil on 14 Jan 2013; Fetched Address of customer, WITH(NOLOCK) implementation
-- Modified By: Vivek on 24thjune,2013, Included select statement to populate the booking data for re-edditing.
-- [TC_GetCustomerDetails] 5,3312,1107
-- Modified By: Vivek Gupta on 6th Aug,2013, Added Booking date in select Statement
-- Modified By: Vivek Gupta on 16th Aug,2013 Added @BookingName,@bookingMobile,@promDeliveryDate
-- Modified By Manish on 27-08-2013 changing the column from BookingCompletedDate to BookingDate
-- Modified By Vivek Gupta on 6/9/2013, Added Salutaion and lastname and inquiry Added Date
-- Modified By: vivek Gupta on 30-09-2013, Added Variables @CarDetails,@VersionId,@ModelYear,@CarColorId
-- Modified By: Ashwini Dhamankar on Nov 6,2014  Fetched CarDetails from vwAllMMv instead of vwMMV and added input parameter @ApplicationId.
-- Modified By: Ashwini Dhamankar on 24-11-2014, Fetched CouponCode, CwOfferId, IsOfferClaimed for Offers.
-- Modified By: Ashwini Dhamankar on 2-12-2014, Fetched OfferTitle for Offers.
-- Modified By Vivek Gupta on 03-04-2015, fetched paymentmode and pickupdatetime
-- Modified By : Ashwini Dhamankar on Jan 8,2015 (Fetched @TC_Deals_StockId and  @TC_DealsStockVINId)
-- Modified By : Ashwini Dhamankar on Feb 19,2016 (For sourceId = 140 fetch color and model year)
-- Modified By : Ruchira Patil on 22 Feb 2016 (to fetch VIN nos in case of dropped of inquiries)
-- Modified By : Ruchira Patil on 14 April 2016 (to fetch VIN nos in case of CW advantage Masking Number Inquiry)
--[TC_GetCustomerDetails] 5,20905,15406,1 -- source id = 140
--[TC_GetCustomerDetails] 5,23400,17435,1
-- Modified By : Ashwini Dhamankar on May 12,2016 (Added constraint of InquirySourceId 147 and 148)
-- =============================================
CREATE Procedure [dbo].[TC_GetCustomerDetails] 
@BranchId NUMERIC, 
@CustId BIGINT,
@TC_NewCarInquiriesId INT = null,
@ApplicationId TINYINT = NULL
AS                
	BEGIN       
	    DECLARE @Price BIGINT
	    DECLARE @Payment BIGINT
	    DECLARE @Pending BIGINT
	    DECLARE @VinNo VARCHAR(20)
	    DECLARE @IsLoanRequired INT
	    DECLARE @BookingDate DATETIME
	    DECLARE @BookingName VARCHAR(50)
	    DECLARE @BookingMobile VARCHAR(50)
	    DECLARE @PromDeliveryDate DATETIME
	    DECLARE @InquiryAddedDate DATETIME --Modified By Vivek Gupta on 6/9/2013, Added Salutaion and lastname and inquiry Added Date
	    DECLARE @BookingSalutation VARCHAR(20)
	    DECLARE @BookingLastName VARCHAR(50)
	 -- Modified By: vivek Gupta on 30-09-2013
		DECLARE @CarDetails VARCHAR(100)
		DECLARE @VersionId INT
		DECLARE @ModelYear VARCHAR(50)
		DECLARE @CarColorId INT
	 -- Modified By: Ashwini Dhamankar on 24-11-2014, Added CouponCode and CWOfferId for Offers.
		DECLARE @CouponCode VARCHAR(25)
		DECLARE @CwOfferId INT
		DECLARE @IsOfferClaimed BIT
		DECLARE @OfferTitle VARCHAR(50)
		DECLARE @PaymentMode VARCHAR(50)
		DECLARE @PickupDateTime DATETIME
		DECLARE @TC_Deals_StockId INT = NULL
		DECLARE @TC_DealsStockVINId INT = NULL
		DECLARE @TC_InquirySourceId INT = NULL
		DECLARE @CityId INT = NULL
		DECLARE @IsPaymentSuccess BIT = 0
			    
	    SELECT @Price=Price,
	           @Payment=Payment,
	           @Pending=PendingPayment,
	           @VinNo=VinNo,
	           @IsLoanRequired=IsLoanRequired,
	           @BookingDate = BookingDate,   /*BookingCompletedDate*/  --Modified By Manish on 27-08-2013 changing the column from BookingCompletedDate to BookingDate
	           @BookingName = BookingName,
	           @BookingMobile = BookingMobile,
	           @PromDeliveryDate = PrefDeliveryDate,
	           @BookingSalutation = Salutation,
	           @BookingLastName = LastName,
			   @ModelYear = ModelYear,
			   @CarColorId = CarColorId,
			   @IsOfferClaimed = IsOfferClaimed,
			   @PaymentMode = PaymentMode,
			   @PickupDateTime = PickupDateTime,
			   @TC_Deals_StockId = TC_Deals_StockId,
			   @TC_DealsStockVINId = TC_Deals_StockVINId
			   
	    FROM   TC_NewCarBooking WITH(NOLOCK)
	    WHERE  
	        TC_NewCarInquiriesId = @TC_NewCarInquiriesId

	----------Below code added by Vivek Gupta on 6/9/2013------------
	    SELECT @InquiryAddedDate = CreatedOn, @VersionId = VersionId, @CouponCode=CouponCode,@CwOfferId=CwOfferId,@TC_InquirySourceId = TC_InquirySourceId,
		@TC_Deals_StockId = TC_Deals_stockId    --Modified By: Ashwini Dhamankar on 24-11-2014, Added CouponCode and CWOfferId for Offers.
		,@CityId = CityId -- Modified by Ruchira Patil on 21st April 2016 (added cityid)
		,@IsPaymentSuccess = ISNULL(IsPaymentSuccess,0)   --Modified by : Ashwini Dhamankar on May 12,2016 
	    FROM   TC_NewCarInquiries WITH(NOLOCK)
	    WHERE TC_NewCarInquiriesId = @TC_NewCarInquiriesId
	-------------------------------------------------------------------------------

		--Added By : Ashwini Dhamankar on Feb 19,2016 (For sourceId = 140 fetch color and model year)
		IF(@TC_InquirySourceId = 140 OR (@TC_InquirySourceId = 146 AND @TC_Deals_StockId IS NOT NULL) OR (@TC_InquirySourceId IN(147,148) AND @IsPaymentSuccess = 0))   --CW advantage Masking Number Inquiry
		BEGIN
		IF(@Price IS NULL) -- for edit booking @Price condition is used
		BEGIN
			--SELECT @TC_Deals_StockId = TC_Deals_stockId FROM TC_NewCarInquiries WITH(NOLOCK) WHERE TC_NewCarInquiriesId = @TC_NewCarInquiriesId 
			-- Modified by Ruchira Patil on 21st April 2016 (changed join condition on TC_Deals_StockPrices table and added cityid)
			SELECT @ModelYear = YEAR(DS.MakeYear),@CarColorId = DS.VersionColorId,@Price = SP.DiscountedPrice 
			FROM TC_Deals_Stock DS WITH(NOLOCK) 
			JOIN TC_NewCarInquiries NCI WITH(NOLOCK) ON NCI.TC_Deals_StockId = DS.Id AND NCI.TC_NewCarInquiriesId = @TC_NewCarInquiriesId
			LEFT JOIN TC_Deals_StockPrices SP WITH(NOLOCK) ON DS.Id = SP.TC_Deals_StockId
			WHERE DS.Id = @TC_Deals_StockId AND SP.CityId = @CityId
		END
			-- Added by Ruchira on 22 feb 2016 (to fetch VIN nos in case of dropped of inquiries)
			SELECT TD.TC_DealsStockVINId AS DealsStockVINId, TD.VINNo,TC_Deals_StockId
			INTO #tempVin
			FROM TC_Deals_StockVIN TD WITH(NOLOCK) 
			WHERE TD.Status = 2 and TC_Deals_StockId = @TC_Deals_StockId
		END
	
	
	-- Added By: vivek Gupta on 30-09-2013
	    SELECT	@CarDetails=V.Make + ' ' + V.Model + ' '  + V.Version + ' '  
		FROM	vwAllMMV V WITH(NOLOCK)
		WHERE	V.VersionId=@VersionId AND V.ApplicationId=@ApplicationId  
	--------------------------------------------------------------------------------
	    -- Modified By: Ashwini Dhamankar on 24-11-2014, Fetched Color.
		IF(@CarColorId IS NULL)
		BEGIN
			SELECT @CarColorId=V.VersionColorsId
			FROM   vwAllVersionColors V WITH(NOLOCK)
			WHERE  V.VersionId = @VersionId
					AND ApplicationId=@ApplicationId
		END

   ------------------------------------------------------------------------------------
       
	    --Added By: Ashwini Dhamankar on Dec 2, 2014 , Fetched OfferTitle
       SELECT @OfferTitle=OfferTitle  
	   FROM   DealerOffers WITH(NOLOCK)
	   where  ID=@CwOfferId    

    ------------------------------------------------------------------------------------------------ 
	--Added By : Ashwini Dhamankar on Jan 8,2015
		--IF(@TC_DealsStockVINId IS NOT NULL)
		--BEGIN
		--	SELECT @MakeYear = DATEPART(yyyy,DS.MakeYear),
		--		   @Colour = dbo.Titlecase(C.Name) 
		--	FROM TC_Deals_Stock DS WITH (NOLOCK)
		--	INNER JOIN Colors C WITH (NOLOCK) ON C.ID = DS.VersionColorId
		--	LEFT JOIN TC_Deals_StockVIN DSV WITH (NOLOCK) ON DSV.TC_Deals_StockId = DS.Id
		--	--LEFT JOIN TC_NewCarInquiries NCI WITH (NOLOCK) ON NCI.TC_DealsStockVINId = DSV.TC_DealsStockVINId
		--	--LEFT JOIN TC_Deals_StockPrices DSP WITH (NOLOCK) ON DSP.TC_Deals_StockId = DS.Id AND DSP.CityId = NCI.CityId
		--	WHERE DSV.TC_DealsStockVINId = @TC_DealsStockVINId
		--END

	   ----Modified By Vivek Gupta on 6/9/2013, Added Salutaion and lastname and inquiry Added Date
	   -- Modified By: vivek Gupta on 30-09-2013, Added @CarDetails,@ModelYear,@CarColorId
		SELECT	CustomerName,Mobile,Email,Buytime,Comments,Address,Salutation,LastName,@Price AS Price,@Payment AS Payment,@Pending AS Pending,
		        @VinNo AS VinNo,@IsLoanRequired AS LoanRequired,@BookingDate AS BookingDate,@InquiryAddedDate AS InquiryAddedDate,
		        ISNULL(@BookingName,CustomerName) AS BookingName,ISNULL(@BookingMobile,Mobile) AS BookingMobile, @PromDeliveryDate AS PromDeliveryDate,
		        @BookingSalutation AS BookingSalutation,@BookingLastName AS BookingLastName,@CarDetails AS CarDetails, @VersionId AS VersionId,-- Modified By : Tejashree Patil on 14 Jan 2013;Modified By Vivek Gupta on6/9/2013, Added Salutaion and lastname and inquiry Added Date
		        @ModelYear AS ModelYear, @CarColorId AS CarColorId,@CouponCode AS CouponCode, @CwOfferId AS CwOfferId, ISNULL(@IsOfferClaimed,0) AS IsOfferClaimed, @OfferTitle AS OfferTitle,   --Modified By: Ashwini Dhamankar on 24-11-2014, Added CouponCode, CWOfferId and OfferTitle for Offers.
		        @PaymentMode AS PaymentMode ,@PickupDateTime AS PickupDateTime,@TC_DealsStockVINId TC_DealsStockVINId,@TC_Deals_StockId TC_Deals_StockId,@TC_InquirySourceId AS TC_InquirySourceId,@IsPaymentSuccess AS IsPaymentSuccess
		FROM	TC_CustomerDetails WITH(NOLOCK)             --Modified By Vivek Gupta on 24thjune,2013,	      --Modified By Vivek Gupta on 16thAug,2013,Added Parameters BookingName,BookingMobile,PromDeliveryDate		 	                                                                                                          
		WHERE	BranchId=@BranchId                          -- Added New parameters in select statement(@Price,@Payment,@Pending,@VinNo,@IsLoanRequired)
				AND id=@CustId

		-- -- Added by Ruchira on 22 feb 2016 (to fetch VIN nos in case of dropped of inquiries)
		IF(@TC_InquirySourceId = 140 OR (@TC_InquirySourceId = 146 AND @TC_Deals_StockId IS NOT NULL) OR (@TC_InquirySourceId IN(147,148) AND @IsPaymentSuccess = 0)) -- CW advantage Masking Number Inquiry
		BEGIN
			SELECT DealsStockVINId,VINNo,TC_Deals_StockId
			FROM #tempVin
		END
	END
------------------------------------------------------------------------------------------------------------------------------------------------------------------------


