IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[SC].[SetDailyTracker]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [SC].[SetDailyTracker]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 19-08-2013
-- Description:	Set Daily Tracker for the day
--EXEC [sc].[SetDailyTracker]
--Modified by Reshma Shetty 20/08/2013 Added Livelistings and UC Buyers trackers 
--Modified by Reshma Shetty 2/09/2013 Added Leads processed,Leads assigned and Bookings trackers 
--Modified by Fulsmita Bhattacharjee 07/11/2014 added New Car Leads, Dealer listings and Responses, Individual Listings and Responses
--Modified by Fulsmita Bhattacharjee 24/11/2014 modified Tracker 'UCD' commented packagetype!=28
-- Modified by Fulsmita Bhattacharjee 01/12/2014 modified Tracker type 'Mahindra leads Assigned' , 'Mahindra Car Bookings' and 'Mahindra Car Invoiced'
-- Modified By Fulsmita Bhattacharjee on 03/12/2014  , modified NCD tracker type.
-- Modified By Fulsmita Bhattacharjee on 06/12/2014  , added trackers 'Dealer PQ Impression' and 'Unique Cookie for PQ'
-- Modified by Fulsmita Bhattacharjee on 17/12/2014  , uncommented 'New Car Leads' Tracker
 --Modified by Fulsmita Bhattacharjee, Coupon generated  on 29/12/2014, due to chage in logic of the tables
 --Modified by Fulsmita Bhattacharjee, on 30/12/2014 , added trackers. RO value, ES funnel 10, ES funnel 30, ES funnel 50, ES funnel 70, ES funnel 90
 -- Modified by Fulsmita Bhattacharjee, UCD Dealer Sales and NCD Dealer Sales on 08/01/2015
 -- Modified by Fulsmita Bhattacharjee, on 30/01/2015 . added Trackers, DistinctCookie As FormFillCustomers ,Formfill InboundLeads, Formfill distinct Mobile
 --                                                                    ,DealerWise Unique Mobile Including NumberMasking
 --Modified by Fulsmita Bhattacharjee on 30/01/2015 , Changed Logic of Dealer PQ leads-taking count of distinct mobile numbers,added  NCD PQ_Sponsored Active Dealer Counts
 --Modified by Fulsmita Bhattacharjee on 11/02/2015 , Active NCD , New Dealers Joined UCD, Dealers Left UCD
 -- Modified By Prajakta Gunjal on 27/02/2015 , Warranties Approved
 --Modified by Prajakta Gunjal on 27/02/2015, Tracker 'DealerWise Unique Mobile Including NumberMasking', taken leads from mobile and desktop omly.
 --Modified by Prajakta Gunjal on 02/03/2015 commented tracker 'DistinctCookie As FormFillCustomers' because of change in new car lead conversion rate logic.
 --Modified by Prajakta Gunjal on 02/03/2015 commented trackers FormFill DistinctMobile, 'Formfill InboundLeads' because of change in new car lead conversion rate
 --Modified By Prajakta Gunjal on 02/03/2015 added tracker 'FormFill (DistinctMobile+InboundLeads+DealerLocator)','DealerWise Unique Mobile Including NumberMasking-NCD','DistinctCookie As FormFillCustomers'
 --Modified by Prajakta Gunjal on 25/03/2015 added three more ids of table TC_InquirySource - 6,95,96 in the tracker 'FormFill (DistinctMobile+InboundLeads+DealerLocator)'
 --Modified By Prajakta Gunjal on 30/03/2015 ,TrackerType Warranties Approved
 -- Modified by Fulsmita Bhattacharjee on 10/04/2015, added New Car Leads DS, DS Denominator New Car Leads, OverAll Denominator New Car Leads
 -- ,ES Denominator New Car Leads and New Car Leads OverAll this are modified logics for new car lead conversion for new financial year
 --Modified by Fulsmita Bhattacharjee on 21/04/2015, modified Warranties sold Query, Added Warranty Activated, Modified Dealer wise Unique Responses ,included all the leads
 --Modified by Fulsmita Bhattacharjee on 22/04/2015 added MTD Trackers ,MTD Dealer wise Unique Responses, MTD New Car Leads DS, MTD DS Denominator New Car Leads,
 -- MTD ES Denominator New Car Leads, MTD Overall Denominator New Car Leads,MTD New Car Leads OverAll
  -- ConsumerType?
  -- Modified by Fulsmita Bhattacharjee on 02/06/2015, Modified New Car Leads DS, DS Denominator New Car Leads, ES Denominator New Car Leads, MTD New Car Leads DS,
  -- MTD DS Denominator New Car Leads, MTD ES Denominator New Car Leads
  -- Modified by Chirag Perla on 24/07/2015: Added Live Listings for Individuals in Tracker type 'Livelistings-Indiv' with same logic as current Tracker Type 'Listings Live' which is for Dealers; Commented previous Tracker Types 'Livelistings-Indiv' and 'Livelistings-Dealer' 
  -- Modified  by Fulsmita Bhattacharjee on 24/07/2015: modified New Car Leads DS, DS Denominator New Car Leads, MTD New Car Leads DS, MTD DS Denominator New Car Leads
  -- Modified by Chirag Perla on 18-09-2015: Added 2 new Trackers('Overall Dealer wise Unique Responses', 'MTD Overall Dealer wise Unique Responses') to bring in Overall Used Car Leads(Verified + Unverified). Modified 2 existing Trackers('Dealer wise Unique Responses', 'MTD Dealer wise Unique Responses') which will now use TC_BuyerInquiries table for Used Car Leads. 
  -- Modified by Chirag Perla on 06-10-2015: Used Cast on Date 'CreatedOn' in the Trackers 'MTD Dealer wise Unique Responses' and 'MTD Overall Dealer wise Unique Responses'. Also added an additional where clause of "PQPageId<>39" in 'DS Denominator New Car Leads' to exclude Featured PQs. 
  -- Modified by Chirag Perla on 09-10-2015: Added an additional where clause of "PQPageId<>39" in 'MTD DS Denominator New Car Leads', 'MTD ES Denominator New Car Leads', 'MTD OverAll Denominator New Car Leads' to exclude Featured PQs. 
  -- Modified by Chirag Perla on 13-11-2015: Used 'DealerExclusion' table to filter by non-DS Dealers excluded for DS Numbers or ES Dealers only included for ES Numbers in the following trackers---'New Car Leads DS', 'DS Denominator New Car Leads', 'ES Denominator New Car Leads', 'MTD New Car Leads DS''MTD DS Denominator New Car Leads', 'MTD ES Denominator New Car Leads', 'NCD'
  -- Modified by Chirag Perla on 26-11-2015: Commented the Trackers 'NCD' and 'TLL Accepted Top for New Car Leads', as it is no longer used(Also Dealer_NewCar table is not avaialbe anymore).
  -- Modified by Chirag Perla on 17-12-2015: Changed 'New Car Leads DS' logic to exclude CarTrade Leads(LeadClickSource=500) and Leads that have already been received in the same month.

-- =============================================
CREATE PROCEDURE [SC].[SetDailyTracker]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @Date DATE=CONVERT(varchar(11),GETDATE(),113)
	
	--Modified by AM Added 12-09-2013 to get tracker date as 1 day before
	DECLARE @TrackerDate DATE=CONVERT(varchar(11),GETDATE()-1,113)
	
	-- Modified by Chirag Perla on 17-12-2015: For 'New Car Leads DS' MTD Leads
	DECLARE @TrackerMonthStart DATE=DATEADD(mm, DATEDIFF(mm, 0, GETDATE()-1), 0)
	DECLARE @TrackerPrevDay DATE=CONVERT(varchar(11),GETDATE()-2,113)


	DECLARE @BookDate DATE=CONVERT(varchar(11),GETDATE()-1,113)

	--Modified by Fulsmita Bhattacharjee , for Active Used Car Dealers Count
		DECLARE @Autobiz_Usage_Date DATE=CONVERT(varchar(11),GETDATE()-30,113)

		DECLARE @Stock_Update_Date DATE=CONVERT(varchar(11),GETDATE()-30,113)

		DECLARE @Autobiz_Usage_Percent  INT
		set @Autobiz_Usage_Percent=50
		
	--Modified by Fulsmita Bhattacharjee , for Active Used Car Dealers Count

		DECLARE @TrackerDateprevious DATE=CONVERT(varchar(11),GETDATE()-2,113)

	-- ********************************************************************************************************
	-- commented by Fulsmita bhattacharjee on 02/12/2014 as the defination of UCD changed

 --   INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)
 --   SELECT 'UCD' AS TrackerType,COUNT(*) UCDCnt,@TrackerDate AS TrackerDate
 --   FROM ConsumerCreditPoints WITH(NOLOCK)
 --   WHERE ConsumerType = 1 
	----AND PackageType!=28
	--AND CONVERT(varchar(11),ExpiryDate,113) >= @Date

	-- Modified by Fulsmita bhattacharjee on 02/12/2014 as the defination of UCD changed , we now take distinct count from three different parameters
	-- the parameters are 1) All paid dealers 2) free listing dealers who has updated stock in last 30 days 3) dealer whose autobiz usage is >=50%

INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)
select 'Active Used Car Dealers' as TrackerType,
  count(distinct ConsumerId) as TrackerCount,
  @TrackerDate as TrackerDate
  from
