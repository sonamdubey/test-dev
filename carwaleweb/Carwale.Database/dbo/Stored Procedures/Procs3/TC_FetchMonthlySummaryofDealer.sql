IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_FetchMonthlySummaryofDealer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_FetchMonthlySummaryofDealer]
GO
	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 15th Dec 2015
-- Description:	to show monthly summay of report of a dealer
-- EXEC TC_FetchMonthlySummaryofDealer 5,'03/30/2016','04/01/2016'
-- Modified by : Ruchira Patilon 24 th Dec 2015 (to fetch booked leads for previous months)
-- Modified By : Nilima More On 5th Feb 2016 (Closed leads=booked leads + lost leads,Added lost to Co-dealer and their respective percentage)
-- Modified BY: Ashwini Dhamankar on Feb 10,2016 (Added modified logic for lost lead)
-- Modified By : Nilima More On 11th Feb 2016 (Lost to codealer Condition on Closed Date.)
-- Modified by : Kritika Choudhary on 5th April 2016, modified output based on date filter also commented BookedLastMonth,BookedSecondLastMonth,BookedThirdLastMonth and BookedFourthLastMonth
-- =============================================
CREATE PROCEDURE [dbo].[TC_FetchMonthlySummaryofDealer]
	@BranchId INT,
	@FromDate DATETIME= NULL,
    @ToDate DATETIME= NULL
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @DealerName VARCHAR(MAX),
	@LeadAssigned INT,@ActiveLeads INT,@BookedCurrMonth INT,
	@LostLeads INT,@BookedPercentageCurrMonth INT,--@BookedLastMonth INT
	--@BookedSecondLastMonth INT,@BookedThirdLastMonth INT,
	@LostToCodealer INT,@LostToCompetitionBrand INT,@LostToCodealerPercentage INT,
	@LostToOtherBrandPercentage INT--,@BookedFourthLastMonth INT--,@BookedCurrentMonthLead INT

	SET @ToDate = DATEADD(ms, -2, DATEADD(dd, 1, DATEDIFF(dd, 0,@ToDate))) 

	SELECT 
	@DealerName = D.Organization ,
	@LeadAssigned = COUNT(DISTINCT(CASE WHEN TCIL.TC_UserId IS NOT NULL AND TCIL.CreatedDate BETWEEN @FromDate AND @ToDate THEN TCIL.TC_LeadId END)),
	@ActiveLeads = COUNT(DISTINCT(CASE WHEN TCIL.TC_LeadStageId<>3 AND ISNULL(TCIL.TC_LeadDispositionID,0)<>4 AND TCIL.CreatedDate BETWEEN @FromDate AND @ToDate THEN TCIL.TC_LeadId END)),
	@LostLeads = COUNT(DISTINCT(CASE WHEN TCIL.TC_LeadStageId = 3 AND ((ISNULL(TCIL.TC_LeadDispositionID,0) NOT IN(1,3,4,70,71,74)) OR (ISNULL(TCIL.TC_LeadDispositionID,0) = 41 AND ISNULL(TNI.BookingStatus,0) <> 32)) AND TCL.LeadClosedDate BETWEEN @FromDate AND @ToDate THEN TCIL.TC_LeadId END)),
	@BookedCurrMonth = COUNT(DISTINCT(CASE WHEN ISNULL(TNI.TC_LeadDispositionId,0) IN(4,41) AND ISNULL(TNI.BookingStatus,0) = 32 AND TNI.BookingDate BETWEEN @FromDate AND @ToDate THEN TNI.TC_NewCarInquiriesId END)),--its according to the selected date range not current month 
	--@BookedCurrentMonthLead = COUNT(DISTINCT(CASE WHEN ISNULL(TNI.TC_LeadDispositionId,0) = 4 AND MONTH(TNI.BookingDate ) = MONTH(@ToDate) AND YEAR(TNI.BookingDate ) = YEAR(@ToDate) THEN TCIL.TC_LeadId END)),
	--Commented by: Kritika Choudhary on 5th April 2016
	--@BookedLastMonth = COUNT(DISTINCT(CASE WHEN ISNULL(TNI.TC_LeadDispositionId,0) IN(4,41) AND ISNULL(TNI.BookingStatus,0) = 32 AND
	--DATEDIFF(MONTH,TNI.BookingDate, @ToDate) = 1
	--THEN TNI.TC_NewCarInquiriesId END)),
	--@BookedSecondLastMonth = COUNT(DISTINCT(CASE WHEN ISNULL(TNI.TC_LeadDispositionId,0) IN(4,41) AND ISNULL(TNI.BookingStatus,0) = 32 AND
	--DATEDIFF(MONTH,TNI.BookingDate, @ToDate) = 2
	--THEN TNI.TC_NewCarInquiriesId END)),
	--@BookedThirdLastMonth =  COUNT(DISTINCT(CASE WHEN ISNULL(TNI.TC_LeadDispositionId,0) IN(4,41) AND ISNULL(TNI.BookingStatus,0) = 32 AND
	--DATEDIFF(MONTH,TNI.BookingDate, @ToDate) = 3
	--THEN TNI.TC_NewCarInquiriesId END)),
	-- @BookedFourthLastMonth = COUNT(DISTINCT(CASE WHEN ISNULL(TNI.TC_LeadDispositionId,0) IN(4,41) AND ISNULL(TNI.BookingStatus,0) = 32 AND
	--	DATEDIFF(MONTH,TNI.BookingDate, @ToDate) = 4
	--	THEN TNI.TC_NewCarInquiriesId END)),
	
	@LostToCodealer = COUNT(DISTINCT(CASE WHEN ISNULL(TNI.TC_LeadDispositionId,0) = 65 AND TCL.LeadClosedDate BETWEEN @FromDate AND @ToDate
									THEN TNI.TC_NewCarInquiriesId
									--THEN TNI.TC_InquiriesLeadId
								END)),
	@LostToCompetitionBrand = COUNT(DISTINCT(CASE WHEN ISNULL(TNI.TC_LeadDispositionId,0) = 64 AND TCL.LeadClosedDate BETWEEN @FromDate AND @ToDate
									THEN TNI.TC_NewCarInquiriesId
								END))
	--@PendingDeliveryLeads =  COUNT(DISTINCT(CASE WHEN TNI.TC_LeadDispositionId = 4 AND ISNULL(TNI.CarDeliveryStatus,0)<>77 AND ISNULL(TNI.MostInterested,0) = 1 AND MONTH(TNI.BookingDate ) = MONTH(@ToDate) AND YEAR(TNI.BookingDate ) = YEAR(@ToDate) THEN TNI.TC_NewCarInquiriesId END))

	FROM TC_Lead AS TCL WITH(NOLOCK)
	JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TCL.TC_LeadId=TCIL.TC_LeadId
	AND TCIL.TC_LeadInquiryTypeId = 3 AND TCIL.BranchId = @BranchId
	JOIN TC_NewCarInquiries TNI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId =TNI.TC_InquiriesLeadId 
	JOIN Dealers D WITH(NOLOCK) ON D.ID = TCIL.BranchId
	GROUP BY D.Organization

	SELECT @DealerName DealerName ,
	@LeadAssigned LeadAssigned,@ActiveLeads ActiveLeads,(@LostLeads + @BookedCurrMonth) ClosedLeads,@LostLeads Lost,--@BookedCurrentMonthLead BookedCurrentMonthLead,
	@BookedCurrMonth BookedCurrMonth,--@BookedLastMonth BookedLastMonth ,@BookedSecondLastMonth BookedSecondLastMonth,
	--@BookedThirdLastMonth BookedThirdLastMonth,@BookedFourthLastMonth BookedFourthLastMonth,
	@LostToCodealer LostToCodealer,@LostToCompetitionBrand LostToCompetitionBrand,
	CASE WHEN (@LostLeads + @BookedCurrMonth) = 0 
	     THEN 0 
		 ELSE
			CONVERT(INT, ROUND(CAST(CAST(@BookedCurrMonth AS FLOAT) / (@LostLeads + @BookedCurrMonth) AS DECIMAL(8, 2)) * 100, 0)) 
	END BookedPercentageCurrMonth ,
	CASE WHEN (@LostLeads + @BookedCurrMonth + @LostToCodealer) = 0 
	     THEN 0 
		 ELSE
			CONVERT(INT, ROUND(CAST(CAST(@LostToCodealer AS FLOAT) / (@LostLeads + @BookedCurrMonth ) AS DECIMAL(8, 2)) * 100, 0)) 
	END  LostToCodealerPercentage ,
	CASE WHEN (@LostLeads + @BookedCurrMonth + @LostToCompetitionBrand) = 0 
	     THEN 0 
		 ELSE
			CONVERT(INT, ROUND(CAST(CAST(@LostToCompetitionBrand AS FLOAT) / (@LostLeads + @BookedCurrMonth ) AS DECIMAL(8, 2)) * 100, 0))
	END LostToOtherBrandPercentage

	
END

-----------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

