IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[WF_AP_DailyActualValues]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[WF_AP_DailyActualValues]
GO

	CREATE PROCEDURE [dbo].[WF_AP_DailyActualValues]
	
 AS

BEGIN

DECLARE @Day AS BIGINT
DECLARE @Month AS BIGINT
DECLARE @Year AS BIGINT
DECLARE @SyncDate AS DATETIME

SET @Day = DAY(GETDATE()-1)
SET @Month = MONTH(GETDATE()-1)
SET @Year = YEAR(GETDATE()-1)
SET @SyncDate = GETDATE()-1

	------------------------ UPDATE DATA FROM CMS -----------------------------------
	--Automotive CPA
	EXEC WF_CMS_GetAutomotiveCPA 4,1,@Day,@Month,@Year,@SyncDate
	--Automotive CPL
	EXEC WF_CMS_GetAutomotiveCPL 5,1,@Day,@Month,@Year,@SyncDate
	--Automotive CPM
	EXEC WF_CMS_GetAutomotiveCPM 6,1,@Day,@Month,@Year,@SyncDate
	--Finance CPM
	EXEC WF_CMS_GetFinanceCPM 7,1,@Day,@Month,@Year,@SyncDate
	--Insurance CPL
	EXEC WF_CMS_GetInsuranceCPL 8,1,@Day,@Month,@Year,@SyncDate
	--Non Automotive CPL
	EXEC WF_CMS_GetNonAutomotiveCPL 9,1,@Day,@Month,@Year,@SyncDate
	--Non Automotive CPM
	EXEC WF_CMS_GetNonAutomotiveCPM 10,1,@Day,@Month,@Year,@SyncDate
	--Events
	EXEC WF_CMS_GetEvents 11,1,@Day,@Month,@Year,@SyncDate
	
	------------------------ END UPDATE DATA FROM CMS --------------------------------
	
	------------------------ UPDATE DATA FROM Individual & Dealer Classified ---------
	--TotalListing
	EXEC WF_CL_TotalListing 14,1,@Day,@Month,@Year,@SyncDate
	--NotVerified
	EXEC WF_CL_NotVerified 15,1,@Day,@Month,@Year,@SyncDate
	--PaidListing
	EXEC WF_CL_PaidListing 17,1,@Day,@Month,@Year,@SyncDate
	--RevenuePerListing
	EXEC WF_CL_RevenuePerListing 18,1,@Day,@Month,@Year,@SyncDate
	--ClassifiedRevenue
	EXECUTE WF_CL_Revenue 105,1,@Day,@Month,@Year,@SyncDate
	--DealerClassified
	EXEC WF_CL_DealerClassified 19,1,@Day,@Month,@Year,@SyncDate
	
	--Fake%
	EXEC WF_CL_FakePer 16,1,@Day,@Month,@Year,@SyncDate
	------------------------ END UPDATE DATA FROM Individual & Dealer Classified ------

	------------------------ UPDATE DATA FROM NCS ------------------------------------

	--DealerSubscription
	EXECUTE WF_DealerSubscription 106,107,1,@Day,@Month,@Year,@SyncDate 
	--SkodaBookings
	EXEC WF_OEMCarSales 108,109,15,1,@Day,@Month,@Year,@SyncDate 
	--GMBookings
	EXEC WF_OEMCarSales 110,111,2,1,@Day,@Month,@Year,@SyncDate 
	--RenaultBookings
	EXEC WF_OEMCarSales 112,113,29,1,@Day,@Month,@Year,@SyncDate 
	--MahindraBookings
	EXEC WF_OEMCarSales 114,115,9,1,@Day,@Month,@Year,@SyncDate 

	----------------------- END UPDATE DATA FROM NCS ---------------------------------
	
	------------------------ UPDATE DATA FROM Site Performance -----------------------
	-- UniqueVisitors
	EXEC WF_SP_UniqueVisitors 61,1,@Month,@Year
	-- Pageviews
	EXEC WF_SP_PageViews 66,1,@Month,@Year
	--Unique Buyers - New Cars
	EXEC WF_UB_UsedCar 104,1,@Day,@Month,@Year,@SyncDate  
	--Unique Buyers - Used Cars
	EXEC WF_UB_NewCars 103,1,@Day,@Month,@Year,@SyncDate   


	----------------------- END UPDATE DATA FROM Site Performance --------------------

	-----------------------UPDATE DATA FOR Operations--------------------------------- 
	--To Get Total Leads
	EXEC WF_GetLEADS 87,1,@Day,@Month,@Year,@SyncDate
	--To Get OEM Leads
	EXECUTE WF_GetOEM 88,1,@Day,@Month,@Year,@SyncDate
	--To GET NCS Leads
	EXECUTE WF_GetNCS  89,1,@Day,@Month,@Year,@SyncDate
	
	--To Get First Connected
	EXECUTE WF_FirstConnect 90,1,@Day,@Month,@Year,@SyncDate
	--To Get Average First Connected
	EXECUTE WF_FirstConnectAvg  91,1,@Day,@Month,@Year,@SyncDate

	--Consultation Section
	--To Get Consultation Leads 
	EXEC WF_Operations_CST 93,5,1,@Day,@Month,@Year,@SyncDate
	--To Get Consultation Leads AVG
	EXEC WF_Operations_CST_Avg 92,5,1,@Day,@Month,@Year,@SyncDate
	--To Get % for Consultation for 2 days
	EXEC WF_Operations_CT_Percentage 117,5,2,1,@Day,@Month,@Year,@SyncDate

	--TestDrive Section
	--To Get TestDrive Leads 
	EXEC WF_Operations_CST 94,14,1,@Day,@Month,@Year,@SyncDate
	--To Get TestDrive Leads AVG
	EXEC WF_Operations_CST_Avg 116,14,1,@Day,@Month,@Year,@SyncDate
	--To Get % for Test Drive for 7 days
	EXEC WF_Operations_CT_Percentage 118,14,7,1,@Day,@Month,@Year,@SyncDate
	
	--Sales Section
	--To Get Sales Leads
	EXEC WF_Operations_CST 95,16,1,@Day,@Month,@Year,@SyncDate
	--To Get Sales Leads AVG
	EXEC WF_Operations_CST_Avg 96,16,1,@Day,@Month,@Year,@SyncDate

	----------------------- END UPDATE DATA FOR Operations --------------------

END