(select distinct ConsumerId from Consumercreditpoints with (NoLock) where PackageType!=28 and
ConsumerType=1 and CONVERT(varchar(11),Expirydate,113)>=@TrackerDate
Union all
select distinct DealerId as ConsumerId  from SellInquiries with (NoLock) where PackageType=28 and CONVERT(varchar(11),LastUpdated ,113)>=@Stock_Update_Date
union all
  Select
(case  when NoOfUniqueCalls*100/ (CASE WHEN NoOfUniqueLeads=0 THEN 1 ELSE NoOfUniqueLeads END )>=50 then BranchId  end)
from (
SELECT ISNULL(B.BranchId,A.BranchId) BranchId,
    ISNULL(A.NoOfUniqueCalls,0) NoOfUniqueCalls, ISNULL(B.NoOfUniqueLeads,0) NoOfUniqueLeads
FROM
(SELECT TL.BranchId BranchId ,COUNT(DISTINCT TC.TC_LeadId) NoOfUniqueCalls
FROM TC_Calls AS TC WITH (NOLOCK)
JOIN TC_InquiriesLead AS TL WITH (NOLOCK) ON TC.TC_LeadId=TL.TC_LeadId
WHERE
TC.IsActionTaken=1
AND TL.TC_LeadInquiryTypeId=1
AND CONVERT(varchar(11),TC.ActionTakenOn ,113) BETWEEN @Autobiz_Usage_Date AND @TrackerDate
GROUP BY TL.BranchId) A
FULL OUTER JOIN
(SELECT TL.BranchId BranchId,COUNT(DISTINCT TL.TC_LeadId) NoOfUniqueLeads
FROM  TC_Lead AS TL WITH (NOLOCK)
JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId
where TCIL.TC_LeadInquiryTypeId=1
AND CONVERT(varchar(11),TL.LeadCreationDate ,113) BETWEEN @Autobiz_Usage_Date AND @TrackerDate
GROUP BY TL.BranchId) B ON   A.BranchId=B.BranchId
) C WHERE  NoOfUniqueCalls*100/ (CASE WHEN NoOfUniqueLeads=0 THEN 1 ELSE NoOfUniqueLeads END )>=50 ) L
	
	
  


-- *********************************************************************************************************************************************
---- Modified by Fulsmita Bhattacharjee,  on 30/01/2015 , DealerWise Unique Mobile from sellInquires and number masking calls
------- Modified by Chirag - 18-09-2015: Used Car Leads should now be taken from TC_BuyerInquiries. Below Query EXCLUDES InquirySource '93' as those indicate Unverified Leads

	--Dealer Responses (Dealer wise unique responses)
	INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)

	SELECT 'Dealer wise Unique Responses' AS TrackerType
	,sum(B.UniqueMobile) AS TrackerCount
	,@TrackerDate AS TrackerDate
FROM (

SELECT f.BranchId,Count(Distinct g.Mobile) as UniqueMobile
FROM TC_BuyerInquiries a WITH (NOLOCK)
		LEFT JOIN TC_InquirySource b WITH (NOLOCK) ON a.TC_InquirySourceId = b.ID
		LEFT JOIN TC_InquiriesLead d WITH (NOLOCK) ON a.TC_InquiriesLeadId = d.TC_InquiriesLeadId
		LEFT JOIN TC_Lead f WITH (NOLOCK) ON d.TC_LeadId = f.TC_LeadId
		LEFT JOIN TC_CustomerDetails g WITH (NOLOCK) ON f.TC_CustomerId = g.id
		where b.Id in (1,6,91,36,34,35,37,106,101,40,38)
		and CONVERT(VARCHAR(11), a.CreatedOn, 113) = @TrackerDate
		Group by f.BranchId
		
	)B



--SELECT 'Dealer wise Unique Responses' AS TrackerType
--	,sum(B.UniqueMobile) AS TrackerCount
--	,@TrackerDate AS TrackerDate
--FROM (
--	SELECT count(DISTINCT A.Mobile1) AS 'UniqueMobile' --,A.Dealers1 
--	FROM (
--		SELECT d1.id AS Dealers1
--			,MM.BuyerMobile AS Mobile1
--		FROM MM_Inquiries MM WITH (NOLOCK)
--		LEFT JOIN Dealers D1 WITH (NOLOCK) ON D1.ID = MM.ConsumerId
--		WHERE D1.TC_DealerTypeId = 1
--			AND CONVERT(VARCHAR(11), MM.CallStartDate, 113) = @TrackerDate
--			AND MM.CallStatus NOT IN ('NotConnected') -- CallStatus missed is i
--			--group by D1.id
		
--		UNION ALL
		
--		SELECT S.DealerId AS Dealers1
--			,C.Mobile AS Mobile1
--		FROM UsedCarPurchaseInquiries U WITH (NOLOCK)
--		INNER JOIN SellInquiries S WITH (NOLOCK) ON U.SellInquiryId = S.ID
--		INNER JOIN Customers C WITH (NOLOCK) ON C.Id = U.CustomerID
--		WHERE CONVERT(VARCHAR(11), U.RequestDateTime, 113) = @TrackerDate
--		-- including only web and mobile leads
--		--and u.sourceid in ('1','43')
--			--group by S.DealerId, convert(date,U.RequestDateTime)
--		) A
--	GROUP BY A.Dealers1
--	) B

-- *********************************************************************************************************************************************
---Added by Chirag - 18-09-2015: Used Car Leads should now be taken from TC_BuyerInquiries. Below Query INCLUDES InquirySource '93' as we are taking an overall count.

INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)


SELECT 'Overall Dealer wise Unique Responses' AS TrackerType
	,sum(B.UniqueMobile) AS TrackerCount
	,@TrackerDate AS TrackerDate
FROM (

SELECT f.BranchId,Count(Distinct g.Mobile) as UniqueMobile
FROM TC_BuyerInquiries a WITH (NOLOCK)
		LEFT JOIN TC_InquirySource b WITH (NOLOCK) ON a.TC_InquirySourceId = b.ID
		LEFT JOIN TC_InquiriesLead d WITH (NOLOCK) ON a.TC_InquiriesLeadId = d.TC_InquiriesLeadId
		LEFT JOIN TC_Lead f WITH (NOLOCK) ON d.TC_LeadId = f.TC_LeadId
		LEFT JOIN TC_CustomerDetails g WITH (NOLOCK) ON f.TC_CustomerId = g.id
		where b.Id in (1,6,91,36,34,35,37,106,101,40,38,93)
		and CONVERT(VARCHAR(11), a.CreatedOn, 113) = @TrackerDate
		Group by f.BranchId
		
	)B




--************************************************************************************************************************************
	-- Modified by Fulsmita Bhattacharjee on 22/04/2015 , MTD DealerWise Unique Mobile from sellInquires and number masking calls
	-- Modified by Chirag on 18-09-2015: Used Car Leads MTD should now be taken from TC_BuyerInquiries. Below Query EXCLUDES InquirySource '93' as those indicate Unverified Leads
	---Modified by Chirag on 06-10-2015: Used Cast on Date 'CreatedOn'
	INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)



	SELECT 'MTD Dealer wise Unique Responses' AS TrackerType
	,sum(B.UniqueMobile) AS TrackerCount
	,@TrackerDate AS TrackerDate
FROM (

SELECT f.BranchId,Count(Distinct g.Mobile) as UniqueMobile
FROM TC_BuyerInquiries a WITH (NOLOCK)
		LEFT JOIN TC_InquirySource b WITH (NOLOCK) ON a.TC_InquirySourceId = b.ID
		LEFT JOIN TC_InquiriesLead d WITH (NOLOCK) ON a.TC_InquiriesLeadId = d.TC_InquiriesLeadId
		LEFT JOIN TC_Lead f WITH (NOLOCK) ON d.TC_LeadId = f.TC_LeadId
		LEFT JOIN TC_CustomerDetails g WITH (NOLOCK) ON f.TC_CustomerId = g.id
		where b.Id in (1,6,91,36,34,35,37,106,101,40,38)
		and Cast(a.CreatedOn as Date) BETWEEN DATEADD(dd,-(DAY(@TrackerDate)-1),@TrackerDate) and @TrackerDate
		Group by f.BranchId
		
	)B




--SELECT 'MTD Dealer wise Unique Responses' AS TrackerType
--	,sum(B.UniqueMobile) AS TrackerCount
--	,@TrackerDate AS TrackerDate
--FROM (
--	SELECT count(DISTINCT A.Mobile1) AS 'UniqueMobile' --,A.Dealers1 
--	FROM (
--		SELECT d1.id AS Dealers1
--			,MM.BuyerMobile AS Mobile1
--		FROM  Dealers D1 WITH (NOLOCK)
--		JOIN MM_Inquiries MM  WITH (NOLOCK) ON D1.ID = MM.ConsumerId  -- ConsumerType?
--		WHERE D1.TC_DealerTypeId = 1
--			AND MM.CallStartDate BETWEEN CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@TrackerDate)-1),@TrackerDate),101) and @Date
--			AND MM.CallStatus <>'NotConnected' -- CallStatus missed is i
--			--group by D1.id
		
--		UNION ALL
		
--		SELECT S.DealerId AS Dealers1
--			,C.Mobile AS Mobile1
--		FROM SellInquiries S  WITH (NOLOCK)
--		INNER JOIN UsedCarPurchaseInquiries U WITH (NOLOCK) ON U.SellInquiryId = S.ID
--		INNER JOIN Customers C WITH (NOLOCK) ON C.Id = U.CustomerID
--		WHERE  U.RequestDateTime BETWEEN CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@TrackerDate)-1),@TrackerDate),101) and @Date
--		-- including only web and mobile leads
--		--and u.sourceid in ('1','43')
--			--group by S.DealerId, convert(date,U.RequestDateTime)
--		) A
--	GROUP BY A.Dealers1
--	) B



--************************************************************************************************************************************
---Added by Chirag on 18-09-2015: Used Car Leads should now be taken from TC_BuyerInquiries. Below Query INCLUDES InquirySource '93' as we are taking an overall count.
---Modified by Chirag on 06-10-2015: Used Cast on Date 'CreatedOn'

INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)


SELECT 'MTD Overall Dealer wise Unique Responses' AS TrackerType
	,sum(B.UniqueMobile) AS TrackerCount
	,@TrackerDate AS TrackerDate
FROM (

SELECT f.BranchId,Count(Distinct g.Mobile) as UniqueMobile
FROM TC_BuyerInquiries a WITH (NOLOCK)
		LEFT JOIN TC_InquirySource b WITH (NOLOCK) ON a.TC_InquirySourceId = b.ID
		LEFT JOIN TC_InquiriesLead d WITH (NOLOCK) ON a.TC_InquiriesLeadId = d.TC_InquiriesLeadId
		LEFT JOIN TC_Lead f WITH (NOLOCK) ON d.TC_LeadId = f.TC_LeadId
		LEFT JOIN TC_CustomerDetails g WITH (NOLOCK) ON f.TC_CustomerId = g.id
		where b.Id in (1,6,91,36,34,35,37,106,101,40,38,93)
		and Cast(a.CreatedOn as Date) BETWEEN DATEADD(dd,-(DAY(@TrackerDate)-1),@TrackerDate) and @TrackerDate
		Group by f.BranchId
		
	)B




--************************************************************************************************************************************
	-- Modified by Fulsmita Bhattacharjee, Live Listings posted by Dealers

	INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)
SELECT 'Livelistings-Dealer' AS TrackerType --DEALERS
	,COUNT(DISTINCT ProfileId) AS Cnt
	,@TrackerDate AS TrackerDate
FROM LiveListingsDailyLog WITH (NOLOCK)
WHERE CONVERT(varchar(11),AsOnDate, 113) =@TrackerDate 
and SellerType=1 





--************************************************************************************************************************************
	-- Modified by Chirag Perla, Live Listings posted by Individuals --- 23_07_2015

	INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)
SELECT 'Livelistings-Indiv' AS TrackerType --INDIVIDUALS
	,COUNT(DISTINCT ProfileId) AS Cnt
	,@TrackerDate AS TrackerDate
FROM LiveListingsDailyLog WITH (NOLOCK)
WHERE CONVERT(varchar(11),AsOnDate, 113) =@TrackerDate 
and SellerType=2




--************************************************************************************************************************************
	-- Modified by Fulsmita Bhattacharjee, Unique Cookie on 16/12/2014


  INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
SELECT 'Unique Cookie for PQ' AS TrackerType
    ,COUNT(distinct CWCookieValue) as TrackerCount
	,@TrackerDate as TrackerDate
	from PQ_ClientInfo WITH(NOLOCK)
	where CONVERT(VARCHAR(11), EntryDate, 113) = @TrackerDate

--************************************************************************************************************************************
-- Modified by Fulsmita Bhattacharjee, RO Value on 30/12/2014
	 INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
SELECT 'RO Value' AS TrackerType
    ,sum(FinalROValue) as TrackerCount
	,@TrackerDate as TrackerDate
	from ESM_FreezeROReceiveLogs with (NOLOCk)
	 where CONVERT(VARCHAR(11), UpdatedOn, 113) = @TrackerDate


--************************************************************************************************************************************
-- Modified by Fulsmita Bhattacharjee, ES Sales funnel at 10% probability on 30/12/2014
	 	 INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
SELECT 'ES Funnel 10' AS TrackerType
    ,sum(ProposedAmount) as TrackerCount
	,@TrackerDate as TrackerDate
	from ESM_ProposedProductLogs with (NOLOCk)
	 where  Probability =10  
	 and CONVERT(VARCHAR(11), UpdatedOn, 113) = @TrackerDate


--************************************************************************************************************************************
-- Modified by Fulsmita Bhattacharjee, ES Sales funnel at 30% probability on 30/12/2014
	  INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
SELECT 'ES Funnel 30' AS TrackerType
    ,sum(ProposedAmount) as TrackerCount
	,@TrackerDate as TrackerDate
	from ESM_ProposedProductLogs with (NOLOCk)
	 where  Probability =30
	 and CONVERT(VARCHAR(11), UpdatedOn, 113) = @TrackerDate


--************************************************************************************************************************************
-- Modified by Fulsmita Bhattacharjee, ES Sales funnel at 50% probability on 30/12/2014
	  INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
SELECT 'ES Funnel 50' AS TrackerType
    ,sum(ProposedAmount) as TrackerCount
	,@TrackerDate as TrackerDate
	from ESM_ProposedProductLogs with (NOLOCk)
	 where  Probability =50  
	 and CONVERT(VARCHAR(11), UpdatedOn, 113) = @TrackerDate


--************************************************************************************************************************************
-- Modified by Fulsmita Bhattacharjee, ES Sales funnel at 70% probability on 30/12/2014
	  INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
SELECT 'ES Funnel 70' AS TrackerType
    ,sum(ProposedAmount) as TrackerCount
	,@TrackerDate as TrackerDate
	from ESM_ProposedProductLogs with (NOLOCk)
	 where  Probability =70  
	 and CONVERT(VARCHAR(11), UpdatedOn, 113) = @TrackerDate


--************************************************************************************************************************************
-- Modified by Fulsmita Bhattacharjee, ES Sales funnel at 90% probability on 30/12/2014
	  INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
SELECT 'ES Funnel 90' AS TrackerType
    ,sum(ProposedAmount) as TrackerCount
	,@TrackerDate as TrackerDate
	from ESM_ProposedProductLogs with (NOLOCk)
	 where  Probability =90  
	 and CONVERT(VARCHAR(11), UpdatedOn, 113) = @TrackerDate

----************************************************************************************************************************************
---- Modified by Fulsmita Bhattacharjee, NCD Dealer Sales on 08/01/2015
	 
 INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
SELECT 'NCD Dealer Sales' AS TrackerType
    ,sum(B.finalamount) as TrackerCount
	,@TrackerDate as TrackerDate
	from RVN_DealerPackageFeatures  A with (NOLOCk) inner join 
	DCRM_PaymentTransaction B with (NoLock) on A.TransactionId=B.TransactionId
	 where CONVERT(VARCHAR(11), ApprovedOn,113) = @TrackerDate
	 and PackageId in (43,50,51,56,57,58,59,60,64,65,66,67,68,69,70,71) -- package Id for NCD

----************************************************************************************************************************************
---- Modified by Fulsmita Bhattacharjee, UCD Dealer Sales on 08/01/2015

	 	  INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
SELECT 'UCD Dealer Sales' AS TrackerType
    ,sum(B.finalamount) as TrackerCount
	,@TrackerDate as TrackerDate
	from RVN_DealerPackageFeatures  A with (NOLOCk) inner join 
	DCRM_PaymentTransaction B with (NoLock) on A.TransactionId=B.TransactionId
	 where CONVERT(VARCHAR(11), ApprovedOn,113) = @TrackerDate
	 and PackageId in (30,31,32,33,34,36,37,46,47,52,53,54,55,61,62,63) -- package Id for UCD


----************************************************************************************************************************************
--Inserted By Prajakta Gunjal on 02/03/2015 in replacement of above three commented trackers 'Formfill InboundLeads','FormFill DistinctMobile',
--'DistinctCookie As FormFillCustomers' and added tracker 'FormFill (DistinctMobile+InboundLeads+DealerLocator)','DealerWise Unique Mobile Including NumberMasking-NCD',
--'DistinctCookie As FormFillCustomers'
--Numerator for New Car Lead Conversion Rate 'FormFill (DistinctMobile+InboundLeads+DealerLocator)'
--Modified by Prajakta Gunjal on 25/03/2015 added three more ids of table TC_InquirySource - 6,95,96 as carwale spot page is replaced by 
--CarWale Advertisement,Carwale Dealershowroom M SiteCarwale Dealershowroom Desktop
-- Modified by Fulsmita Bhattacharjee on 10/04/2015, for the new financial year new logic for all new car leads trackers
-- Modified by Fulsmita Bhattacharjee on 02/06/2015, ES Dealer toyota Exception added as TL.BranchId not in 10216
-- Commented by Fulsmita Bhattacharjee on 24/07/2014 , definition changed by Product Manager : Gokul Bhadri
 

---- Numerator New Car Leads for Dealers , all dealer leads destination table is TC_NewCarInquiries
--INSERT INTO SC.DailyTracker (
--	TrackerType
--	,TrackerCount
--	,TrackerDate
--	)
--SELECT 'New Car Leads DS' AS TrackerType
--	,sum(x.SumLeads) AS TrackerCount
--	,@TrackerDate AS TrackerDate
--FROM (
--	SELECT count(DISTINCT CD.Mobile) AS SumLeads
--		,C.Make
--	FROM TC_NewCarInquiries A WITH (NOLOCK)
--	INNER JOIN TC_InquiriesLead l WITH (NOLOCK) ON l.TC_InquiriesLeadId = A.TC_InquiriesLeadId
--	INNER JOIN TC_Lead TL WITH (NOLOCK) ON TL.TC_LeadId = l.TC_LeadId
--	INNER JOIN TC_CustomerDetails CD WITH (NOLOCK) ON CD.id = TL.TC_CustomerId
--	INNER JOIN TC_InquirySource B WITH (NOLOCK) ON A.TC_InquirySourceid = B.Id
--	INNER JOIN vwMMV C WITH (NOLOCK) ON A.VersionId = C.VersionId
--	WHERE b.Product_Newcar = 1
--		AND CONVERT(VARCHAR(11), A.CreatedOn, 113) = @TrackerDate
--		AND TL.BranchId not in (9735,10216) -- we exclude Reno India Dealer
--		--AND C.MakeId NOT IN (45)-- we exclude renault India leads as this are ES Leads
--	GROUP BY C.Make
--	) x

	----************************************************************************************************************************************
