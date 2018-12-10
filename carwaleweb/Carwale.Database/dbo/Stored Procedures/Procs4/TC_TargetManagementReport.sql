IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_TargetManagementReport]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_TargetManagementReport]
GO

	-- =============================================
-- Author	:	Sachin Bharti
-- Create date	:	25th Sep 2013
-- Description	:	Get all details for Target Management 	
-- Modified by: Deepak on 16-10-2013 for resolving the issue of not loading report. Exact reason not identified under monitoring.
-- =============================================
CREATE PROCEDURE [dbo].[TC_TargetManagementReport]
	@FromDate	DateTime,
	@ToDate		DateTime,
	@BRANCHID	INT,
	@ReportingUsersList VARCHAR(MAX) = '-1'
AS
BEGIN
	
	------Below code is added by Deepak on 16-10-2013 for resolving the issue of not loading report. Exact reason not identified under monitoring----
	     DECLARE @TempUsers Table(UserId INT)
		
		IF (@ReportingUsersList IS NOT NULL)
		  BEGIN 
			INSERT INTO @TempUsers (UserId)
			SELECT ListMember AS UserId FROM fnSplitCSV(@ReportingUsersList)
		  END 
	--------------------------------------------------------------------------------------------------------------------------
	
	SELECT TU.UserName,TU.lvl,TU.Id AS TC_UsersId,
	COUNT(DISTINCT (CASE WHEN	TIL.CreatedDate BETWEEN @FromDate AND @ToDate THEN TIL.TC_LeadId END)) AS TotalLead,
	COUNT(DISTINCT (CASE WHEN	TCN.TC_LeadDispositionId = 4  AND TCN.BookingDate BETWEEN @FromDate AND @ToDate THEN TCN.TC_NewCarInquiriesId END )) AS BookedLead, 
	COUNT(DISTINCT (CASE WHEN	TCN.CarDeliveryStatus = 77 AND TCN.CarDeliveryDate BETWEEN @FromDate AND @ToDate   THEN TCN.TC_NewCarInquiriesId END )) AS DeliveredLead,
	COUNT(DISTINCT (CASE WHEN	TB.Invoicedate  IS NOT NULL AND TB.InvoiceDate BETWEEN @FromDate AND @ToDate THEN TCN.TC_NewCarInquiriesId END )) AS RetailLead
	FROM DEALERS as D WITH (NOLOCK)																									   
	INNER JOIN TC_Users TU WITH (NOLOCK) ON D.ID = TU.BranchId 
	                        AND D.ID = @BranchId 
							AND TU.BranchId = @BranchId AND TU.lvl <> 0 
							AND TU.IsActive = 1 
							AND (TU.Id IN (SELECT  UserId FROM @TempUsers) 
							          OR @ReportingUsersList IS NULL)
	LEFT JOIN TC_InquiriesLead AS TIL WITH (NOLOCK) ON TU.ID=TIL.TC_UserId  AND TIL.TC_LeadInquiryTypeId = 3 
	                           AND (TIL.TC_UserId IN (SELECT  UserId FROM @TempUsers ) 
							         OR @ReportingUsersList IS NULL)
	LEFT JOIN TC_NewCarInquiries AS TCN WITH (NOLOCK) ON TIL.TC_InquiriesLeadId=TCN.TC_InquiriesLeadId
	LEFT JOIN TC_NewCarBooking AS TB WITH (NOLOCK) ON TB.TC_NewCarInquiriesId = TCN.TC_NewCarInquiriesId
	WHERE TIL.BranchId = @BRANCHID
	GROUP BY TU.UserName,TU.lvl,TU.Id

	SELECT TC_UsersId,T.TC_TargetTypeId,T.Target 
	FROM TC_UserModelsTarget AS T
	JOIN TC_Users AS U ON T.TC_UsersId=U.Id
		                    AND U.BranchId=@BRANCHID
							AND U.IsActive=1
    WHERE [MONTH]= MONTH(GETDATE()) 
	AND [Year] = YEAR(GETDATE())


END
