IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UnassigendLeadAndEnquiryReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UnassigendLeadAndEnquiryReport]
GO

	

-- ================================================================================================
-- Author: Vivek Rajak
-- Create date: 06 july,2015
-- Description:	This Proc Returns combined report of unassiend lead and unassined enquiry.
-- Mofified By : Afrose on 11th November 2015, changed input parameter branchid and userid from bigint to int
-- Modified By : Upendra Kumar 30 Nov 2015 , 
--exec TC_UnassigendLeadAndEnquiryReport 5,1,243,0,'2014/11/30','2015/12/01',1,50 assignedTo : 243
-- Modified by :Ashwini Dhamankar on Dec 15,2015 (added constraint of MostInterested=1	)
--exec TC_UnassigendLeadAndEnquiryReport 3838,1,18,0,'2015-12-01 12:00:00 AM','2015-12-21 11:59:59 PM' ,1,500
-- ================================================================================================
CREATE PROCEDURE [dbo].[TC_UnassigendLeadAndEnquiryReport] 
@BranchId    INT,
@InquiryType TINYINT,
@UserId    INT,
@ReqFromUnassignedPage BIT=0,
@FromDate DATE=NULL,
@ToDate   DATE=NULL,
@FromIndex  INT=1, 
@ToIndex  INT=20
AS
BEGIN
	IF (@InquiryType=1)--buyer
		BEGIN		
			WITH CTE AS 
			(SELECT TC_ImportBuyerInquiriesId AS Id,Name,Email,Mobile,Location,CarDetails,EntryDate AS LeadCreationDate ,1 AS ReportId
			     ,ROW_NUMBER() OVER (PARTITION BY TC_ImportBuyerInquiriesId ORDER By EntryDate DESC) as ROW --added by upendra 
			    FROM TC_ImportBuyerInquiries B WITH(NOLOCK)
				WHERE B.BranchId = @BranchId 
					AND B.IsDeleted=0 
					AND B.UserId=@UserId 
					AND B.TC_BuyerInquiriesId IS NULL     
					AND (@FromDate IS NULL OR  CONVERT(DATE,B.EntryDate)>= @FromDate) 
					AND (@ToDate IS NULL OR  CONVERT(DATE,B.EntryDate) <= @ToDate)
			UNION 
			SELECT L.TC_LeadId AS Id,C.CustomerName AS Name,C.Email, C.Mobile,C.Location as Location,TCIL.CarDetails as CarDetails,TCBI.CreatedOn AS LeadCreationDate, 2 as ReportId 
			      ,ROW_NUMBER() OVER (PARTITION BY L.TC_LeadId ORDER By TCBI.CreatedOn DESC) as ROW --added by upendra     
			   FROM TC_Lead L WITH(NOLOCK)
			   INNER JOIN TC_CustomerDetails C WITH(NOLOCK) ON   L.TC_CustomerId = C.Id								
			   JOIN TC_InquiriesLead  AS TCIL WITH(NOLOCK) ON   L.TC_LeadId = TCIL.TC_LeadId	
			   JOIN TC_BuyerInquiries AS TCBI WITH(NOLOCK) ON TCBI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId AND ISNULL(TCBI.MostInterested,0)=1	
			   WHERE C.BranchId = @BranchId  
			        AND L.TC_LeadStageId IS NULL 
					AND TCIL.TC_LeadInquiryTypeId=@InquiryType 				 
					AND (@FromDate IS NULL OR  CONVERT(DATE,TCBI.CreatedOn)>= @FromDate) 
					AND (@ToDate IS NULL OR  CONVERT(DATE,TCBI.CreatedOn) <= @ToDate)
				) 
			

	  SELECT Id,Name,Email,Mobile,Location,CarDetails,LeadCreationDate,ReportId,ROW_NUMBER() OVER ( ORDER BY CTE.LeadCreationDate DESC ) rownumber 
      INTO   #tblTempBuyer1
      FROM  CTE WITH(NOLOCK)  WHERE ROW = 1 ORDER BY LeadCreationDate DESC;	 
	  
      SELECT Id,Name,Email,Mobile,Location,CarDetails,LeadCreationDate,ReportId  
      FROM   #tblTempBuyer1
      WHERE  rownumber BETWEEN @FromIndex AND @ToIndex 
     

      SELECT COUNT(1) AS RecordCount 
      FROM   #tblTempBuyer1

      DROP TABLE #tblTempBuyer1
	END

	IF (@InquiryType=2)--Seller

		BEGIN
		 WITH CTE AS
		  (SELECT TC_ImportSellerInquiriesId AS Id,Name,Email,Mobile,Location,( ISNULL(CarMake,'') + ' ' + ISNULL(CarModel,'') + ' ' + ISNULL(CarVersion,'')) AS CarDetails,B.EntryDate AS LeadCreationDate ,1 as ReportId
					,ROW_NUMBER() OVER (PARTITION BY TC_ImportSellerInquiriesId ORDER By B.EntryDate DESC) as ROW --added by upendra 
			  FROM TC_ImportSellerInquiries B WITH(NOLOCK)
			  WHERE BranchId = @BranchId
					AND B.IsDeleted=0 
					AND B.UserId=@UserId 
					AND B.TC_ImportSellerInquiriesId IS NULL
					AND	(@FromDate IS NULL OR  CONVERT(DATE,EntryDate) >= @FromDate) 
					AND (@ToDate IS NULL OR  CONVERT(DATE,EntryDate) <= @ToDate)
			UNION 
			SELECT L.TC_LeadId AS Id,C.CustomerName AS Name,C.Email, C.Mobile,C.Location,TCIL.CarDetails,TCBI.CreatedOn AS LeadCreationDate, 2 AS ReportId 
					 ,ROW_NUMBER() OVER (PARTITION BY L.TC_LeadId ORDER By TCBI.CreatedOn DESC) as ROW --added by upendra    
			   FROM 
			   TC_Lead L WITH(NOLOCK)  
			    INNER JOIN TC_CustomerDetails C WITH(NOLOCK) ON   L.TC_CustomerId = C.Id 																
																																
			   JOIN TC_InquiriesLead  AS TCIL WITH(NOLOCK) ON    L.TC_LeadId = TCIL.TC_LeadId 
																 
			   JOIN TC_SellerInquiries AS TCBI WITH(NOLOCK) ON TCBI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId
			   WHERE C.BranchId = @BranchId  
			        AND L.TC_LeadStageId IS NULL 
					AND  TCIL.TC_LeadInquiryTypeId=@InquiryType 
					AND	(@FromDate IS NULL OR  CONVERT(DATE,TCBI.CreatedOn)>= @FromDate) 
					AND (@ToDate IS NULL OR  CONVERT(DATE,TCBI.CreatedOn) <= @ToDate)
				)
		  SELECT Id,Name,Email,Mobile,Location,CarDetails,LeadCreationDate,ReportId ,ROW_NUMBER() OVER ( ORDER BY CTE.LeadCreationDate DESC ) rownumber 
		  INTO   #tblTempSeller 
		  FROM  CTE WITH(NOLOCK)  WHERE ROW = 1 ORDER BY LeadCreationDate DESC;

		  SELECT Id,Name,Email,Mobile,Location,CarDetails,LeadCreationDate,ReportId  
		  FROM   #tblTempSeller
		  WHERE  rownumber BETWEEN @FromIndex AND @ToIndex 
     

		  SELECT COUNT(1) AS RecordCount 
		  FROM   #tblTempSeller 

		  DROP TABLE #tblTempSeller 
		END
		
	IF (@InquiryType=3)--new car
		
		BEGIN
		WITH CTE AS

			(SELECT Id,Name,Email,Mobile,City AS Location,( ISNULL(CarMake,'') + ' ' + ISNULL(CarModel,'') + ' ' + ISNULL(CarVersion,'')) AS CarDetails,B.EntryDate AS LeadCreationDate , 1 as ReportId
				,ROW_NUMBER() OVER (PARTITION BY Id ORDER By B.EntryDate DESC) as ROW --added by upendra 
				FROM TC_ExcelInquiries B WITH(NOLOCK)
				WHERE BranchId = @BranchId
					AND B.IsDeleted=0 
					AND B.UserId=@UserId 
					AND B.TC_NewCarInquiriesId IS NULL
					AND (@FromDate IS NULL OR  CONVERT(DATE,EntryDate) >= @FromDate) 
					AND (@ToDate IS NULL OR  CONVERT(DATE,EntryDate) <= @ToDate)
			UNION 
			SELECT L.TC_LeadId AS Id,C.CustomerName ,C.Email, C.Mobile,C.Location,TCIL.CarDetails ,TCBI.CreatedOn AS LeadCreationDate, 2 AS ReportId
					,ROW_NUMBER() OVER (PARTITION BY L.TC_LeadId ORDER By TCBI.CreatedOn DESC) as ROW --added by upendra      
				FROM TC_Lead L WITH(NOLOCK)  
			   INNER JOIN TC_CustomerDetails C WITH(NOLOCK) ON L.TC_CustomerId = C.Id											
			   JOIN TC_InquiriesLead  AS TCIL WITH(NOLOCK) ON L.TC_LeadId = TCIL.TC_LeadId
			   JOIN TC_NewCarInquiries AS TCBI WITH(NOLOCK) ON TCBI.TC_InquiriesLeadId=TCIL.TC_InquiriesLeadId AND ISNULL(TCBI.MostInterested,0)=1	 
			   WHERE C.BranchId = @BranchId  
			   		AND L.TC_LeadStageId IS NULL 
					AND  TCIL.TC_LeadInquiryTypeId=@InquiryType 
					AND (@FromDate IS NULL OR  CONVERT(DATE,TCBI.CreatedOn) >= @FromDate) 
					AND	(@ToDate IS NULL OR  CONVERT(DATE,TCBI.CreatedOn) <= @ToDate)

			)
			SELECT Id,Name,Email,Mobile,Location,CarDetails,LeadCreationDate,ReportId ,ROW_NUMBER() OVER ( ORDER BY CTE.LeadCreationDate DESC) rownumber 
			  INTO   #tblTempNewCar 
			  FROM  CTE WITH(NOLOCK)  WHERE ROW = 1 ORDER BY LeadCreationDate DESC;

			  SELECT Id,Name,Email,Mobile,Location,CarDetails,LeadCreationDate,ReportId  
			  FROM   #tblTempNewCar 
			  WHERE  rownumber BETWEEN @FromIndex AND @ToIndex 
     

			  SELECT COUNT(1) AS RecordCount 
			  FROM   #tblTempNewCar 

			  DROP TABLE #tblTempNewCar		
			END
	
END


---------------------------------------------

/****** Object:  StoredProcedure [dbo].[TC_SubInquiryDetailsBuyer]    Script Date: 12/22/2015 7:00:05 PM ******/
SET ANSI_NULLS ON