-- Modified by Fulsmita Bhattacharjee on 24/07/2015, according to  New Definition, we are consedering dealer wise leads
-- Modified by Chirag Perla on 13-11-2015: Used 'DealerExclusion' table to exclude non-DS Dealers
-- Modified by Chirag Perla on 17-12-2015: Changed 'New Car Leads DS' logic to exclude CarTrade Leads(LeadClickSource=500) and Leads that have already been received in the same month.	
		
		
CREATE Table #temp
(Already_Leads varchar(50))


insert into #temp

SELECT CAST(tc.Mobile AS VARCHAR(40)) +'-'+ CAST(f.BranchId AS VARCHAR(40)) AS Already_Leads
FROM dealers as d JOIN TC_CustomerDetails TC with (NoLock) ON d.ID = TC.BranchId
    JOIN TC_Lead as f with (NoLock) on TC.Id = F.TC_CustomerId
    JOIN TC_InquiriesLead c  with (NoLock) ON C.TC_LeadId = f.TC_LeadId
    JOIN TC_NewCarInquiries a  with (NoLock)  ON a.TC_InquiriesLeadId = c.TC_InquiriesLeadId
	LEFT JOIN PQDealerAdLeads pqd with (nolock) ON pqd.PushStatus = a.TC_NewCarInquiriesId
    LEFT JOIN TC_InquirySource b with (NoLock) on a.TC_InquirySourceId = b.ID
    where CONVERT(VARCHAR(11), a.CreatedOn, 113) between @TrackerMonthStart and @TrackerPrevDay
	and f.BranchId not in ( Select DealerId From DealerExclusion ) -- Excluded Dealers
    and b.Product_Newcar = 1  
	and isnull(pqd.LeadClickSource,0) <> 500


;with CTE as (

SELECT CAST(tc.Mobile AS VARCHAR(40)) +'-'+ CAST(f.BranchId AS VARCHAR(40)) AS DISTINCT_MOBILE
FROM dealers as d JOIN TC_CustomerDetails TC with (NoLock) ON d.ID = TC.BranchId
    JOIN TC_Lead as f with (NoLock) on TC.Id = F.TC_CustomerId
    JOIN TC_InquiriesLead c  with (NoLock) ON C.TC_LeadId = f.TC_LeadId
    JOIN TC_NewCarInquiries a  with (NoLock)  ON a.TC_InquiriesLeadId = c.TC_InquiriesLeadId
	LEFT JOIN PQDealerAdLeads pqd with (nolock) ON pqd.PushStatus = a.TC_NewCarInquiriesId
    LEFT JOIN TC_InquirySource b with (NoLock) on a.TC_InquirySourceId = b.ID
    where CONVERT(VARCHAR(11), a.CreatedOn, 113) = @TrackerDate
	and f.BranchId not in ( Select DealerId From DealerExclusion ) -- Excluded Dealers
    and b.Product_Newcar = 1  
	and isnull(pqd.LeadClickSource,0) <> 500
)

INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)

SELECT 'New Car Leads DS' AS TrackerType
	,count(Distinct x.DISTINCT_MOBILE)  AS TrackerCount
	,@TrackerDate AS TrackerDate 
	 
FROM  
(
Select DISTINCT_MOBILE
FROM CTE
where DISTINCT_MOBILE not in (Select ISNULL(Already_Leads,'') from #temp)
)x

Drop Table #temp
		
				

		
		
--		INSERT INTO SC.DailyTracker (
--	TrackerType
--	,TrackerCount
--	,TrackerDate
--	)
--	SELECT 'New Car Leads DS' AS TrackerType
--	,sum(x.Mobile)  AS TrackerCount
--	,@TrackerDate AS TrackerDate
--FROM  (
--SELECT count(distinct tc.Mobile) as Mobile
----, f.BranchId 
--FROM dealers as d JOIN TC_CustomerDetails TC with (NoLock) ON d.ID = TC.BranchId
--    JOIN TC_Lead as f with (NoLock) on TC.Id = F.TC_CustomerId
--    JOIN TC_InquiriesLead c  with (NoLock) ON C.TC_LeadId = f.TC_LeadId
--    JOIN TC_NewCarInquiries a  with (NoLock)  ON a.TC_InquiriesLeadId = c.TC_InquiriesLeadId
--    LEFT JOIN TC_InquirySource b with (NoLock) on a.TC_InquirySourceId = b.ID
--    where CONVERT(VARCHAR(11), a.CreatedOn, 113) = @TrackerDate
--	and f.BranchId not in ( Select DealerId From DealerExclusion ) -- Excluded Dealers
--    and b.Product_Newcar = 1
--    group by f.BranchId 
--    )x

	----************************************************************************************************************************************

-- Denominator New Car Leads for Dealers we do not consider cookies from ES Dealers which are 9735 and 9350 DealerId
-- Modified by Fulsmita Bhattacharjee on 02/06/2015,ES Dealer toyota Exception added as TL.BranchId not in 10216
-- Modified by Fulsmita Bhattacharjee on 24/07/2015, changed group by Make to group by dealerid due to change of definition, also removed two used cars dealers ,1393,5454
-- Modified by Chirag Perla on 06-10-2015/: Added an extra where clause "A.PQPageId <> 39" to exclude the Impressions being recorded for "Featured" models.
-- Modified by Chirag Perla on 13-11-2015: Used 'DealerExclusion' table to exclude non-DS Dealers
INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)
SELECT 'DS Denominator New Car Leads' AS Trackertype
	,Sum(x.CookieSum) AS Trackercount
	,@TrackerDate AS TrackerDate
FROM (
	SELECT count(DISTINCT D.CWCookieValue) AS CookieSum
		,e.MakeId
	FROM NewCarPurchaseInquiries A WITH (NOLOCK)
	INNER JOIN PQ_DealerSponsoredAd_ViewLogs B WITH (NOLOCK) ON A.Id = B.PQId
	INNER JOIN PQ_DealerSponsored C WITH (NOLOCK) ON C.Id = B.CampaignId
	INNER JOIN pq_clientinfo D WITH (NOLOCK) ON B.PQId = D.PQId
	INNER JOIN vwMMV E WITH (NOLOCK) ON A.CarVersionId = E.VersionId
	WHERE C.DealerId NOT IN ( Select DealerId From DealerExclusion ) -- Excluded Dealers
		AND CONVERT(VARCHAR(11), A.RequestDateTime, 113) = @TrackerDate
		AND A.PQPageId <> 39
	GROUP BY e.MakeId
	) x

-- Denominator New Car Leads for ES
-- Modified by Fulsmita Bhattacharjee on 02/06/2015,ES Dealer toyota Exception added as TL.BranchId in 10216
---- Modified by Chirag Perla on 13-11-2015: Used 'DealerExclusion' table to include only ES Dealers
INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)
SELECT 'ES Denominator New Car Leads' AS Trackertype
	,Sum(x.CookieSum) AS TrackerCount
	,@TrackerDate AS TrackerDate
FROM (
	SELECT count(DISTINCT D.CWCookieValue) AS CookieSum
		,E.Make
	FROM NewCarPurchaseInquiries A WITH (NOLOCK)
	INNER JOIN PQ_DealerSponsoredAd_ViewLogs B WITH (NOLOCK) ON A.Id = B.PQId
	INNER JOIN PQ_DealerSponsored C WITH (NOLOCK) ON C.Id = B.CampaignId
	INNER JOIN pq_clientinfo D WITH (NOLOCK) ON B.PQId = D.PQId
	INNER JOIN vwMMV E WITH (NOLOCK) ON A.CarVersionId = E.VersionId
	WHERE C.DealerId IN (Select DealerId From DealerExclusion where IsES = 1 ) -- These are ES Dealers
		AND CONVERT(VARCHAR(11), A.RequestDateTime, 113) = @TrackerDate
	GROUP BY E.Make
	) x

-- Denominator New Car Leads OverAll
INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)
SELECT 'OverAll Denominator New Car Leads' AS Trackertype
	,Sum(x.CookieSum) AS TrackerCount
	,@TrackerDate AS TrackerDate
FROM (
	SELECT count(DISTINCT D.CWCookieValue) AS CookieSum
		,E.Make
	FROM NewCarPurchaseInquiries A WITH (NOLOCK)
	INNER JOIN PQ_DealerSponsoredAd_ViewLogs B WITH (NOLOCK) ON A.Id = B.PQId
	INNER JOIN PQ_DealerSponsored C WITH (NOLOCK) ON C.Id = B.CampaignId
	INNER JOIN pq_clientinfo D WITH (NOLOCK) ON B.PQId = D.PQId
	INNER JOIN vwMMV E WITH (NOLOCK) ON A.CarVersionId = E.VersionId
		AND CONVERT(VARCHAR(11), A.RequestDateTime, 113) = @TrackerDate
	GROUP BY E.Make
	) x

--Numerator for New Car Leads OverAll
-- PQ Dealer adLeads this leads come in PQ Page includes all the form fill
INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)
SELECT 'New Car Leads OverAll' AS TrackerType
	,sum(y.MobileUnique) AS TrackerCount
	,@TrackerDate AS TrackerDate
