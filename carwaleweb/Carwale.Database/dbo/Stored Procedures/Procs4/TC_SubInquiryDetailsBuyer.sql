IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SubInquiryDetailsBuyer]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SubInquiryDetailsBuyer]
GO

	
-- =============================================
-- Created By: Manish Chourasiya
-- Created Date:13-03-2013
-- Description: DashBoard Reports for Used Car buyer Inquiries below are the report ids for parametre	
-- -- 1.Total inquiries   
   -- 2. Followup inquiries  
    --3. Closed inquiries  
    --4.Converted inquiries  
    --5. Archived
	-- Modified By Vivek Gupta on 24-02-2014, Added join with inquirysource 
-- Tejashree Patil : 16 Jun 2014 Fetched TC_LeadStageId, TCIL.TC_UserId.
-- Tejashree Patil : 2 July 2014 Fetched IsTDRequested,TDRequestedDate.
--Vicky Gupta : 21 July 2015 Fetched Next follow-up date and time  in @ReportId=2.
--Afrose on 28th Sept 2015, select new column eagerness(TC_InquiryStatusId) from table TC_TC_InquiriesLead; last call comments, modified source for other sources specifically
--Modified by :Ashwini Dhamankar on Dec 15,2015 (added constraint of MostInterested=1)
--=============================================
CREATE  PROCEDURE [dbo].[TC_SubInquiryDetailsBuyer]
  (
	@BranchId int,
	@FromDate DATETIME,
	@ToDate  DATETIME,
	@ReportId TINYINT  
   )
