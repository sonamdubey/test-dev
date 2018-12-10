IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[DCRM_GetTotalBookedCarPrice]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[DCRM_GetTotalBookedCarPrice]
GO

	
-------------------------------------------
--Author : Vinay Kumar Prajapati 3rd desc 2015
--Puspose : Get total  Showroom price of total Cars booked Through carwale as per Dealer , User login wise  
-- EXEC [dbo].[DCRM_GetTotalBookedCarPrice] 997
-- Modifed By : Sunil M. Yadav On 04th Aug 2016 , Comnvert temp table variable to hash table to improve performance.
-------------------------------------

CREATE  PROCEDURE [dbo].[DCRM_GetTotalBookedCarPrice] 
 @ExecutiveId    INT  = NULL
AS  
  
BEGIN  
 IF @ExecutiveId > 0 
    BEGIN  
		--Get All the Dealers with city 
		CREATE TABLE  #TempDealers(DealerId INT)

		INSERT INTO #TempDealers(DealerId)
		SELECT DISTINCT D.ID AS DealerId
		FROM DCRM_ADM_UserDealers DAU WITH (NOLOCK) 
			INNER JOIN Dealers AS D  WITH (NOLOCK) ON DAU.DealerId = D.ID
			--INNER JOIN TC_DealerCities AS DC WITH(NOLOCK) ON DC.DealerId=D.ID
		WHERE DAU.UserId = @ExecutiveId 

		CREATE INDEX IX_TempDealers ON #TempDealers(DealerId);

		--Cars showroom price which booked Through carwale 
		-- DECLARE @TempCarsPriceBookedViaCarwale Table(DealerId INT,Price Int,Inquiry Int)
		CREATE TABLE #TempCarsPriceBookedViaCarwale(DealerId INT,Price INT,Inquiry INT)	-- Sunil M. Yadav On 04th Aug 2016 , Comnvert temp table variable to hash table to improve performance.
		
		INSERT INTO #TempCarsPriceBookedViaCarwale(DealerId,Price,Inquiry)		
		SELECT D.DealerId,ISNULL(SP.Price,0) AS Price ,NCB.TC_NewCarBookingId AS Inquiry
		FROM TC_NewCarInquiries TNI WITH(NOLOCK)
		INNER JOIN TC_NewCarBooking AS NCB WITH(NOLOCK) ON NCB.TC_NewCarInquiriesId=TNI.TC_NewCarInquiriesId AND TNI.ContractId IS NOT NULL AND TNI.CampaignId IS NOT NULL
		INNER JOIN TC_InquiriesLead TIL WITH(NOLOCK) ON TNI.TC_InquiriesLeadId = TIL.TC_InquiriesLeadId 
		INNER JOIN TC_InquirySource  TIS WITH(NOLOCK) ON TNI.TC_InquirySourceId = TIS.Id
		INNER JOIN #TempDealers  D   ON D.DealerId = TIL.BranchId
	    LEFT JOIN NewCarShowroomPrices  AS SP WITH(NOLOCK) ON SP.CarVersionId = TNI.VersionId AND SP.CityId=TNI.CityId
		WHERE TIS.TC_InquiryGroupSourceId = 11 AND TNI.BookingStatus = 32  

		SELECT BPC.DealerId,SUM(BPC.Price) AS Price,COUNT(BPC.Inquiry) AS TotalBookedCar 
		FROM #TempCarsPriceBookedViaCarwale AS BPC
		GROUP BY  BPC.DealerId

		 DROP TABLE #TempDealers;
		 DROP TABLE #TempCarsPriceBookedViaCarwale;
	END
 
END

