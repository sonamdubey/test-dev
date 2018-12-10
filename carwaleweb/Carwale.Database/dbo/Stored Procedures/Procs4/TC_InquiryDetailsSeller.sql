IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InquiryDetailsSeller]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InquiryDetailsSeller]
GO

	-- =============================================
-- Created By: Vivek Gupta
-- Created Date:25-05-2013
-- Description: Reports for Used Car sell Inquiries below are the report ids for parametre	
-- -- 1. Total
   -- 2. Lost
    --3. Converted
    --4. Active 
    --5. HOT
    --6. WARM
    --7. NORMAL
    --8. NOTSET
    --9. Not Contacted
    --10.DISCARDED
    --11.FAKE
    --12.NOT INTERESTED

	-- Modified By Vivek Gupta on 16th , dec, 2013 added select queries for following values
	--13.Current Inquiries in a Given date range @FromDate AND @ToDate
	--14.Followup sheduled to current inquiries in a Given date range @FromDate AND @ToDate
	--15.Called inquiries  in a Given date range @FromDate AND @ToDate
	--16.Carry Forward inquiries before @FromDate
	--17.Shceduled followups to all carry forward inquiries till @ToDate
	--18.Called , Action taken on shcheduled inquiries for all carryforward followups.

	-- Modified By: Vivek Gupta on 20-12-2013, added condition A.ActionComments <> 'Customer verified'
	-- Tejashree Patil on 16 Jun 2014, Fetched Tc_LeadStageId and TCIL.TC_UserId, TL.TC_LeadId.
	-- Modified By Vivek gupta on 7-7-2014, Added join with inquirysource table to get source
	--Modified By : Afrose on 14-12-2015, selected last call comment in case of lost or rejected
	-- modeified By: Deepak on 16th May 2016, Added Distinct and CallId for reportid-14 and 15
	-- Modified by : Kritika Choudhary on 17th May 2016, added distinct, A.TC_CallsId for reportid-17 and 18
   --=============================================
CREATE  PROCEDURE [dbo].[TC_InquiryDetailsSeller]
  (
	@BranchId int,
	@FromDate DATETIME,
	@ToDate  DATETIME,
	@ReportId TINYINT,
	@UserId INT  
   )