FROM (
	SELECT count(DISTINCT x.UniqueMobile) AS MobileUnique
	FROM (
		SELECT b.Make AS Make1
			,a.mobile AS UniqueMobile
		FROM PQDealerAdLeads a WITH (NOLOCK)
		LEFT JOIN vwMMV b WITH (NOLOCK) ON a.versionid = b.VersionId
		WHERE b.Make IS NOT NULL
			AND CONVERT(VARCHAR(11), A.RequestDateTime, 113) = @TrackerDate
		--GROUP BY b.Make
		
		UNION
		
		-- inbound leads
		SELECT v.make AS Make1
			,cm.Mobile AS UniqueMobile
		FROM CRM_Leads l WITH (NOLOCK)
		LEFT JOIN CRM_CarBasicData d WITH (NOLOCK) ON d.LeadId = l.ID
		LEFT JOIN vwMMV v WITH (NOLOCK) ON v.versionId = d.VersionId
		LEFT JOIN CRM_LeadSource s WITH (NOLOCK) ON s.LeadId = l.ID
		LEFT JOIN LA_Agencies a WITH (NOLOCK) ON a.Id = s.SourceId
		LEFT JOIN CRM_Customers cm WITH (NOLOCK) ON cm.ID = l.CNS_CustId
		WHERE CONVERT(VARCHAR(11), l.createdon, 113) = @TrackerDate
			AND a.Id = 87 -- CarWale Toll Free leads
			--GROUP BY v.make
		
		UNION
		
		-- dealer locator form fill
		SELECT v.Make AS Make1
			,g.mobile AS UniqueMobile
		FROM TC_NewCarInquiries a WITH (NOLOCK)
		LEFT JOIN TC_InquirySource b WITH (NOLOCK) ON a.TC_InquirySourceId = b.ID
		LEFT JOIN TC_InquiriesLead d WITH (NOLOCK) ON a.TC_InquiriesLeadId = d.TC_InquiriesLeadId
		LEFT JOIN TC_Lead f WITH (NOLOCK) ON d.TC_LeadId = f.TC_LeadId
		LEFT JOIN TC_CustomerDetails g WITH (NOLOCK) ON f.TC_CustomerId = g.id
		LEFT JOIN vwMMV v WITH (NOLOCK) ON a.versionid = v.VersionId
		WHERE CONVERT(VARCHAR(11), A.CreatedOn, 113) = @TrackerDate
			AND b.id IN (
				33
				,6
				,95
				,96
				) -- considering  CarWale Advertisement, CarWale Spot Page, Carwale Dealershowroom M Site and Desktop
		--GROUP BY v.Make
		
		UNION
		
		-- we consider all th leads from NCS_TDReq table, leads includes test drive, upcoming cars, all finance leads website and mobile
		SELECT c.NAME AS Make1
			,mobile AS UniqueMobile
		FROM NCS_TDReq td WITH (NOLOCK)
		INNER JOIN vwMMV v WITH (NOLOCK) ON td.VersionId = v.VersionId
		INNER JOIN CarVersions cv WITH (NOLOCK) ON cv.ID = v.VersionId
		INNER JOIN CarModels cm WITH (NOLOCK) ON cm.ID = cv.CarModelId
		INNER JOIN CarMakes c WITH (NOLOCK) ON c.ID = cm.CarMakeId
		INNER JOIN NCS_Leadtypes d WITH (NOLOCK) ON td.LeadType = d.Id
		WHERE CONVERT(VARCHAR(11), td.CreatedOn, 113) = @TrackerDate
			--AND d.Id IN (
			--	3
			--	,4
			--	) -- 3 and 4 are test drive and upcoming cars source id 
			--GROUP BY c.NAME
		) x
	GROUP BY x.Make1
	) y

----************************************************************************************************************************************
-- Modified by Fulsmita on 22/04/2015 ,MTD  New Car Leads for DS
-- Modified by Fulsmita Bhattacharjee on 02/06/2015, ES Dealer toyota Exception added as TL.BranchId not in 10216
-- Commented by Fulsmita Bhattacharjee on 24/07/2015 due to definition changed
	
--INSERT INTO SC.DailyTracker (
--	TrackerType
--	,TrackerCount
--	,TrackerDate
--	)
--SELECT 'MTD New Car Leads DS' AS TrackerType
--	,sum(x.SumLeads) AS TrackerCount
--	,@TrackerDate AS TrackerDate
--FROM (
--	SELECT count(DISTINCT CD.Mobile) AS SumLeads
--		,C.Make
--	FROM TC_NewCarInquiries A WITH (NOLOCK)
--	INNER JOIN TC_InquiriesLead l WITH (NOLOCK) ON l.TC_InquiriesLeadId = A.TC_InquiriesLeadId
--	INNER JOIN TC_Lead TL WITH (NOLOCK) ON TL.TC_LeadId = l.TC_LeadId
--	INNER JOIN TC_CustomerDetails CD WITH (NOLOCK) ON CD.id = TL.TC_CustomerId
--	INNER JOIN TC_InquirySource B WITH (NOLOCK) ON A.TC_InquirySourceid = B.Id
--	INNER JOIN vwMMV C WITH (NOLOCK) ON A.VersionId = C.VersionId
--	WHERE b.Product_Newcar = 1
--		AND    A.CreatedOn BETWEEN CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@TrackerDate)-1),@TrackerDate),101) and @Date
--		AND TL.BranchId not in (9735,10216) -- we exclude Reno India Dealer
--		--AND C.MakeId NOT IN (45)-- we exclude renault India leads as this are ES Leads
--	GROUP BY C.Make
--	) x 

----************************************************************************************************************************************
-- Modified by Fulsmita Bhattacharjee on 24/07/2015, according to  New Definition, we are consedering dealer wise leads, also removed two Used Car Daelers ,1393,5454
-- Modified by Chirag Perla on 13-11-2015: Used 'DealerExclusion' table to exclude non-DS Dealers

		INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)
	SELECT 'MTD New Car Leads DS' AS TrackerType
	,sum(x.Mobile)  AS TrackerCount
	,@TrackerDate AS TrackerDate
FROM  (
SELECT count(distinct tc.Mobile) as Mobile
--, f.BranchId 
FROM dealers as d JOIN TC_CustomerDetails TC with (NoLock) ON d.ID = TC.BranchId
    JOIN TC_Lead as f with (NoLock) on TC.Id = F.TC_CustomerId
    JOIN TC_InquiriesLead c  with (NoLock) ON C.TC_LeadId = f.TC_LeadId
    JOIN TC_NewCarInquiries a  with (NoLock)  ON a.TC_InquiriesLeadId = c.TC_InquiriesLeadId
    LEFT JOIN TC_InquirySource b with (NoLock) on a.TC_InquirySourceId = b.ID
    where a.CreatedOn BETWEEN CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@TrackerDate)-1),@TrackerDate),101) and @Date
	and f.BranchId not in ( Select DealerId From DealerExclusion )  --Excluded Dealers
    and b.Product_Newcar = 1
    group by f.BranchId 
    )x

----************************************************************************************************************************************
-- Modified by Fulsmita on 22/04/2015 ,MTD Denominator New Car Leads for DS
-- Modified by Fulsmita Bhattacharjee on 02/06/2015,ES Dealer toyota Exception added as TL.BranchId not in 10216
-- Modified by Fulsmita on 24/7/2015 , chnaged group by Make to group by Dealerid, due to definition changes , also removed two Used Car Daelers ,1393,5454
-- Modified by Chirag Perla on 09-10-2015: Added an extra where clause "A.PQPageId <> 39" to exclude the Impressions being recorded for "Featured" models.
-- Modified by Chirag Perla on 13-11-2015: Used 'DealerExclusion' table to exclude non-DS Dealers

	INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)
SELECT 'MTD DS Denominator New Car Leads' AS Trackertype
	,Sum(x.CookieSum) AS Trackercount
	,@TrackerDate AS TrackerDate
FROM (
	SELECT count(DISTINCT D.CWCookieValue) AS CookieSum
		,e.MakeId
	FROM NewCarPurchaseInquiries A WITH (NOLOCK)
	INNER JOIN PQ_DealerSponsoredAd_ViewLogs B WITH (NOLOCK) ON A.Id = B.PQId
	INNER JOIN PQ_DealerSponsored C WITH (NOLOCK) ON C.Id = B.CampaignId
	INNER JOIN pq_clientinfo D WITH (NOLOCK) ON B.PQId = D.PQId
	INNER JOIN vwMMV E WITH (NOLOCK) ON A.CarVersionId = E.VersionId
	WHERE C.DealerId NOT IN ( Select DealerId From DealerExclusion ) -- Excluded Dealers
		AND A.RequestDateTime BETWEEN CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@TrackerDate)-1),@TrackerDate),101) and @Date
		AND A.PQPageId <> 39
	GROUP BY e.MakeId
	) x


----************************************************************************************************************************************
-- Modified by Fulsmita on 22/04/2015 ,MTD Denominator New Car Leads for ES
-- Modified by Fulsmita Bhattacharjee on 02/06/2015, ES Dealer toyota Exception added as TL.BranchId  in 10216
-- Modified by Chirag Perla on 09-10-2015: Added an extra where clause "A.PQPageId <> 39" to exclude the Impressions being recorded for "Featured" models.
-- Modified by Chirag Perla on 13-11-2015: Used 'DealerExclusion' table to include only ES Dealers

INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)
SELECT 'MTD ES Denominator New Car Leads' AS Trackertype
	,Sum(x.CookieSum) AS TrackerCount
	,@TrackerDate AS TrackerDate