AS
	BEGIN
		 IF @ReportId=1
      BEGIN 
			 SELECT DISTINCT 
					   TC.Id  
					   ,TC.CustomerName 
					   ,TC.Email 
					   ,TC.Mobile 
					   ,TC.Location 
					   ,TCBI.CreatedOn AS EntryDate
					   ,TU.UserName
					   ,TCIL.TC_InquiryStatusId AS Eagerness --Added by Afrose
					   ,TCAC.LastCallComment AS Comment--Added by Afrose
					   ,ISNULL(vwMMV.Car,'Price:' +CONVERT(VARCHAR,TCBI.PriceMin)+'-'+CONVERT(VARCHAR,TCBI.PriceMax)) AS Car
					   ,YEAR(TCS.MakeYear) AS MakeYear
					   ,Price
					   ,Kms
					   ,Colour
					   ,CASE TCIS.Source WHEN 'Other Sources' THEN 'Other Sources-' + ' ' +(SELECT SourceName FROM TC_OtherInquirySources WITH(NOLOCK) WHERE InquiryId = TCBI.TC_BuyerInquiriesId)                                                               ELSE TCIS.Source END AS  Source  --Modified by Afrose	  
					   ,TL.TC_LeadId
					   ,TCIL.TC_UserId
					   ,TL.TC_LeadStageId
					   ,TCBI.IsTDRequested AS IsTDRequested
					   ,TCBI.TDRequestedDate AS TDRequestedDate,
					    '' AS  NextFollowUpDT 
				FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
				 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
				 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
						  AND TCIL.TC_LeadInquiryTypeId=1
						  AND  TL.BranchId	= @BranchId 
						  AND  TCIL.BranchId=  @BranchId 						  
				 JOIN TC_BuyerInquiries AS TCBI WITH(NOLOCK)   ON TCBI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
																  AND TCBI.CreatedOn BETWEEN @FromDate AND @ToDate
																  AND ISNULL(TCBI.MostInterested,0)=1  -- MOST INTERESTED
				 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
				 LEFT OUTER JOIN TC_Stock  AS TCS WITH(NOLOCK) ON TCS.Id=TCBI.StockId
				 LEFT OUTER JOIN  vwMMV  WITH(NOLOCK)   ON  vwMMV.VersionId=TCS.VersionId
				 LEFT OUTER JOIN  TC_InquirySource TCIS WITH(NOLOCK)   ON  TCBI.TC_InquirySourceId=TCIS.Id-- Modified By Vivek Gupta on 24-02-2014
				 LEFT OUTER JOIN TC_ActiveCalls AS TCAC WITH(NOLOCK) ON TCAC.TC_LeadId=TL.TC_LeadId --Added by Afrose
				
				ORDER BY TCBI.CreatedOn DESC
		  END 
		   ELSE IF  @ReportId=2
		     BEGIN
		     SELECT DISTINCT 
					   TC.Id , 
					   TC.CustomerName, 
					   TC.Email, 
					   TC.Mobile, 
					   TC.Location, 
					   TCBI.CreatedOn AS EntryDate, 
					   TU.UserName,
					   TCIL.TC_InquiryStatusId AS Eagerness, --Added by Afrose
					   TCAC.LastCallComment AS Comment, --Added by Afrose
					   ISNULL(vwMMV.Car,'Price:' +CONVERT(VARCHAR,TCBI.PriceMin)+'-'+CONVERT(VARCHAR,TCBI.PriceMax)) AS Car,
					   YEAR(TCS.MakeYear) AS MakeYear,
					   Price,
					   Kms,
					   Colour,
                       CASE TCIS.Source WHEN 'Other Sources' THEN 'Other Sources-' + ' ' +(SELECT SourceName FROM TC_OtherInquirySources WITH(NOLOCK)  WHERE InquiryId = TCBI.TC_BuyerInquiriesId)                                                               ELSE TCIS.Source END AS  Source,  --Modified by Afrose
					   TL.TC_LeadId,
					   TCIL.TC_UserId,
					   TL.TC_LeadStageId,
					   TCBI.IsTDRequested AS IsTDRequested,
					   TCBI.TDRequestedDate AS TDRequestedDate,
					   TCAC.ScheduledOn AS NextFollowUpDT
				FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
				 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
				 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
						  AND TCIL.TC_LeadInquiryTypeId=1
						  AND  TL.BranchId	= @BranchId 
						  AND  TCIL.BranchId=  @BranchId
						  AND  (TL.TC_LeadStageId=1 OR TL.TC_LeadStageId=2)
				 JOIN TC_BuyerInquiries AS TCBI WITH(NOLOCK)   ON TCBI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
																  AND TCBI.CreatedOn BETWEEN @FromDate AND @ToDate
																  AND TCBI.TC_LeadDispositionId IS NULL
																  AND ISNULL(TCBI.MostInterested,0) = 1
				 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
				 LEFT OUTER JOIN TC_Stock  AS TCS WITH(NOLOCK) ON TCS.Id=TCBI.StockId
				 LEFT OUTER JOIN  vwMMV  WITH(NOLOCK)  ON  vwMMV.VersionId=TCS.VersionId
				 LEFT OUTER JOIN  TC_InquirySource TCIS  WITH(NOLOCK)  ON  TCBI.TC_InquirySourceId=TCIS.Id-- Modified By Vivek Gupta on 24-02-2014
				 JOIN TC_ActiveCalls AS TCAC WITH(NOLOCK)	ON TL.TC_LeadId = TCAC.TC_LeadId --Modified by Vicky Gupta on 21-07-2015 
				ORDER BY TCBI.CreatedOn DESC
		      END 
		   ELSE IF @ReportId=3
		   BEGIN
		   --WITH CTE AS (
		   SELECT  
					   TC.Id , 
					   TC.CustomerName, 
					   TC.Email, 
					   TC.Mobile, 
					   TC.Location, 
					   TCBI.CreatedOn AS EntryDate, 
					   TU.UserName,
					   TCIL.TC_InquiryStatusId AS Eagerness, --Added by Afrose
					   TCAC.LastCallComment AS Comment, --Added by Afrose
					   ISNULL(vwMMV.Car,'Price:' +CONVERT(VARCHAR,TCBI.PriceMin)+'-'+CONVERT(VARCHAR,TCBI.PriceMax)) AS Car,
					   YEAR(TCS.MakeYear) AS MakeYear,
					   Price,
					   Kms,
					   Colour,
					   TCLD.Name AS [Reason for Close],
					   CASE TCIS.Source WHEN 'Other Sources' THEN 'Other Sources-' + ' ' +(SELECT SourceName FROM TC_OtherInquirySources WITH(NOLOCK)  WHERE InquiryId = TCBI.TC_BuyerInquiriesId)                                                               ELSE TCIS.Source END AS  Source,  --Modified by Afrose	
					   TL.TC_LeadId,
					   TCIL.TC_UserId,
					   TL.TC_LeadStageId,
					   TCBI.IsTDRequested AS IsTDRequested,
					   TCBI.TDRequestedDate AS TDRequestedDate,
					    '' AS  NextFollowUpDT 
				FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
				 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
				 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
						  AND TCIL.TC_LeadInquiryTypeId=1
						  AND  TL.BranchId	= @BranchId 
						  AND  TCIL.BranchId=  @BranchId
						  --AND  TL.TC_LeadStageId=3
				 JOIN TC_BuyerInquiries AS TCBI WITH(NOLOCK)   ON TCBI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId AND ISNULL(TCBI.MostInterested,0)=1
				  AND TCBI.CreatedOn BETWEEN @FromDate AND @ToDate
           		  AND  ((TL.TC_LeadDispositionId=3 OR TL.TC_LeadDispositionId=1 OR TCBI.TC_LeadDispositionId IS NOT  NULL)
                 AND ((TCBI.TC_LeadDispositionId<>4 AND TCBI.TC_LeadDispositionId<>34) OR TCBI.TC_LeadDispositionId IS NULL)) 
           		 JOIN TC_LeadDisposition AS TCLD WITH(NOLOCK)  ON ISNULL(TCBI.TC_LeadDispositionId,TL.TC_LeadDispositionId)=TCLD.TC_LeadDispositionId
                 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
				 LEFT OUTER JOIN TC_Stock  AS TCS WITH(NOLOCK) ON TCS.Id=TCBI.StockId
				 LEFT OUTER JOIN  vwMMV  WITH(NOLOCK)  ON  vwMMV.VersionId=TCS.VersionId
				 LEFT OUTER JOIN  TC_InquirySource TCIS  WITH(NOLOCK)  ON  TCBI.TC_InquirySourceId=TCIS.Id-- Modified By Vivek Gupta on 24-02-2014
				 LEFT OUTER JOIN TC_ActiveCalls AS TCAC WITH(NOLOCK) ON TCAC.TC_LeadId=TL.TC_LeadId --Added by Afrose
				/*UNION ALL
				SELECT  
					   TC.Id , 
					   TC.CustomerName, 
					   TC.Email, 
					   TC.Mobile, 
					   TC.Location, 
					   TCBI.CreatedOn AS EntryDate, 
					   TU.UserName,
					   ISNULL(vwMMV.Car,'Price:' +CONVERT(VARCHAR,TCBI.PriceMin)+'-'+CONVERT(VARCHAR,TCBI.PriceMax)) AS Car,
					   YEAR(TCS.MakeYear) AS MakeYear,
					   Price,
					   Kms,
					   Colour
				FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
				 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
				 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
						  AND TCIL.TC_LeadInquiryTypeId=1
						  AND  TL.BranchId	= @BranchId 
						  AND  TCIL.BranchId=  @BranchId
						  AND TL.TC_LeadStageId<>3
				 JOIN TC_BuyerInquiries AS TCBI WITH(NOLOCK)   ON TCBI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
																  AND TCBI.CreatedOn BETWEEN @FromDate AND @ToDate
																  AND TCBI.TC_LeadDispositionId IS NOT NULL 
																  AND  TCBI.TC_LeadDispositionId<>42
				 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
				 LEFT OUTER JOIN TC_Stock  AS TCS WITH(NOLOCK) ON TCS.Id=TCBI.StockId
				 LEFT OUTER JOIN  vwMMV   ON  vwMMV.VersionId=TCS.VersionId) 
				 SELECT * FROM CTE ORDER BY EntryDate DESC;*/
				
		   END
		   ELSE IF @ReportId=4
		    BEGIN 
		    SELECT DISTINCT 
					   TC.Id , 
					   TC.CustomerName, 
					   TC.Email, 
					   TC.Mobile, 
					   TC.Location, 
					   TCBI.CreatedOn AS EntryDate, 
					   TU.UserName,
					   TCIL.TC_InquiryStatusId AS Eagerness, --Added by Afrose
					   TCAC.LastCallComment AS Comment, --Added by Afrose
					   ISNULL(vwMMV.Car,'Price:' +CONVERT(VARCHAR,TCBI.PriceMin)+'-'+CONVERT(VARCHAR,TCBI.PriceMax)) AS Car,
					   YEAR(TCS.MakeYear) AS MakeYear,
					   Price,
					   Kms,
					   Colour,
					   CASE TCIS.Source WHEN 'Other Sources' THEN 'Other Sources-' + ' ' +(SELECT SourceName FROM TC_OtherInquirySources WITH(NOLOCK) WHERE InquiryId = TCBI.TC_BuyerInquiriesId)                                                               ELSE TCIS.Source END AS  Source,  --Modified by Afrose	
					   TL.TC_LeadId,
					   TCIL.TC_UserId,
					   TL.TC_LeadStageId,
					   TCBI.IsTDRequested AS IsTDRequested,
					   TCBI.TDRequestedDate AS TDRequestedDate,
					    '' AS  NextFollowUpDT 
				FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
				 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
				 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
						  AND TCIL.TC_LeadInquiryTypeId=1
						  AND  TL.BranchId	= @BranchId 
						  AND  TCIL.BranchId=  @BranchId
				 JOIN TC_BuyerInquiries AS TCBI WITH(NOLOCK)   ON TCBI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
																  AND TCBI.CreatedOn BETWEEN @FromDate AND @ToDate
																  AND TCBI.TC_LeadDispositionId =4
																  AND ISNULL(TCBI.MostInterested,0)=1 
				 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
				 LEFT OUTER JOIN TC_Stock  AS TCS WITH(NOLOCK) ON TCS.Id=TCBI.StockId
				 LEFT OUTER JOIN  vwMMV WITH(NOLOCK)   ON  vwMMV.VersionId=TCS.VersionId
				 LEFT OUTER JOIN  TC_InquirySource TCIS WITH(NOLOCK)  ON  TCBI.TC_InquirySourceId=TCIS.Id-- Modified By Vivek Gupta on 24-02-2014
				 LEFT OUTER JOIN TC_ActiveCalls AS TCAC WITH(NOLOCK) ON TCAC.TC_LeadId=TL.TC_LeadId --Added by Afrose
				ORDER BY TCBI.CreatedOn DESC
		    END
		    ELSE IF @ReportId=5
		    BEGIN 
		    SELECT DISTINCT 
					   TC.Id , 
					   TC.CustomerName, 
					   TC.Email, 
					   TC.Mobile, 
					   TC.Location, 
					   TCBI.CreatedOn AS EntryDate, 
					   TU.UserName,
					   TCIL.TC_InquiryStatusId AS Eagerness, --Added by Afrose
					   TCAC.LastCallComment AS Comment, --Added by Afrose
					   ISNULL(vwMMV.Car,'Price:' +CONVERT(VARCHAR,TCBI.PriceMin)+'-'+CONVERT(VARCHAR,TCBI.PriceMax)) AS Car,
					   YEAR(TCS.MakeYear) AS MakeYear,
					   Price,
					   Kms,
					   Colour,
					   CASE TCIS.Source WHEN 'Other Sources' THEN 'Other Sources-' + ' ' +(SELECT SourceName FROM TC_OtherInquirySources WITH(NOLOCK) WHERE InquiryId = TCBI.TC_BuyerInquiriesId)                                                               ELSE TCIS.Source END AS  Source,  --Modified by Afrose	
					   TL.TC_LeadId,
					   TCIL.TC_UserId,
					   TL.TC_LeadStageId,
					   TCBI.IsTDRequested AS IsTDRequested,
					   TCBI.TDRequestedDate AS TDRequestedDate,
					    '' AS  NextFollowUpDT 
				FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
				 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
				 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
						  AND TCIL.TC_LeadInquiryTypeId=1
						  AND  TL.BranchId	= @BranchId 
						  AND  TCIL.BranchId=  @BranchId
						  AND TL.TC_LeadDispositionId=41
				 JOIN TC_BuyerInquiries AS TCBI WITH(NOLOCK)   ON TCBI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
																  AND TCBI.CreatedOn BETWEEN @FromDate AND @ToDate
																  AND  ISNULL(TCBI.MostInterested,0)=1
                 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
				 LEFT OUTER JOIN TC_Stock  AS TCS WITH(NOLOCK) ON TCS.Id=TCBI.StockId
				 LEFT OUTER JOIN  vwMMV WITH(NOLOCK)  ON  vwMMV.VersionId=TCS.VersionId
				 LEFT OUTER JOIN  TC_InquirySource TCIS WITH(NOLOCK)  ON  TCBI.TC_InquirySourceId=TCIS.Id-- Modified By Vivek Gupta on 24-02-2014
				 LEFT OUTER JOIN TC_ActiveCalls AS TCAC WITH(NOLOCK) ON TCAC.TC_LeadId=TL.TC_LeadId --Added by Afrose
				ORDER BY TCBI.CreatedOn DESC
		    END 
/*        ELSE IF @ReportId=6
		    BEGIN 
		    SELECT DISTINCT 
						   TC.Id , 
						   TC.CustomerName, 
						   TC.Email, 
						   TC.Mobile, 
						   TC.Location, 
						   TL.LeadCreationDate AS EntryDate, 
						   TU.UserName,
			  			   TCIL.CarDetails  AS Car
						FROM   TC_CustomerDetails AS TC   WITH(NOLOCK)  
						 JOIN  TC_Lead          AS TL   WITH(NOLOCK)ON TL.TC_CustomerId= TC.Id 
						 JOIN  TC_InquiriesLead AS TCIL             ON TL.TC_LeadId=TCIL.TC_LeadId
											  AND  TL.BranchId	= @BranchId 
											  AND  TL.TC_LeadDispositionId=3
											  AND  TCIL .TC_LeadInquiryTypeId=1
											  AND  TL.LeadCreationDate BETWEEN @FromDate AND @ToDate
						 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
						ORDER BY EntryDate DESC
		    END 
		ELSE IF @ReportId=7
		    BEGIN 
		    SELECT DISTINCT 
						   TC.Id , 
						   TC.CustomerName, 
						   TC.Email, 
						   TC.Mobile, 
						   TC.Location, 
						   TL.LeadCreationDate AS EntryDate, 
						   TU.UserName,
			  			   TCIL.CarDetails  AS Car
						FROM   TC_CustomerDetails AS TC   WITH(NOLOCK)  
						 JOIN  TC_Lead          AS TL   WITH(NOLOCK)ON TL.TC_CustomerId= TC.Id 
						 JOIN  TC_InquiriesLead AS TCIL             ON TL.TC_LeadId=TCIL.TC_LeadId
											  AND  TL.BranchId	= @BranchId 
											  AND  TL.TC_LeadDispositionId=1
											  AND  TCIL .TC_LeadInquiryTypeId=1
											  AND  TL.LeadCreationDate BETWEEN @FromDate AND @ToDate
						 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
						ORDER BY EntryDate DESC
		    END */
				
		      
	END

----------------------------


/****** Object:  StoredProcedure [dbo].[TC_SubInquiryDetailsNewCar]    Script Date: 12/22/2015 7:00:28 PM ******/
SET ANSI_NULLS ON