AS
	BEGIN
	   IF @ReportId=1--Total
		 BEGIN 
		 SELECT DISTINCT 
				   TC.Id , 
				   TC.CustomerName, TS.Source,
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   year(TCSI.MakeYear) as MakeYear,
				   TCSI.Price,
				   TCSI.Kms,
				   TCSI.Colour,
				   TCSI.CreatedOn AS EntryDate, 
				   TU.UserName,
				   vwMMV.Car,
				   TL.TC_LeadId,
				   TCIL.TC_UserId,
				   TL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
				  

			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
			 JOIN  TC_InquirySource AS TS   WITH(NOLOCK) ON TC.TC_InquirySourceId = TS.Id	
			 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
			 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					  AND TCIL.TC_LeadInquiryTypeId=2
					  AND  TL.BranchId	= @BranchId 
					  AND  TCIL.BranchId=  @BranchId 
			 JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK)   ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
															  AND TCSI.CreatedOn BETWEEN @FromDate AND @ToDate
			 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
			 LEFT OUTER JOIN  vwMMV  WITH(NOLOCK)  ON vwMMV.VersionId=TCSI.CarVersionId
			 LEFT JOIN  TC_Calls   AS C   WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId

			ORDER BY TCSI.CreatedOn DESC
	  END 
	   ELSE IF @ReportId=2--Lost
		 BEGIN
		  SELECT 
				   TC.Id , 
				   TC.CustomerName, TS.Source,
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   year(TCSI.MakeYear) as MakeYear,
				   TCSI.Price,
				   TCSI.Kms,
				   TCSI.Colour,
				   TCSI.CreatedOn AS EntryDate, 
				   TU.UserName,
				   vwMMV.Car,
				   TCLD.Name AS [Reason for Close],
				   TL.TC_LeadId,
                   TCIL.TC_UserId,
				   TL.TC_LeadStageId, --Tejashree Patil on 16 Jun 2014 --
				   (SELECT TOP 1 ActionComments FROM TC_Calls WITH(NOLOCK) WHERE TC_LeadId = TCIL.TC_LeadId AND IsActionTaken = 1 ORDER BY TC_CallsId DESC) AS LastCallComment --Afrose

			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
			 JOIN  TC_InquirySource AS TS   WITH(NOLOCK) ON TC.TC_InquirySourceId = TS.Id	
			 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
			 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					  AND TCIL.TC_LeadInquiryTypeId=2
					  AND  TL.BranchId	= @BranchId 
					  AND  TCIL.BranchId=  @BranchId
					 -- AND  TL.TC_LeadStageId=3
			 JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK)   ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
			  AND TCSI.CreatedOn BETWEEN @FromDate AND @ToDate
			  AND  TCSI.TC_LeadDispositionId NOT IN (1,3,4) AND TCSI.TC_LeadDispositionId IS NOT NULL
   			 JOIN TC_LeadDisposition AS TCLD WITH(NOLOCK) ON ISNULL(TCSI.TC_LeadDispositionId,TL.TC_LeadDispositionId)=TCLD.TC_LeadDispositionId												  
			 LEFT OUTER JOIN  TC_Users  AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
			 LEFT OUTER JOIN  vwMMV  WITH(NOLOCK) ON  vwMMV.VersionId=TCSI.CarVersionId	
			 LEFT JOIN  TC_Calls   AS C   WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId
			 	
			 WHERE (TCIL.TC_UserId = @UserId OR @UserId IS NULL)		
		 END
	   ELSE IF @ReportId=3 --Converted
		 BEGIN 
		  SELECT DISTINCT 
				   TC.Id , 
				   TC.CustomerName, TS.Source,
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   year(TCSI.MakeYear) as MakeYear,
				   TCSI.Price,
				   TCSI.Kms,
				   TCSI.Colour,
				   TCSI.CreatedOn AS EntryDate, 
				   TU.UserName,
				   vwMMV.Car,
				   TL.TC_LeadId,
                   TCIL.TC_UserId,
				   TL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
				   
			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
			 JOIN  TC_InquirySource AS TS   WITH(NOLOCK) ON TC.TC_InquirySourceId = TS.Id	
			 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
			 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					  AND TCIL.TC_LeadInquiryTypeId=2
					  AND  TL.BranchId	= @BranchId 
					  AND  TCIL.BranchId=  @BranchId
			 JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK)   ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
															  AND TCSI.CreatedOn BETWEEN @FromDate AND @ToDate
															  AND TCSI.TC_LeadDispositionId =4
			 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
			 LEFT OUTER JOIN  vwMMV   WITH(NOLOCK) ON  vwMMV.VersionId=TCSI.CarVersionId
			 LEFT JOIN  TC_Calls   AS C   WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId
			 WHERE (TCIL.TC_UserId = @UserId OR @UserId IS NULL)	
			ORDER BY TCSI.CreatedOn DESC
		 END	  
	   ELSE IF @ReportId=4--Active
		 BEGIN
		 SELECT DISTINCT 
				   TC.Id , 
				   TC.CustomerName, TS.Source,
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   year(TCSI.MakeYear) as MakeYear,
				   TCSI.Price,
				   TCSI.Kms,
				   TCSI.Colour,
				   TCSI.CreatedOn AS EntryDate, 
				   TU.UserName,
				   vwMMV.Car,
				   TL.TC_LeadId,
                   TCIL.TC_UserId,
				   TL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
				 

			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
			 JOIN  TC_InquirySource AS TS   WITH(NOLOCK) ON TC.TC_InquirySourceId = TS.Id	
			 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
			 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					  AND TCIL.TC_LeadInquiryTypeId=2
					  AND TL.BranchId	= @BranchId 
					  AND TCIL.BranchId=  @BranchId
					  AND (TCIL.TC_LeadStageId=1 OR TCIL.TC_LeadStageId=2)
					  AND TCIL.TC_UserId IS NOT NULL 
			 JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK)   ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
															  AND TCSI.CreatedOn BETWEEN @FromDate AND @ToDate
															  AND TCSI.TC_LeadDispositionId IS NULL
			 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
			 LEFT OUTER JOIN  vwMMV WITH(NOLOCK)  ON  vwMMV.VersionId=TCSI.CarVersionId
			 LEFT JOIN  TC_Calls   AS C   WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId
			 WHERE (TCIL.TC_UserId = @UserId OR @UserId IS NULL)	
			ORDER BY TCSI.CreatedOn DESC
		  END 
	   ELSE IF  @ReportId=5--HOT
		 BEGIN
		 SELECT DISTINCT 
				   TC.Id , 
				   TC.CustomerName, TS.Source,
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   year(TCSI.MakeYear) as MakeYear,
				   TCSI.Price,
				   TCSI.Kms,
				   TCSI.Colour,
				   TCSI.CreatedOn AS EntryDate, 
				   TU.UserName,
				   TL.TC_LeadId,
				   vwMMV.Car,
                   TCIL.TC_UserId,
				   TL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
				  
			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
			 JOIN  TC_InquirySource AS TS   WITH(NOLOCK) ON TC.TC_InquirySourceId = TS.Id	
			 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
			 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					  AND TCIL.TC_LeadInquiryTypeId=2
					  AND  TL.BranchId	= @BranchId 
					  AND  TCIL.BranchId=  @BranchId
					  AND  TCIL.TC_InquiryStatusId = 1
					  AND TCIL.TC_UserId IS NOT NULL
			 JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK)   ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
															  AND TCSI.CreatedOn BETWEEN @FromDate AND @ToDate
															  AND TCSI.TC_LeadDispositionId IS NULL
			 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
			 LEFT OUTER JOIN  vwMMV  WITH(NOLOCK) ON  vwMMV.VersionId=TCSI.CarVersionId
			 LEFT JOIN  TC_Calls   AS C   WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId
			ORDER BY TCSI.CreatedOn DESC
		  END 
	   ELSE IF  @ReportId=6--WARM
		 BEGIN
		 SELECT DISTINCT 
				   TC.Id , 
				   TC.CustomerName, TS.Source,
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   year(TCSI.MakeYear) as MakeYear,
				   TCSI.Price,
				   TCSI.Kms,
				   TCSI.Colour,
				   TCSI.CreatedOn AS EntryDate, 
				   TU.UserName,
				   TL.TC_LeadId,
				   vwMMV.Car,
                   TCIL.TC_UserId,
				   TL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
				   
			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
			 JOIN  TC_InquirySource AS TS   WITH(NOLOCK) ON TC.TC_InquirySourceId = TS.Id	
			 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
			 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					  AND TCIL.TC_LeadInquiryTypeId=2
					  AND  TL.BranchId	= @BranchId 
					  AND  TCIL.BranchId=  @BranchId
					  AND  TCIL.TC_InquiryStatusId = 2
			 JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK)   ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
															  AND TCSI.CreatedOn BETWEEN @FromDate AND @ToDate
															  AND TCSI.TC_LeadDispositionId IS NULL
			 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
			 LEFT OUTER JOIN  vwMMV WITH(NOLOCK)  ON  vwMMV.VersionId=TCSI.CarVersionId
			 LEFT JOIN  TC_Calls   AS C   WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId
			ORDER BY TCSI.CreatedOn DESC
		  END 
	   ELSE IF  @ReportId=7--NORMAL
		 BEGIN
		 SELECT DISTINCT 
				   TC.Id , 
				   TC.CustomerName, TS.Source,
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   year(TCSI.MakeYear) as MakeYear,
				   TCSI.Price,
				   TCSI.Kms,
				   TCSI.Colour,
				   TCSI.CreatedOn AS EntryDate, 
				   TU.UserName,
				   TL.TC_LeadId,
				   vwMMV.Car,
                   TCIL.TC_UserId,
				   TL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
				 
			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
			 JOIN  TC_InquirySource AS TS   WITH(NOLOCK) ON TC.TC_InquirySourceId = TS.Id	
			 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
			 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					  AND TCIL.TC_LeadInquiryTypeId=2
					  AND  TL.BranchId	= @BranchId 
					  AND  TCIL.BranchId=  @BranchId
					  AND  TCIL.TC_InquiryStatusId = 3
			 JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK)   ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
															  AND TCSI.CreatedOn BETWEEN @FromDate AND @ToDate
															  AND TCSI.TC_LeadDispositionId IS NULL
			 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
			 LEFT OUTER JOIN  vwMMV WITH(NOLOCK) ON  vwMMV.VersionId=TCSI.CarVersionId
			 LEFT JOIN  TC_Calls   AS C   WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId
			ORDER BY TCSI.CreatedOn DESC
		  END 
	   ELSE IF  @ReportId=8--NOTSET
		 BEGIN
		 SELECT DISTINCT 
				   TC.Id , 
				   TC.CustomerName, TS.Source,
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   year(TCSI.MakeYear) as MakeYear,
				   TCSI.Price,
				   TCSI.Kms,
				   TCSI.Colour,
				   TCSI.CreatedOn AS EntryDate, 
				   TU.UserName,
				   TL.TC_LeadId,
				   vwMMV.Car,
                   TCIL.TC_UserId,
				   TL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
				   
			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
			 JOIN  TC_InquirySource AS TS   WITH(NOLOCK) ON TC.TC_InquirySourceId = TS.Id	
			 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
			 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					  AND TCIL.TC_LeadInquiryTypeId=2
					  AND  TL.BranchId	= @BranchId 
					  AND  TCIL.BranchId=  @BranchId
					  AND  TCIL.TC_InquiryStatusId IS NULL
					  AND TCIL.TC_UserId IS NOT NULL
			 JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK)   ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
															  AND TCSI.CreatedOn BETWEEN @FromDate AND @ToDate
															  AND TCSI.TC_LeadDispositionId IS NULL
			 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
			 LEFT OUTER JOIN  vwMMV WITH(NOLOCK)  ON  vwMMV.VersionId=TCSI.CarVersionId
			 LEFT JOIN  TC_Calls   AS C   WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId
			ORDER BY TCSI.CreatedOn DESC
		  END 
		  
	   ELSE IF @ReportId=9--Not Contacted
		 BEGIN
		 SELECT DISTINCT 
				   TC.Id , 
				   TC.CustomerName, TS.Source,
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   year(TCSI.MakeYear) as MakeYear,
				   TCSI.Price,
				   TCSI.Kms,
				   TCSI.Colour,
				   TCSI.CreatedOn AS EntryDate, 
				   TU.UserName,
				   TL.TC_LeadId,
				   vwMMV.Car,
                   TCIL.TC_UserId,
				   TL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
				  
			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
			 JOIN  TC_InquirySource AS TS   WITH(NOLOCK) ON TC.TC_InquirySourceId = TS.Id	
			 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
			 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					  AND TCIL.TC_LeadInquiryTypeId=2
					  AND  TL.BranchId	= @BranchId 
					  AND  TCIL.BranchId=  @BranchId
					  AND  (TCIL.TC_LeadStageId=1 AND TCIL.TC_UserId IS NOT NULL)
			 JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK)   ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
															  AND TCSI.CreatedOn BETWEEN @FromDate AND @ToDate
															  AND TCSI.TC_LeadDispositionId IS NULL
			 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
			 LEFT OUTER JOIN  vwMMV WITH(NOLOCK)  ON  vwMMV.VersionId=TCSI.CarVersionId
			 LEFT JOIN  TC_Calls   AS C   WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId
			 WHERE (TCIL.TC_UserId = @UserId OR @UserId IS NULL)	
			ORDER BY TCSI.CreatedOn DESC
		  END 
		  
	   ELSE IF @ReportId=10 -- DISCARDED
		 BEGIN 
		   SELECT DISTINCT 
				   TC.Id , 
				   TC.CustomerName, TS.Source,
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   YEAR(TCSI.MakeYear) as MakeYear,
				   TCSI.Price,
				   TCSI.Kms,
				   TCSI.Colour,
				   TCSI.CreatedOn AS EntryDate, 
				   TU.UserName,
				   TL.TC_LeadId,
				   vwMMV.Car,
                   TCIL.TC_UserId,
				   TL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
				  

			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
			 JOIN  TC_InquirySource AS TS   WITH(NOLOCK) ON TC.TC_InquirySourceId = TS.Id	
			 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
			 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					  AND TCIL.TC_LeadInquiryTypeId=2
					  AND  TL.BranchId	= @BranchId 
					  AND  TCIL.BranchId=  @BranchId
					  AND TL.TC_LeadDispositionId IN(1,3)
			 JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK)   ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
															  AND TCSI.CreatedOn BETWEEN @FromDate AND @ToDate
			 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
			 LEFT OUTER JOIN  vwMMV WITH(NOLOCK)  ON  vwMMV.VersionId=TCSI.CarVersionId
			 LEFT JOIN  TC_Calls   AS C   WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId
			ORDER BY TCSI.CreatedOn DESC
		 END 
	   ELSE IF @ReportId=11 -- FAKE
		 BEGIN 
		   SELECT DISTINCT 
				   TC.Id , 
				   TC.CustomerName, TS.Source,
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   YEAR(TCSI.MakeYear) as MakeYear,
				   TCSI.Price,
				   TCSI.Kms,
				   TCSI.Colour,
				   TCSI.CreatedOn AS EntryDate, 
				   TU.UserName,
				   TL.TC_LeadId,
				   vwMMV.Car,
                   TCIL.TC_UserId,
				   TL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
				  
			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
			 JOIN  TC_InquirySource AS TS   WITH(NOLOCK) ON TC.TC_InquirySourceId = TS.Id	
			 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
			 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					  AND TCIL.TC_LeadInquiryTypeId=2
					  AND  TL.BranchId	= @BranchId 
					  AND  TCIL.BranchId=  @BranchId
					  AND TL.TC_LeadDispositionId=1
			 JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK)   ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
															  AND TCSI.CreatedOn BETWEEN @FromDate AND @ToDate
			 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
			 LEFT OUTER JOIN  vwMMV WITH(NOLOCK)  ON  vwMMV.VersionId=TCSI.CarVersionId
			 LEFT JOIN  TC_Calls   AS C   WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId
			ORDER BY TCSI.CreatedOn DESC
		 END 
	   ELSE IF @ReportId=12 -- NOT INTERESTED
		 BEGIN 
		   SELECT DISTINCT 
				   TC.Id , 
				   TC.CustomerName, TS.Source,
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   YEAR(TCSI.MakeYear) as MakeYear,
				   TCSI.Price,
				   TCSI.Kms,
				   TCSI.Colour,
				   TCSI.CreatedOn AS EntryDate, 
				   TU.UserName,
				   TL.TC_LeadId,
				   vwMMV.Car,
                   TCIL.TC_UserId,
				   TL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
				   
			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
			 JOIN  TC_InquirySource AS TS   WITH(NOLOCK) ON TC.TC_InquirySourceId = TS.Id	
			 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
			 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					  AND TCIL.TC_LeadInquiryTypeId=2
					  AND  TL.BranchId	= @BranchId 
					  AND  TCIL.BranchId=  @BranchId
					  AND TL.TC_LeadDispositionId=3
			 JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK)   ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
															  AND TCSI.CreatedOn BETWEEN @FromDate AND @ToDate
			 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
			 LEFT OUTER JOIN  vwMMV  WITH(NOLOCK) ON  vwMMV.VersionId=TCSI.CarVersionId
			 LEFT JOIN  TC_Calls   AS C   WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId
			ORDER BY TCSI.CreatedOn DESC
		 END
	 ELSE IF @ReportId=13--Current inquiries
		 BEGIN
		 SELECT DISTINCT 
				   TC.Id , 
				   TC.CustomerName, TS.Source,
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   year(TCSI.MakeYear) as MakeYear,
				   TCSI.Price,
				   TCSI.Kms,
				   TCSI.Colour,
				   TCSI.CreatedOn AS EntryDate, 
				   TU.UserName,
				   TL.TC_LeadId,
				   vwMMV.Car,
                   TCIL.TC_UserId,
				   TL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
				 
			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
			 JOIN  TC_InquirySource AS TS   WITH(NOLOCK) ON TC.TC_InquirySourceId = TS.Id	
			 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
			 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					  AND TCIL.TC_LeadInquiryTypeId=2
					  AND TL.BranchId	= @BranchId 
					  AND TCIL.BranchId=  @BranchId
					  AND (TCIL.TC_LeadStageId=1 OR TCIL.TC_LeadStageId=2)
					  AND TCIL.TC_UserId IS NOT NULL 
			 JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK)   ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
															  AND TCSI.CreatedOn BETWEEN @FromDate AND @ToDate
															  AND TCSI.TC_LeadDispositionId IS NULL
			 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
			 LEFT OUTER JOIN  vwMMV WITH(NOLOCK)  ON  vwMMV.VersionId=TCSI.CarVersionId
			 LEFT JOIN  TC_Calls   AS C   WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId
			 WHERE (TCIL.TC_UserId = @UserId OR @UserId IS NULL)	
			ORDER BY TCSI.CreatedOn DESC
		  END         
	 ELSE IF @ReportId=14--Shcedulled followups for current inquiries. -- modeified By Deepak on 16th May 2016, Added Distinct and CallId
		 BEGIN
		 SELECT		DISTINCT	A.TC_CallsId,
				   TC.Id , 
				   TC.CustomerName, TS.Source,
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   year(TCSI.MakeYear) as MakeYear,
				   TCSI.Price,
				   TCSI.Kms,
				   TCSI.Colour,
				   TCSI.CreatedOn AS EntryDate, 
				   TU.UserName,
				   TL.TC_LeadId,
				   vwMMV.Car,
                   TCIL.TC_UserId,
				   TL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
				
			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
			 JOIN  TC_InquirySource AS TS   WITH(NOLOCK) ON TC.TC_InquirySourceId = TS.Id	
			 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
			 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					  AND TCIL.TC_LeadInquiryTypeId=2
					  AND TL.BranchId	= @BranchId 
					  AND TCIL.BranchId=  @BranchId
					  AND TCIL.TC_UserId IS NOT NULL 
					  AND TCIL.TC_LeadStageId <> 3					  
			 JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK)   ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
															  AND TCSI.CreatedOn BETWEEN @FromDate AND @ToDate															  
			 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
			  JOIN TC_Calls AS A WITH(NOLOCK)  ON A.TC_LeadId = TCIL.TC_LeadId AND TCIL.IsActive = 1
			 LEFT OUTER JOIN  vwMMV  WITH(NOLOCK) ON  vwMMV.VersionId=TCSI.CarVersionId
			  LEFT JOIN  TC_Calls   AS C   WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId
			 WHERE (TCIL.TC_UserId = @UserId OR @UserId IS NULL)
			 AND (A.ScheduledOn BETWEEN @FromDate AND @ToDate)
			 AND ISNULL(A.IsActionTaken,0) <> 1 AND (A.ActionComments <> 'Customer Verified')
			ORDER BY TCSI.CreatedOn DESC
		  END         
	ELSE IF @ReportId=15--called inquiries of followups of current inquiries.-- modeified By Deepak on 16th May 2016, Added Distinct and CallId
		 BEGIN
		 SELECT  DISTINCT A.TC_CallsId,
				   TC.Id , 
				   TC.CustomerName, TS.Source,
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   year(TCSI.MakeYear) as MakeYear,
				   TCSI.Price,
				   TCSI.Kms,
				   TCSI.Colour,
				   TCSI.CreatedOn AS EntryDate, 
				   TU.UserName,
				   TL.TC_LeadId,
				   vwMMV.Car,
                   TCIL.TC_UserId,
				   TL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
				  
			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
			 JOIN  TC_InquirySource AS TS   WITH(NOLOCK) ON TC.TC_InquirySourceId = TS.Id	
			 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
			 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					  AND TCIL.TC_LeadInquiryTypeId=2
					  AND TL.BranchId	= @BranchId 
					  AND TCIL.BranchId=  @BranchId
					  AND TCIL.TC_UserId IS NOT NULL 
					  AND TCIL.TC_LeadStageId <> 3
			 JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK)   ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
															  AND TCSI.CreatedOn BETWEEN @FromDate AND @ToDate
			 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
			  JOIN TC_Calls AS A WITH(NOLOCK)  ON A.TC_LeadId = TCIL.TC_LeadId AND TCIL.IsActive = 1
			 LEFT OUTER JOIN  vwMMV WITH(NOLOCK)  ON  vwMMV.VersionId=TCSI.CarVersionId
			 LEFT JOIN  TC_Calls   AS C   WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId
			 WHERE (TCIL.TC_UserId = @UserId OR @UserId IS NULL)
			 AND (A.ScheduledOn BETWEEN @FromDate AND @ToDate)	
			 AND ISNULL(A.IsActionTaken,0) = 1 AND (A.ActionComments <> 'Customer Verified')
			
			ORDER BY TCSI.CreatedOn DESC
		  END         
	ELSE IF @ReportId=16--Carryforward inquiries
		 BEGIN
		 SELECT DISTINCT 
				   TC.Id , 
				   TC.CustomerName, TS.Source,
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   year(TCSI.MakeYear) as MakeYear,
				   TCSI.Price,
				   TCSI.Kms,
				   TCSI.Colour,
				   TCSI.CreatedOn AS EntryDate, 
				   TU.UserName,
				   TL.TC_LeadId,
				   vwMMV.Car,
                   TCIL.TC_UserId,
				   TL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
				  
			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
			 JOIN  TC_InquirySource AS TS   WITH(NOLOCK) ON TC.TC_InquirySourceId = TS.Id	
			 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
			 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					  AND TCIL.TC_LeadInquiryTypeId=2
					  AND TL.BranchId	= @BranchId 
					  AND TCIL.BranchId=  @BranchId
					  AND (TCIL.TC_LeadStageId=1 OR TCIL.TC_LeadStageId=2)
					  AND TCIL.TC_UserId IS NOT NULL 
			 JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK)   ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
															  AND TCSI.CreatedOn < @FromDate
															  AND TCSI.TC_LeadDispositionId IS NULL
			 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
			 LEFT OUTER JOIN  vwMMV WITH(NOLOCK)  ON  vwMMV.VersionId=TCSI.CarVersionId
			 LEFT JOIN  TC_Calls   AS C   WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId
			 WHERE (TCIL.TC_UserId = @UserId OR @UserId IS NULL)	
			ORDER BY TCSI.CreatedOn DESC
		  END         
	ELSE IF @ReportId=17--shceduled followups for carryforward inquiries
		 BEGIN
		 SELECT   DISTINCT A.TC_CallsId, -- Modified by : Kritika Choudhary on 17th May 2016, added distinct, A.TC_CallsId
				   TC.Id , 
				   TC.CustomerName, TS.Source,
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   year(TCSI.MakeYear) as MakeYear,
				   TCSI.Price,
				   TCSI.Kms,
				   TCSI.Colour,
				   TCSI.CreatedOn AS EntryDate, 
				   TU.UserName,
				   TL.TC_LeadId,
				   vwMMV.Car,
                   TCIL.TC_UserId,
				   TL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
				  
			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
			 JOIN  TC_InquirySource AS TS   WITH(NOLOCK) ON TC.TC_InquirySourceId = TS.Id	
			 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
			 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					  AND TCIL.TC_LeadInquiryTypeId=2
					  AND TL.BranchId	= @BranchId 
					  AND TCIL.BranchId=  @BranchId
					  AND TCIL.TC_UserId IS NOT NULL 
					  AND TCIL.TC_LeadStageId <> 3
			 JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK)   ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
															  AND TCSI.CreatedOn < @FromDate								
			 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
			  JOIN TC_Calls AS A WITH(NOLOCK)  ON A.TC_LeadId = TCIL.TC_LeadId AND TCIL.IsActive = 1
			 LEFT OUTER JOIN  vwMMV  WITH(NOLOCK) ON  vwMMV.VersionId=TCSI.CarVersionId
			  LEFT JOIN  TC_Calls   AS C   WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId
			 WHERE (TCIL.TC_UserId = @UserId OR @UserId IS NULL)	
			 AND (A.ScheduledOn <= @ToDate)
			 AND ISNULL(A.IsActionTaken,0)<>1 AND (A.ActionComments <> 'Customer Verified')
			ORDER BY TCSI.CreatedOn DESC
		  END   
		ELSE IF @ReportId=18--called followups inquiries for carryforward inquiries.
		 BEGIN
		 SELECT   DISTINCT A.TC_CallsId, -- Modified by : Kritika Choudhary on 17th May 2016, added distinct, A.TC_CallsId
				   TC.Id , 
				   TC.CustomerName, TS.Source,
				   TC.Email, 
				   TC.Mobile, 
				   TC.Location, 
				   year(TCSI.MakeYear) as MakeYear,
				   TCSI.Price,
				   TCSI.Kms,
				   TCSI.Colour,
				   TCSI.CreatedOn AS EntryDate, 
				   TU.UserName,
				   TL.TC_LeadId,
				   vwMMV.Car,
                   TCIL.TC_UserId,
				   TL.TC_LeadStageId --Tejashree Patil on 16 Jun 2014
				   
			FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
			 JOIN  TC_InquirySource AS TS   WITH(NOLOCK) ON TC.TC_InquirySourceId = TS.Id	
			 JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
			 JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
					  AND TCIL.TC_LeadInquiryTypeId=2
					  AND TL.BranchId	= @BranchId 
					  AND TCIL.BranchId=  @BranchId
					  AND (TCIL.TC_LeadStageId=1 OR TCIL.TC_LeadStageId=2)
					  AND TCIL.TC_UserId IS NOT NULL 
					  AND TCIL.TC_LeadStageId <> 3
			 JOIN TC_SellerInquiries AS TCSI WITH(NOLOCK)   ON TCSI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
															  AND TCSI.CreatedOn < @FromDate
															  AND TCSI.TC_LeadDispositionId IS NULL
			 LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
			  JOIN TC_Calls AS A WITH(NOLOCK)  ON A.TC_LeadId = TCIL.TC_LeadId AND TCIL.IsActive = 1
			 LEFT JOIN  TC_Calls   AS C   WITH(NOLOCK) ON C.TC_LeadId = TCIL.TC_LeadId
			 LEFT OUTER JOIN  vwMMV WITH(NOLOCK)  ON  vwMMV.VersionId=TCSI.CarVersionId
			 WHERE (TCIL.TC_UserId = @UserId OR @UserId IS NULL)	
			 AND A.ScheduledOn <= @ToDate 
			 AND ISNULL(A.IsActionTaken,0) = 1 AND (A.ActionComments <> 'Customer Verified')
			 AND A.IsActionTaken = 1
			ORDER BY TCSI.CreatedOn DESC
		  END               
	END