FROM (
	SELECT count(DISTINCT D.CWCookieValue) AS CookieSum
		,E.Make
	FROM NewCarPurchaseInquiries A WITH (NOLOCK)
	INNER JOIN PQ_DealerSponsoredAd_ViewLogs B WITH (NOLOCK) ON A.Id = B.PQId
	INNER JOIN PQ_DealerSponsored C WITH (NOLOCK) ON C.Id = B.CampaignId
	INNER JOIN pq_clientinfo D WITH (NOLOCK) ON B.PQId = D.PQId
	INNER JOIN vwMMV E WITH (NOLOCK) ON A.CarVersionId = E.VersionId
	WHERE C.DealerId IN ( Select DealerId From DealerExclusion where IsES = 1 ) -- These are ES Dealers
		AND  A.RequestDateTime BETWEEN CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@TrackerDate)-1),@TrackerDate),101) and @Date
		AND A.PQPageId <> 39
	GROUP BY E.Make
	) x

----************************************************************************************************************************************
-- Modified by Fulsmita on 22/04/2015 ,MTD Denominator New Car Leads Overall
-- Modified by Chirag Perla on 09-10-2015: Added an extra where clause "A.PQPageId <> 39" to exclude the Impressions being recorded for "Featured" models.

	-- Denominator New Car Leads OverAll
INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)
SELECT 'MTD OverAll Denominator New Car Leads' AS Trackertype
	,Sum(x.CookieSum) AS TrackerCount
	,@TrackerDate AS TrackerDate
FROM (
	SELECT count(DISTINCT D.CWCookieValue) AS CookieSum
		,E.Make
	FROM NewCarPurchaseInquiries A WITH (NOLOCK)
	INNER JOIN PQ_DealerSponsoredAd_ViewLogs B WITH (NOLOCK) ON A.Id = B.PQId
	INNER JOIN PQ_DealerSponsored C WITH (NOLOCK) ON C.Id = B.CampaignId
	INNER JOIN pq_clientinfo D WITH (NOLOCK) ON B.PQId = D.PQId
	INNER JOIN vwMMV E WITH (NOLOCK) ON A.CarVersionId = E.VersionId
		AND  A.RequestDateTime BETWEEN CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@TrackerDate)-1),@TrackerDate),101) and @Date
		AND A.PQPageId <> 39
	GROUP BY E.Make
	) x


----************************************************************************************************************************************
-- Modified by Fulsmita on 22/04/2015 ,MTD New Car Leads OverAll

	INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)
SELECT 'MTD New Car Leads OverAll' AS TrackerType
	,sum(y.MobileUnique) AS TrackerCount
	,@TrackerDate AS TrackerDate
FROM (
	SELECT count(DISTINCT x.UniqueMobile) AS MobileUnique
	FROM (
		SELECT b.Make AS Make1
			,a.mobile AS UniqueMobile
		FROM PQDealerAdLeads a WITH (NOLOCK)
		LEFT JOIN vwMMV b WITH (NOLOCK) ON a.versionid = b.VersionId
		WHERE b.Make IS NOT NULL
			AND A.RequestDateTime BETWEEN CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@TrackerDate)-1),@TrackerDate),101) and @Date
		--GROUP BY b.Make
		
		UNION
		
		-- inbound leads
		SELECT v.make AS Make1
			,cm.Mobile AS UniqueMobile
		FROM CRM_Leads l WITH (NOLOCK)
		LEFT JOIN CRM_CarBasicData d WITH (NOLOCK) ON d.LeadId = l.ID
		LEFT JOIN vwMMV v WITH (NOLOCK) ON v.versionId = d.VersionId
		LEFT JOIN CRM_LeadSource s WITH (NOLOCK) ON s.LeadId = l.ID
		LEFT JOIN LA_Agencies a WITH (NOLOCK) ON a.Id = s.SourceId
		LEFT JOIN CRM_Customers cm WITH (NOLOCK) ON cm.ID = l.CNS_CustId
		WHERE  l.createdon BETWEEN CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@TrackerDate)-1),@TrackerDate),101) and @Date
			AND a.Id = 87 -- CarWale Toll Free leads
			--GROUP BY v.make
		
		UNION
		
		-- dealer locator form fill
		SELECT v.Make AS Make1
			,g.mobile AS UniqueMobile
		FROM TC_NewCarInquiries a WITH (NOLOCK)
		LEFT JOIN TC_InquirySource b WITH (NOLOCK) ON a.TC_InquirySourceId = b.ID
		LEFT JOIN TC_InquiriesLead d WITH (NOLOCK) ON a.TC_InquiriesLeadId = d.TC_InquiriesLeadId
		LEFT JOIN TC_Lead f WITH (NOLOCK) ON d.TC_LeadId = f.TC_LeadId
		LEFT JOIN TC_CustomerDetails g WITH (NOLOCK) ON f.TC_CustomerId = g.id
		LEFT JOIN vwMMV v WITH (NOLOCK) ON a.versionid = v.VersionId
		WHERE A.CreatedOn BETWEEN CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@TrackerDate)-1),@TrackerDate),101) and @Date
			AND b.id IN (
				33
				,6
				,95
				,96
				) -- considering  CarWale Advertisement, CarWale Spot Page, Carwale Dealershowroom M Site and Desktop
		--GROUP BY v.Make
		
		UNION
		
		-- we consider all th leads from NCS_TDReq table, leads includes test drive, upcoming cars, all finance leads website and mobile
		SELECT c.NAME AS Make1
			,mobile AS UniqueMobile
		FROM NCS_TDReq td WITH (NOLOCK)
		INNER JOIN vwMMV v WITH (NOLOCK) ON td.VersionId = v.VersionId
		INNER JOIN CarVersions cv WITH (NOLOCK) ON cv.ID = v.VersionId
		INNER JOIN CarModels cm WITH (NOLOCK) ON cm.ID = cv.CarModelId
		INNER JOIN CarMakes c WITH (NOLOCK) ON c.ID = cm.CarMakeId
		INNER JOIN NCS_Leadtypes d WITH (NOLOCK) ON td.LeadType = d.Id
		WHERE td.CreatedOn BETWEEN CONVERT(VARCHAR(25),DATEADD(dd,-(DAY(@TrackerDate)-1),@TrackerDate),101) and @Date
			--AND d.Id IN (
			--	3
			--	,4
			--	) -- 3 and 4 are test drive and upcoming cars source id 
			--GROUP BY c.NAME
		) x
	GROUP BY x.Make1
	) y


----************************************************************************************************************************************
---- Modified by Fulsmita Bhattacharjee,  on 11/02/2015 , NCD Dealers Active including PQ_Sponsored and Dealer Locator
---- Modified by Fulsmita Bhattacharjee,  on 02/06/2015 , ADDED  Exception Toyota dealer Id as 10216
---- Modified by Chirag Perla on 13-11-2015: Used 'DealerExclusion' table to exclude non-DS Dealers
---- Modified by Chirag Perla on 26-11-2015: Commented the Tracker, as it is no longer used(Also Dealer_NewCar table is not avaialbe anymore).


--	  INSERT INTO SC.DailyTracker (
--    TrackerType
--    ,TrackerCount
--    ,TrackerDate
--    )
--SELECT
--'NCD' as TrackerType,
-- COUNT(DealerId) as TrackerCount,
-- @TrackerDate as TrackerDate
--FROM (
--SELECT  PD.DealerId -- Dealers for PQ page
--FROM PQ_DealerSponsored PD WITH(NOLOCK)
--JOIN  PQ_DealerCitiesModels PC WITH(NOLOCK) on PD.id = PC.pqid 
--WHERE  IsActive = '1'
--AND CONVERT(DATE,ENDDATE) >= GETDATE()
--AND CONVERT(DATE,StartDate) <= GETDATE()
--AND  ( ISNULL( totalcount,0 ) < ISNULL( totalgoal,0) or TotalCount is null or TotalGoal is null)
--AND PD.DealerId not in ( Select DealerId From DealerExclusion ) --Excluded Dealers
--UNION 
--SELECT dn.TcDealerId  as DealerId --  Locate Dealers
--FROM ncd_dealers nd WITH(NOLOCK)
--LEFT OUTER JOIN dealer_newcar dn WITH (NOLOCK) on nd.dealerid = dn.id
--LEFT OUTER JOIN cities c WITH (NOLOCK) on dn.cityid = c.id 
--WHERE nd.ispremium = '1'  -- using Dealer locator
--and nd.isactive = '1'
--and dn.tcdealerid != '-1'  -- It should be Dealers table
--)a

--********************************************************************************************************
-- Modified By Fulsmita Bhattacharjee on 11/02/2015 , Dealers Added UCD

INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)
SELECT 'Dealers Joined UCD' AS TrackerType
	,count(DISTINCT d.id) AS TrackerCount
	,@TrackerDate AS TrackerDate
FROM Dealers d with (NoLock)
INNER JOIN ConsumerCreditPoints c with (NoLock) ON d.ID = c.ConsumerId
WHERE c.consumertype = 1
	AND CONVERT(VARCHAR(11), d.JoiningDate, 113) = @TrackerDate
	AND c.expirydate > @TrackerDate
	and TC_DealerTypeId in(1,3)

--********************************************************************************************************
-- Modified By Fulsmita Bhattacharjee on 11/02/2015 , Dealers Left UCD

INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)
SELECT 'Dealers Left UCD' AS TrackerType
	,count(DISTINCT ConsumerId) AS TrackerCount
	,@TrackerDate AS TrackerDate
FROM ConsumerCreditPoints with (NoLock)
WHERE ConsumerType = 1
	AND CONVERT(VARCHAR(11), ExpiryDate, 113) = @TrackerDate


--********************************************************************************************************
-- Modified By Prajakta Gunjal on 27/02/2015 , Warranties Approved
-- Modified by Prajakta Gunjal on 30/03/2015, Because of change in the definition of Warranties Approved. Warranties sold only to the dealers 
-- are considered.

