IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SubInquiryDetailsSellerAll]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SubInquiryDetailsSellerAll]
GO

	CREATE  PROCEDURE [dbo].[TC_SubInquiryDetailsSellerAll] 
  (
	@BranchId int,
	@FromDate DATETIME,
	@ToDate  DATETIME,
	@ReportId TINYINT  =1
   )
AS
	BEGIN		 
				WITH CTE0 AS (
								SELECT DISTINCT 
								TC.Id , 
								TC.CustomerName, 
								TC.Email, 
								TC.Mobile, 
								TC.Location, 
								year(TCBI.MakeYear) as MakeYear,
								TCBI.Price,
								TCBI.Kms,
								TCBI.Colour,
								TCBI.CreatedOn AS EntryDate, 
								TU.UserName,
								TCIL.TC_InquiryStatusId AS Eagerness, --Added by Afrose
								TCAC.LastCallComment AS Comment,--Added by Afrose
								vwMMV.Car,
								CASE TCIS.Source WHEN 'Other Sources' THEN 'Other Sources-' + ' ' +(SELECT SourceName FROM TC_OtherInquirySources WITH (NOLOCK) WHERE InquiryId = TCBI.TC_SellerInquiriesId)                                                               ELSE TCIS.Source END AS  Source,  --Modified by Afrose
								TL.TC_LeadId,
								TCIL.TC_UserId,
								TL.TC_LeadStageId,
								0 AS IsTDRequested,
								'' AS TDRequestedDate,
								'' AS  NextFollowUpDT,
								ROW_NUMBER() OVER (PARTITION BY TCBI.TC_InquiriesLeadId ORDER By TCBI.CreatedOn DESC) as ROW
								FROM TC_CustomerDetails AS TC   WITH(NOLOCK)  
								JOIN  TC_Lead          AS TL   WITH(NOLOCK) ON TL.TC_CustomerId= TC.Id 
								JOIN  TC_InquiriesLead AS TCIL WITH(NOLOCK) ON TL.TC_LeadId=TCIL.TC_LeadId 
																			AND TCIL.TC_LeadInquiryTypeId=2
																			AND  TL.BranchId	= @BranchId 
																			AND  TCIL.BranchId=  @BranchId 
								JOIN TC_SellerInquiries AS TCBI WITH(NOLOCK)   ON TCBI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId 
																		  AND TCBI.CreatedOn BETWEEN @FromDate AND @ToDate
								LEFT OUTER JOIN  TC_Users         AS TU   WITH(NOLOCK)  ON TU.Id = TCIL.TC_UserId
								LEFT OUTER JOIN  vwMMV WITH(NOLOCK)  ON  vwMMV.VersionId=TCBI.CarVersionId
								LEFT OUTER JOIN  TC_InquirySource TCIS  WITH(NOLOCK)  ON  TCBI.TC_InquirySourceId=TCIS.Id-- Modified By Vivek Gupta on 24-02-2014
								LEFT OUTER JOIN TC_ActiveCalls AS TCAC WITH(NOLOCK) ON TCAC.TC_LeadId=TL.TC_LeadId --Added by Afrose 
								--	ORDER BY TCBI.CreatedOn DESC   
							)	
				SELECT * FROM CTE0 WHERE ROW = 1
		END 