IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_APIReportForSuperUser]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_APIReportForSuperUser]
GO

	-- =============================================
-- Author:		Vishal Srivastava AE1830
-- Create date: 12022014 0704 hrs ist
-- Description:	Get sUPERuSER Report For API
-- Modified By Vivek Gupta on 19-03-2014, Changed 'as inquiry' to 'as total' and 'as TodayInquiry' to 'as TodayTotal'
-- =============================================
CREATE PROCEDURE [dbo].[TC_APIReportForSuperUser]
	-- Add the parameters for the stored procedure here
	@fromDate DATETIME,
	@toDate DATETIME,
	@USERiD INT,
	@versionid INT =null,
	@modelid INT =null
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @RMId TINYINT = NULL,
			@AMId TINYINT = NULL

	SELECT @RMId=TC_SpecialUsersId FROM TC_SpecialUsers WITH(NOLOCK) WHERE TC_SpecialUsersId=@USERiD AND Designation=3
	SELECT @AMId=TC_SpecialUsersId FROM TC_SpecialUsers WITH(NOLOCK) WHERE TC_SpecialUsersId=@USERiD AND Designation=4

    
	SELECT 
	COUNT(DISTINCT (CASE WHEN TBS.CreatedDate BETWEEN @FromDate AND @ToDate THEN TBS.TC_NewCarInquiriesId END )) AS Total,  -- Modified By Vivek Gupta on 19-03-2014
		COUNT(DISTINCT (CASE WHEN  CONVERT(date, TBS.CreatedDate) = CONVERT(date, GETDATE()) THEN TBS.TC_NewCarInquiriesId END )) AS TodayTotal, 
		COUNT(DISTINCT (CASE WHEN  TBS.TC_LeadDispositionID = 4 AND TBS.BookingStatus=32 AND TBS.BookingDate BETWEEN @FromDate AND @ToDate THEN TBS.TC_NewCarInquiriesId END )) AS Booked, 
		COUNT(DISTINCT (CASE WHEN   TBS.TC_LeadDispositionID = 4 AND TBS.BookingStatus=32 AND CONVERT(date, TBS.BookingDate) = CONVERT(date, GETDATE()) THEN TBS.TC_NewCarInquiriesId END )) AS TodayBooked,		 
		COUNT(DISTINCT (CASE WHEN  TBS.CarDeliveryStatus = 77 AND TBS.CarDeliveryDate  BETWEEN @FromDate AND @ToDate THEN TBS.TC_NewCarInquiriesId END )) AS Delivery,
		COUNT(DISTINCT (CASE WHEN  TBS.CarDeliveryStatus = 77 AND CONVERT(DATE, TBS.CarDeliveryDate) = CONVERT(DATE, GETDATE()) THEN TBS.TC_NewCarInquiriesId END )) AS TodayDelivery,
		COUNT(DISTINCT (CASE WHEN TBS.Invoicedate  IS NOT NULL AND TBS.InvoiceDate BETWEEN @FromDate AND @ToDate THEN TBS.TC_NewCarInquiriesId END )) AS Retail,
		COUNT(DISTINCT (CASE WHEN TBS.Invoicedate  IS NOT NULL AND CONVERT(DATE,TBS.InvoiceDate)= CONVERT(DATE,GETDATE())  THEN TBS.TC_NewCarInquiriesId END )) AS TodayRetail,

		COUNT(DISTINCT (CASE WHEN TBS.TestDriveStatus=28 AND TBS.TestDriveDate BETWEEN @fromDate AND @toDate THEN TBS.TC_NewCarInquiriesId END)) AS TestDrive,
		COUNT(DISTINCT (CASE WHEN TBS.TestDriveStatus=28 AND CONVERT(DATE,TBS.TestDriveDate) = CONVERT(DATE,GETDATE()) THEN TBS.TC_NewCarInquiriesId END)) AS TodayTestDrive,
		COUNT(DISTINCT (CASE WHEN TBS.TC_LeadStageId=3 AND  TBS.TC_LeadDispositionID<>4 AND TBS.LeadClosedDate BETWEEN @FromDate AND @toDate THEN TBS.TC_LeadId END )) AS Lost,
		COUNT(DISTINCT (CASE WHEN TBS.TC_LeadStageId=3 AND  TBS.TC_LeadDispositionID<>4 AND CONVERT(DATE,TBS.LeadClosedDate) = CONVERT(DATE,GETDATE()) THEN TBS.TC_LeadId END )) AS TodayLost


		From
		DEALERS as D WITH (NOLOCK)
		LEFT JOIN TC_LeadBasedSummary TBS WITH (NOLOCK) ON D.ID = TBS.DealerId 
		INNER JOIN TC_BrandZone TBZ WITH (NOLOCK) ON D.TC_BrandZoneId = TBZ.TC_BrandZoneId AND TBZ.MakeId = 20 AND  D.IsDealerActive= 1
		INNER JOIN TC_SpecialUsers TSU WITH (NOLOCK) ON D.TC_RMId = TSU.TC_SpecialUsersId AND TSU.IsActive = 1
		INNER JOIN TC_SpecialUsers TSU1 WITH (NOLOCK) ON D.TC_AMId = TSU1.TC_SpecialUsersId AND TSU1.IsActive = 1
	WHERE --(D.TC_RMID = @RMId OR @RMId IS NULL) 
		(D.TC_RMID = isnull(@RMId,D.TC_RMID))	
		AND (D.TC_AMId = isnull(@AMId,D.TC_AMId))
		AND (@ModelId IS NULL OR TBS.CarModelId = @ModelId)
		AND (@versionid IS NULL OR TBS.CarVersionId = @versionid)
END