--INSERT INTO SC.DailyTracker (
--	 TrackerType
--	,TrackerCount
--	,TrackerDate
--	)	
--SELECT 'Warranties Approved' AS TrackerType
--	,count(DISTINCT Id) AS TrackerCount
--	,@TrackerDate AS TrackerDate
--FROM Absure_CarDetails WITH (NOLOCK)
--WHERE FinalWarrantyTypeId is not null
--	AND CONVERT(VARCHAR(11), FinalWarrantyDate, 113) = @TrackerDate
-- Modified by Fulsmita, the final numbers are from RVN_DealerPackageFeatures table



		
 INSERT INTO SC.DailyTracker (
	 TrackerType
	,TrackerCount
	,TrackerDate
	)
SELECT TrackerType,SUM(Warranty_sold),@TrackerDate as TrackerDate
FROM(
	SELECT
	'Warranties Approved' AS TrackerType,
	convert(int,sum(B.Amount) / 4000) as Warranty_sold
	FROM RVN_DealerPackageFeatures A WITH (NOLOCK)
	Inner JOIN DCRM_PaymentDetails B WITH (NOLOCK) ON A.TransactionId = B.TransactionId
	inner join Dealers C with (NOLock) on C.ID=A.DealerId
	WHERE A.PackageId in('72','73','74')
	and  CONVERT(date, B.AddedOn) = @TrackerDate
		   AND B.IsApproved = 1
		   AND A.DealerId NOT IN (
				   '4271'
				   ,'3838'
				   )
				   and C.Organization not like '%test%'
				   group by CONVERT(VARCHAR(11), B.AddedOn, 113)
	UNION
	SELECT
	'Warranties Approved' AS TrackerType,
	0 as Warranty_sold
) a
GROUP BY TrackerType

 --********************************************************************************************************
 -- Created by Fulsmita on 21/04/2015 , warranties activated to customers

 INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)
SELECT 'Warranty Activated' AS TrackerType
	,count(DISTINCT A.AbSure_CarDetailsId) AS TrackerCount
	,@TrackerDate AS TrackerDate
FROM AbSure_ActivatedWarranty A WITH (NOLOCK)
INNER JOIN Absure_carDetails B WITH (NOLOCK) ON A.AbSure_CarDetailsId = B.Id
INNER JOIN AbSure_Trans_Debits D WITH (NOLOCK) ON D.CarId = B.Id
INNER JOIN Dealers C WITH (NOLOCK) ON B.DealerId = C.ID
WHERE B.DealerId NOT IN (
		'4271'
		,'3838'
		)
	AND C.Organization NOT LIKE '%test%'
	AND CONVERT(VARCHAR(11), A.WarrantyStartDate, 113) = @TrackerDate

	 --********************************************************************************************************

-- *********Query Commented by Chirag Perla, as we are using LiveListingsDailyLog to get Live Listing Numbers --- 23_07_2015***********

 --  INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)
 --   SELECT 'Livelistings-Indiv' AS TrackerType,COUNT(ProfileId) LL_Cnt,@TrackerDate AS TrackerDate
 --   FROM livelistings WITH(NOLOCK)
	--WHERE SellerType=2

	------------------------------------------------------------------------------------------------

	--INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)
 --   SELECT 'Livelistings-Dealer' AS TrackerType,COUNT(ProfileId) LL_Cnt,@TrackerDate AS TrackerDate
 --   FROM livelistings WITH(NOLOCK)
	--WHERE SellerType=1
    
	--********************************************************************************************************

    INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)
    SELECT 'UC Buyers' AS TrackerType,COUNT(DISTINCT CustomerID) UsedCarBuyersCount ,@TrackerDate AS TrackerDate
	FROM(SELECT CustomerID
	FROM UsedCarPurchaseInquiries with(nolock)
	WHERE CONVERT(varchar(11),RequestDateTime,113) =@TrackerDate
	UNION ALL
	SELECT CustomerID
	FROM ClassifiedRequests with(nolock)
	WHERE CONVERT(varchar(11),RequestDateTime,113) =@TrackerDate)AS Tab	

--********************************************************************************************************
	--Leads Processed for Skoda and Mahindra
INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)
select trackertype,SUM(cnt), @TrackerDate as TrackerDate
from (
SELECT
CASE
WHEN CMK.ID =15 and CM.GroupType=1 THEN 'Skoda Leads Processed'
WHEN CMK.ID =9 and CM.GroupType=1 THEN 'Mahindra Leads Processed'
WHEN CMK.ID =54 and CM.GroupType=2 THEN 'Mahindra Leads Processed'
END
AS TrackerType,
COUNT(DISTINCT CL.id) AS Cnt, @TrackerDate AS TrackerDate
FROM CRM_Leads AS CL WITH (NOLOCK)
INNER JOIN CRM_CarBasicData AS CBD WITH (NOLOCK) ON CL.Id = CBD.LeadId
INNER JOIN CarVersions VW with(nolock) ON VW.Id = CBD.VersionId
Inner join CarModels CML with (NoLock) on CML.ID=VW.CarModelId
inner join carMakes CMK with (NoLock) on CMK.ID=CML.CarMakeId and CMK.ID in (9,15,54)
INNER JOIN CRM_ADM_GroupModelMapping CM with(nolock) ON CM.ModelId = VW.CarModelId
WHERE CONVERT(varchar(11),CL.CreatedOnDatePart,113) =@TrackerDate
group by CMK.ID,CM.GroupType
)a
group by trackertype, TrackerDate

--********************************************************************************************************

-- Modified by Fulsmita Bhattacharjee on 01/12/2014
INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
SELECT 'Mahindra Leads Assigned' AS TrackerType
    ,COUNT(DISTINCT l.id)
    ,@TrackerDate AS TrackerDate
FROM crm_leads AS l WITH (NOLOCK)
LEFT JOIN CRM_CarBasicData AS d WITH (NOLOCK) ON l.id = d.leadID
LEFT JOIN vwmmv AS v WITH (NOLOCK) ON v.VersionId = d.versionid
LEFT JOIN CRM_CarDealerAssignment AS da WITH (NOLOCK) ON da.CBDId = d.ID
WHERE CONVERT(VARCHAR(11), da.CreatedOn, 113) = @TrackerDate
    AND v.MakeId IN (
        9
        ,54
        )
	--Modified By Fulsmita on 1/12/2014 , taking the count of numbers from differnt table 
	INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,Trackerdate
    )
SELECT 'Mahindra Bookings' AS TrackerType
    , count(*) AS TrackerCount
    ,@TrackerDate AS TrackerDate
    FROM CRM_CarBookingLog AS A WITH (NOLOCK)
    INNER JOIN CRM_CarBasicData AS B WITH (NOLOCK) ON A.CBDId = B.Id
    INNER JOIN VWMMV V WITH (NOLOCK) ON B.VersionId = V.VersionId
    WHERE V.makeId IN (
            9
            ,54
            )
        AND A.IsBookingCompleted = 1
        AND CONVERT(VARCHAR(11), A.BookingCompletedEventOn, 113) =@TrackerDate
--********************************************************************************************************	
--Modified By Fulsmita on 1/12/2014 , new tracker type added
     INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
    select
    'Mahindra Car Invoiced' AS TrackerType ,count(*) as TrackerCount ,@TrackerDate AS TrackerDate
    from CRM_CarInvoices C with (NoLock)
    inner join CRM_ADM_Invoices I with (NoLock) on C.InvoiceId=I.Id
    inner join CRM_CarBasicData CBD with (NoLock) on CBD.ID=C.CBDId
    inner join vwMMV V with (NoLock) on CBD.VersionId=V.VersionId
     where C.IsActive is not Null and C.IsActive=1
     and V.MakeId in (9,54)
     and CONVERT(VARCHAR(11), C.UpdatedOn, 113)= @TrackerDate

	 --Modified by Fulsmita Bhattacharjee on 30/01/2015 , Changed Logic of Dealer PQ leads, tracking count of distinct mobile numbers 
INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
select 'Dealer PQ Assigned for New Car Leads' as TrackerType
    ,SUM(DistinctMobile) as TrackerCount
	,@TrackerDate as TrackerDate
    from
    (
    SELECT 
    count(distinct d.mobile) as DistinctMobile
FROM TC_NewCarInquiries AS A WITH (NOLOCK)
INNER JOIN TC_InquiriesLead AS B WITH (NOLOCK) ON A.TC_InquiriesLeadId = B.TC_InquiriesLeadId
INNER JOIN TC_Lead AS C WITH (NOLOCK) ON C.TC_LeadId = B.TC_LeadId
INNER JOIN TC_CustomerDetails AS D WITH (NOLOCK) ON D.id = C.TC_CustomerId
Inner JOIN TC_InquirySource AS E WITH (NOLOCK) ON E.Id = A.TC_InquirysourceId
WHERE E.Id IN (
        '6'
        ,'34'
        ,'36'
        ,'37'
        ,'38'
        )
    AND CONVERT(varchar(11),A.CreatedOn,113)=@TrackerDate	    
GROUP BY B.BranchId
)a

-- *********************************************************************************************************************************************
-- Modified by Fulsmita Bhattacharjee 02/12/2014 New Car Leads is now sum of the below three trackers Dealer PQ Assigned for New Car Leads, 
-- TLL Accepted Top for New Car Leads  and CRM Leads for New Car Leads
-- Modified by Fulsmita Bhattacharjee 02/12/2014 
-- Modified by Chirag Perla on 26-11-2015: Commented the Tracker, as it is no longer used(Also Dealer_NewCar table is not avaialbe anymore).

--INSERT INTO SC.DailyTracker (
--    TrackerType
--    ,TrackerCount
--    ,TrackerDate
--    )
--select 'TLL Accepted Top for New Car Leads' as TrackerType
--,count(A.Id) as TrackerCount ,
-- @TrackerDate as TrackerDate
-- from NCD_inquiries as A with (NoLock) inner join Dealer_NewCar B with (NoLock) on A.DealerId=B.Id
--where A.InquirySource=2 and  A.IsAccepted=1
--and CONVERT(varchar(11),A.EntryDate,113)=@TrackerDate
--group by CONVERT(varchar(11),A.EntryDate,113)




-- Modified by Fulsmita Bhattacharjee 02/12/2014 

INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
	select 'CRM Leads for New Car Leads' as TrackerType
	,sum(CountLeads) as TrackerCount
	,  @TrackerDate AS TrackerDate 
	from(
SELECT 
    COUNT(DISTINCT l.id) as CountLeads
  , @TrackerDate AS TrackerDate 
FROM crm_leads AS l WITH (NOLOCK)
LEFT JOIN CRM_CarBasicData AS d WITH (NOLOCK) ON l.id = d.leadID
LEFT JOIN vwmmv AS v WITH (NOLOCK) ON v.VersionId = d.versionid
LEFT JOIN CRM_CarDealerAssignment AS da WITH (NOLOCK) ON da.CBDId = d.ID
WHERE CONVERT(varchar(11),da.CreatedOn,113)=@TrackerDate
group by v.Make, CONVERT(varchar(11),da.CreatedOn,113)
)a

 --************************************************************************************************************************************
	-- Modified by Fulsmita Bhattacharjee, Impression Sold  on 12/12/2014

INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
SELECT 'Impression Sold' AS TrackerType
    ,count(B.PQId) as TrackerCount
	,@TrackerDate as TrackerDate
	 from NewCarPurchaseInquiries AS A with (NOLOCK)
inner join PQ_DealerSponsoredAd_ViewLogs AS B with (NOLOCK)
on A.Id=B.PQId
where CONVERT(VARCHAR(11), A.RequestDateTime, 113) = @TrackerDate


--***********************************************************************************************************

	-- Modified By Fulsmita for New ScoreCard Trackers
	-- Modified by Fulsmita Bhattacharjee 07/11/2014 added
	-- Commented by Fulsmita Bhattacharjee on 02/12/2014 due to change of Logic
	-- Modified by Fulsmita Bhattacharjee on 17/12/2014 
	
	--New Car Leads
INSERT INTO SC.DailyTracker (
	TrackerType
	,TrackerCount
	,TrackerDate
	)
SELECT 'New Car Leads' AS TrackerType
	,sum(SumPq)
	,@TrackerDate AS TrackerDate
FROM (
	SELECT COUNT(DISTINCT p.Mobile) AS SumPq
		,Convert(DATE, ncpi.RequestDateTime) AS DATE
	FROM dbo.PQDealerAdLeads AS p WITH (NOLOCK)
	JOIN dbo.NewCarPurchaseInquiries AS ncpi WITH (NOLOCK) ON p.PQId = ncpi.Id
	JOIN dbo.vwMMV AS v WITH (NOLOCK) ON ncpi.CarVersionId = v.VersionId
	WHERE Convert(DATE, ncpi.RequestDateTime) = @TrackerDate
	GROUP BY v.MakeId, Convert(DATE, ncpi.RequestDateTime)
	) a

--********************************************************************************************************	
--Dealer Listings
 INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)	
	SELECT 'Dealer Listing' AS TrackerType, COUNT(distinct ID) AS Cnt,@TrackerDate AS TrackerDate
	FROM SellInquiries WITH (NOLOCK)
	WHERE CONVERT(date,EntryDate) =@TrackerDate

--********************************************************************************************************

  --Total Unique Responses to Dealer listings
	 INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)	
	SELECT 'Total Unique Responses to Dealer listings' AS TrackerType,
	 COUNT(distinct CustomerId) AS Cnt,@TrackerDate AS TrackerDate
	FROM UsedCarPurchaseInquiries WITH (NOLOCK)
	WHERE CONVERT(date,RequestDateTime) =@TrackerDate

	--********************************************************************************************************

	--Individual Listing
	 INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)	
	SELECT 'Individual Listing' AS TrackerType, COUNT(distinct ID) AS Cnt,@TrackerDate AS TrackerDate
	FROM CustomerSellInquiries WITH (NOLOCK)
	WHERE CONVERT(date,EntryDate) =@TrackerDate

	--********************************************************************************************************

	--unique customers to Individual Responses
	 INSERT INTO SC.DailyTracker(TrackerType,TrackerCount,TrackerDate)	
	SELECT 'Unique Responses to Individual Listings' AS TrackerType, COUNT(CustomerId) AS Cnt,@TrackerDate AS TrackerDate
	FROM ClassifiedRequests WITH (NOLOCK)
	WHERE CONVERT(date,RequestDateTime) =@TrackerDate

	 --************************************************************************************************************************************

 -- Modified by Fulsmita Bhattacharjee, Coupon generated  on 12/12/2014
 -- Modified by Fulsmita Bhattacharjee, Coupon generated  on 29/12/2014, due to change in logic of the tables

  INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
SELECT 'Coupon' AS TrackerType
    ,count(distinct A.couponcode) as TrackerCount
	,@TrackerDate as TrackerDate
	 from OfferCouponCodes AS A with (NOLOCK)
inner join pqdealeradleads AS B with (NOLock) on A.referenceid=B.Id -- RefrenceId is now mapped with Id which is the primary key of pqdealeradleads
where CONVERT(VARCHAR(11), A.GeneratedOn, 113) = @TrackerDate
and B.email not like '%carwale%'
and B.name not like '%test%'
and B.email not like '%test%'
and B.Email not like '%@unknown.com' 


--************************************************************************************************************************************
-- Modified by Fulsmita Bhattacharjee, Bookings  on 12/12/2014

INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
SELECT 'Bookings' AS TrackerType
    ,count(TC_NewCarBookingId) as TrackerCount
	,@TrackerDate as TrackerDate
	from TC_newcarbooking WITH(NOLOCK)
	where isofferclaimed=1 --this are bookings through Carwale offer
	and CONVERT(VARCHAR(11), RequestedDate, 113) = @TrackerDate
	--and BookingStatus=1 -- booking is confirmed , no cancellation
--************************************************************************************************************************************
	-- Modified by Fulsmita Bhattacharjee, RSA Sold  on 12/12/2014

	 INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
SELECT 'RSA Sold' AS TrackerType
    ,sum(PackageQuantity) as TrackerCount
	,@TrackerDate as TrackerDate
	from RVN_DealerPackageFeatures WITH(NOLOCK)
	 where packageId in (61,62,63)--this are packages for RSA
	and CONVERT(VARCHAR(11), EntryDate, 113) = @TrackerDate

	--************************************************************************************************************************************

 -- Modified by Fulsmita Bhattacharjee, PQ Taken  on 12/12/2014

 INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
SELECT 'PQ Taken' AS TrackerType
    ,count(Id) as TrackerCount
	,@TrackerDate as TrackerDate
	 from NewCarPurchaseInquiries  with (NOLOCK)
where CONVERT(VARCHAR(11), RequestDateTime, 113) = @TrackerDate



--************************************************************************************************************************************
	-- Modified by Fulsmita Bhattacharjee, Dealer PQ Impression on 16/12/2014

	  INSERT INTO SC.DailyTracker (
    TrackerType
    ,TrackerCount
    ,TrackerDate
    )
 select 
 'Dealer PQ Impression' as TrackerType
 ,sum(SumPQ) as TrackerCount
 ,@TrackerDate as TrackerDate
from (SELECT s.DealerName,s.DealerId,v.Make,v.Model,c.City,CONVERT(VARCHAR(11), n.requestdatetime, 113) createdon,
case
when s.DealerId=9350 and v.Make in ('mahindra','Ssangyong') then 'OEM-Mahindra'
when s.DealerId=9350 and v.make in ('Fiat') then 'OEM-Fiat'
when s.DealerId=9350 and v.Make in ('tata','audi','toyota') then 'ES'
when s.DealerId=9350 and v.Make in ('bmw','Mercedes-Benz','Land Rover','Jaguar','Audi','hyundai') then 'Dealer PQ'
--when s.DealerId=9350 then 'ES'
when s.DealerId=9735 then 'ES'
when s.dealerid is not null then 'Dealer PQ'
end as Group1
,count(distinct n.id) as SumPQ
from NewCarPurchaseInquiries n WITH(NOLOCK)
left join PQ_DealerSponsoredAd_ViewLogs l WITH(NOLOCK) on l.PQId=n.Id
left join PQ_DealerSponsored s WITH(NOLOCK) on s.Id=l.CampaignId
left join vwMMV v WITH(NOLOCK) on v.VersionId=n.CarVersionId
left join NewPurchaseCities c WITH(NOLOCK) on c.InquiryId=n.Id
where
CONVERT(VARCHAR(11), n.requestdatetime, 113) = @TrackerDate
group by s.DealerName,s.DealerId,v.Make,v.Model,c.City,CONVERT(VARCHAR(11), n.requestdatetime, 113)
)a
where Group1='Dealer PQ'
group by createdon


----************************************************************************************************************************************
---- Modified by Fulsmita Bhattacharjee,  on 30/01/2015 , PQ Sponsored Dealers
-- Commented by Fulsmita , we are no longer showing PQ_Sponsored Dealers

insert into SC.DailyTracker
( TrackerType
    ,TrackerCount
    ,TrackerDate)
select 
'NCD PQ_Sponsored' as TrackerType
,count(distinct DealerId) as TrackerCount
,@TrackerDate as TrackerDate
from PQ_DealerSponsored WITH(NOLOCK)
where IsActive = '1'
and CONVERT(VARCHAR(11),EndDate,113) >= @TrackerDate


END