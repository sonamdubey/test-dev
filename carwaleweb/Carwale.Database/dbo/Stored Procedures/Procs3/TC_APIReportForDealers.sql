IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_APIReportForDealers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_APIReportForDealers]
GO

	-- =============================================
-- Author:		Vishal Srivastava AE1830
-- Create date: 12022014 0704 hrs ist
-- Description:	Get Dealers Report For API
-- Modified By Vivek Gupta on 19-03-2014, Changed 'as inquiry' to 'as total' and 'as TodayInquiry' to 'as TodayTotal'
-- Modified By Vishal Srivastava AE1830 on 21 may 2014, changed user id to all user reporting to that user
---Modified by Manish on 21-05-2014 taking count of tc_leadid in place of tc_inquiriesleadId for total inquiry in New car Report.
-- =============================================
CREATE PROCEDURE [dbo].[TC_APIReportForDealers]
	-- Add the parameters for the stored procedure here
	@fromDate DATETIME,
	@toDate DATETIME,
	@userId INT,
	@versionid INT,
	@modelid INT,
	@table varchar(10) OUTPUT
AS
BEGIN
		DECLARE @DealerType TINYINT
		DECLARE @BranchId INT
		declare @lvl int
		
		DECLARE @TblAllChild TABLE (Id int)
		INSERT INTO @TblAllChild EXEC TC_GetALLChild @userId;

			set @fromDate = convert(datetime,convert(varchar(10),@fromDate,120)+ ' 00:00:00')	
	        set @toDate = convert(datetime,convert(varchar(10),@toDate,120)+ ' 23:59:59');


		SET @table=''
		
		SELECT @DealerType=Dealers.TC_DealerTypeId , @BranchId=Dealers.ID, @lvl=TC_Users.lvl FROM Dealers JOIN TC_Users ON Dealers.ID=TC_Users.BranchId where TC_Users.Id=@userId

		IF(@DealerType=2 OR @DealerType=3)
		BEGIN
			IF(@lvl=0)
				BEGIN
					SELECT
					COUNT(DISTINCT(CASE WHEN TCIL.CreatedDate BETWEEN @FromDate AND @toDate THEN TCIL.TC_InquiriesLeadId END) ) AS Total, -- Modified By Vivek Gupta on 19-03-2014
					COUNT(DISTINCT(CASE WHEN TCIL.CreatedDate =GETDATE() THEN TCIL.TC_InquiriesLeadId END) ) AS TodayTotal  ,
					COUNT(DISTINCT (CASE WHEN  TCNCI.CarDeliveryStatus=77 and TCNCI.CarDeliveryDate BETWEEN @fromDate AND @toDate THEN TCNCI.TC_NewCarInquiriesId END )) AS Delivery,
					COUNT(DISTINCT (CASE WHEN  TCNCI.CarDeliveryStatus=77 and TCNCI.CarDeliveryDate = GETDATE() THEN TCNCI.TC_NewCarInquiriesId END )) AS TodayDelivery,
					COUNT(DISTINCT (CASE WHEN  TCNCI.TC_LeadDispositionId = 4 AND TCNCI.BookingDate BETWEEN @FromDate AND @toDate THEN TCNCI.TC_NewCarInquiriesId END )) AS Booked,
					COUNT(DISTINCT (CASE WHEN  TCNCI.TC_LeadDispositionId = 4 AND TCNCI.BookingDate = getdate() THEN TCNCI.TC_NewCarInquiriesId END )) AS TodayBooked,
					COUNT(DISTINCT (CASE WHEN  NB.InvoiceDate IS NOT NULL AND NB.InvoiceDate BETWEEN @FromDate AND @toDate THEN TCNCI.TC_NewCarInquiriesId END )) AS Retail,
					COUNT(DISTINCT (CASE WHEN  NB.InvoiceDate IS NOT NULL AND NB.InvoiceDate = GETDATE() THEN TCNCI.TC_NewCarInquiriesId END )) AS TodayRetail,
					COUNT(DISTINCT (CASE WHEN  TCNCI.TDStatus = 28 AND TCNCI.TDDate BETWEEN @fromDate AND @toDate THEN TCNCI.TC_NewCarInquiriesId END)) AS TestDrive, 
					COUNT(DISTINCT (CASE WHEN  TCNCI.TDStatus = 28 AND TCNCI.TDDate = GETDATE() THEN TCNCI.TC_NewCarInquiriesId END)) AS TodayTestDrive ,
					COUNT(DISTINCT (CASE WHEN (TCIL.TC_LeadStageId=3 AND  TCIL.TC_LeadDispositionID<>4)AND TL.LeadClosedDate BETWEEN @FromDate AND @toDate THEN TCIL.TC_LeadId END )) AS Lost,
					COUNT(DISTINCT (CASE WHEN (TCIL.TC_LeadStageId=3 AND  TCIL.TC_LeadDispositionID<>4)AND TL.LeadClosedDate = GETDATE() THEN TCIL.TC_LeadId END )) AS TodayLost,
					COUNT(DISTINCT (CASE WHEN  TCNCI.TDStatus <> 27 AND TCNCI.TDStatus <> 28 AND TCNCI.TDDate <= @toDate THEN TCNCI.TC_NewCarInquiriesId END)) AS PendingTestDrive,
					COUNT(DISTINCT (CASE WHEN TCAC.ScheduledOn <= @toDate THEN TCIL.TC_LeadId END )) AS PendingFollowUp
   							FROM
							TC_Users U WITH (NOLOCK) 
							 JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON U.ID=TCIL.TC_UserId  AND TCIL.TC_LeadInquiryTypeId = 3
							JOIN TC_Lead AS TL WITH(NOLOCK) ON TL.TC_LeadId = TCIL.TC_LeadId
							LEFT JOIN TC_ActiveCalls AS TCAC WITH (NOLOCK) ON TCAC.TC_LeadId=TCIL.TC_LeadId
							LEFT JOIN TC_NewCarInquiries AS TCNCI WITH (NOLOCK) ON TCIL.TC_InquiriesLeadId=TCNCI.TC_InquiriesLeadId
							LEFT JOIN TC_NewCarBooking AS NB WITH (NOLOCK) ON NB.TC_NewCarInquiriesId = TCNCI.TC_NewCarInquiriesId
							WHERE (@versionid IS NULL OR TCNCI.VersionId=@versionid)
							AND(@modelid IS NULL OR TCNCI.VersionId IN (SELECT C.ID FROM CarVersions AS C  WITH (NOLOCK)  JOIN CarModels AS M  WITH (NOLOCK)  ON C.CarModelId=M.ID WHERE C.CarModelId=@modelid AND M.New=1))
							AND U.BranchId = @BranchId AND U.lvl <> 0 AND U.IsActive = 1
				END
			ELSE
				BEGIN
					SELECT
					COUNT(DISTINCT(CASE WHEN TCIL.CreatedDate BETWEEN @FromDate AND @toDate THEN TCIL.TC_LeadId END) ) AS Total, -- Modified By Vivek Gupta on 19-03-2014 ---Modified by Manish on 21-05-2014 taking count of tc_leadid in place of tc_inquiriesleadId
					COUNT(DISTINCT(CASE WHEN TCIL.CreatedDate =GETDATE() THEN TCIL.TC_InquiriesLeadId END) ) AS TodayTotal  ,
					COUNT(DISTINCT (CASE WHEN  TCNCI.CarDeliveryStatus=77 and TCNCI.CarDeliveryDate BETWEEN @fromDate AND @toDate THEN TCNCI.TC_NewCarInquiriesId END )) AS Delivery,
					COUNT(DISTINCT (CASE WHEN  TCNCI.CarDeliveryStatus=77 and TCNCI.CarDeliveryDate = GETDATE() THEN TCNCI.TC_NewCarInquiriesId END )) AS TodayDelivery,
					COUNT(DISTINCT (CASE WHEN  TCNCI.TC_LeadDispositionId = 4 AND TCNCI.BookingDate BETWEEN @FromDate AND @toDate THEN TCNCI.TC_NewCarInquiriesId END )) AS Booked,
					COUNT(DISTINCT (CASE WHEN  TCNCI.TC_LeadDispositionId = 4 AND TCNCI.BookingDate = getdate() THEN TCNCI.TC_NewCarInquiriesId END )) AS TodayBooked,
					COUNT(DISTINCT (CASE WHEN  NB.InvoiceDate IS NOT NULL AND NB.InvoiceDate BETWEEN @FromDate AND @toDate THEN TCNCI.TC_NewCarInquiriesId END )) AS Retail,
					COUNT(DISTINCT (CASE WHEN  NB.InvoiceDate IS NOT NULL AND NB.InvoiceDate = GETDATE() THEN TCNCI.TC_NewCarInquiriesId END )) AS TodayRetail,
					COUNT(DISTINCT (CASE WHEN (TCIL.TC_LeadStageId=3 AND  TCIL.TC_LeadDispositionID<>4)AND TL.LeadClosedDate BETWEEN @FromDate AND @toDate THEN TCIL.TC_LeadId END )) AS Lost,
					COUNT(DISTINCT (CASE WHEN (TCIL.TC_LeadStageId=3 AND  TCIL.TC_LeadDispositionID<>4)AND TL.LeadClosedDate = GETDATE() THEN TCIL.TC_LeadId END )) AS TodayLost,
					COUNT(DISTINCT (CASE WHEN  TCNCI.TDStatus = 28 AND TCNCI.TDDate BETWEEN @fromDate AND @toDate THEN TCNCI.TC_NewCarInquiriesId END)) AS TestDrive, 
					COUNT(DISTINCT (CASE WHEN  TCNCI.TDStatus = 28 AND TCNCI.TDDate = GETDATE() THEN TCNCI.TC_NewCarInquiriesId END)) AS TodayTestDrive ,
					COUNT(DISTINCT (CASE WHEN  TCNCI.TDStatus <> 27 AND TCNCI.TDStatus <> 28 AND TCNCI.TDDate <= @toDate THEN TCNCI.TC_NewCarInquiriesId END)) AS PendingTestDrive,
					COUNT(DISTINCT (CASE WHEN TCAC.ScheduledOn <= @toDate THEN TCIL.TC_LeadId END )) AS PendingFollowUp
   							FROM
							TC_Users U WITH (NOLOCK) 
							 JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON U.ID=TCIL.TC_UserId  AND TCIL.TC_LeadInquiryTypeId = 3 AND ( U.ID IN (SELECT ID FROM @TBLALLCHILD) OR U.ID=@USERID )-- Modified By Vishal Srivastava AE1830 on 21 may 2014, changed user id to all user reporting to that user
							JOIN TC_Lead AS TL WITH(NOLOCK) ON TL.TC_LeadId = TCIL.TC_LeadId
							LEFT JOIN TC_ActiveCalls AS TCAC WITH (NOLOCK) ON TCAC.TC_LeadId=TCIL.TC_LeadId
							LEFT JOIN TC_NewCarInquiries AS TCNCI WITH (NOLOCK) ON TCIL.TC_InquiriesLeadId=TCNCI.TC_InquiriesLeadId
							LEFT JOIN TC_NewCarBooking AS NB WITH (NOLOCK) ON NB.TC_NewCarInquiriesId = TCNCI.TC_NewCarInquiriesId
							WHERE (@versionid IS NULL OR TCNCI.VersionId=@versionid)
							AND(@modelid IS NULL OR TCNCI.VersionId IN (SELECT C.ID FROM CarVersions AS C  WITH (NOLOCK)  JOIN CarModels AS M  WITH (NOLOCK)  ON C.CarModelId=M.ID WHERE C.CarModelId=@modelid AND M.New=1))
							AND U.BranchId = @BranchId AND U.lvl <> 0 AND U.IsActive = 1
				END
		SET @table += '1,'
		END
		IF(@DealerType=1 OR @DealerType=3)
		BEGIN
		SELECT
			   COUNT(DISTINCT(CASE WHEN (TCBI.CreatedOn BETWEEN @FromDate AND @TODate AND TCIL.TC_LeadStageId=1 and TCBI.TC_LeadDispositionId IS NULL)THEN TCBI.TC_BuyerInquiriesId END )) + COUNT(DISTINCT(CASE WHEN (TCBI.CreatedOn BETWEEN @FromDate AND @TODate AND TCIL.TC_LeadStageId=2 and TCBI.TC_LeadDispositionId IS NULL)THEN TCBI.TC_BuyerInquiriesId END )) AS Total ,
			   COUNT(DISTINCT(CASE WHEN (TCBI.CreatedOn = GETDATE() AND TCIL.TC_LeadStageId=1 and TCBI.TC_LeadDispositionId IS NULL)THEN TCBI.TC_BuyerInquiriesId END )) + COUNT(DISTINCT(CASE WHEN (TCBI.CreatedOn =GETDATE() AND TCIL.TC_LeadStageId=2 and TCBI.TC_LeadDispositionId IS NULL)THEN TCBI.TC_BuyerInquiriesId END )) AS TodayTotal ,
			   COUNT(DISTINCT(CASE WHEN (TCBI.TC_LeadDispositionId=4  AND TCBI.Lastupdateddate BETWEEN @FromDate AND @TODate)  THEN TCBI.TC_BuyerInquiriesId END)) AS Delivery,
			   COUNT(DISTINCT(CASE WHEN (TCBI.TC_LeadDispositionId=4  AND TCBI.Lastupdateddate = GETDATE())  THEN TCBI.TC_BuyerInquiriesId END)) AS TodayDelivery,
			   COUNT(DISTINCT(CASE WHEN (CB.BookingDate BETWEEN @FromDate AND @TODate AND TCBI.BookingStatus=34)THEN CB.TC_CarBookingId END)) AS Booked,
			    COUNT(DISTINCT(CASE WHEN (CB.BookingDate=GETDATE() AND TCBI.BookingStatus=34)THEN CB.TC_CarBookingId END)) AS TodayBooked,
			   COUNT(DISTINCT(CASE WHEN (TCBI.TC_LeadDispositionID<>1
											  AND TCBI.TC_LeadDispositionID<>3
											  AND TCBI.TC_LeadDispositionID<>4
											  AND TCBI.TC_LeadDispositionID IS NOT NULL AND TCBI.CreatedOn BETWEEN @FromDate AND @TODate )THEN TCBI.TC_BuyerInquiriesId END)) AS Lost,
			   COUNT(DISTINCT(CASE WHEN TCIL.TC_LeadStageId = 1  AND TCIL.TC_UserId IS NOT NULL  AND TCBI.TC_LeadDispositionID IS NULL AND TCBI.CreatedOn BETWEEN @FromDate AND @TODate THEN TCBI.TC_BuyerInquiriesId END)) AS PendingCall, 
				COUNT(DISTINCT (CASE WHEN TCIL.TC_LeadDispositionId NOT IN (11,12,13,14,15) AND CONVERT(DATE,AC.ScheduledOn) = CONVERT(DATE, GETDATE()) THEN AC.TC_CallsId END)) AS TodayCall
			   FROM TC_Lead  AS TCL WITH(NOLOCK)
			    JOIN TC_InquiriesLead AS TCIL WITH(NOLOCK)  ON TCL.TC_LeadId=TCIL.TC_LeadId AND  TCIL.TC_UserId=@userId AND  TCIL.TC_LeadInquiryTypeId=1
				JOIN TC_BuyerInquiries AS TCBI WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId=TCBI.TC_InquiriesLeadId
				JOIN DEALERS as D WITH (NOLOCK)	ON D.ID=TCIL.BranchId																								  
				JOIN TC_Users U WITH (NOLOCK) ON D.ID = U.BranchId AND U.BranchId = @BranchId AND U.lvl <> 0 AND U.IsActive = 1 AND U.Id=@userId
				
				LEFT JOIN TC_CarBooking AS CB WITH(NOLOCK) ON CB.CustomerId=TCIL.TC_CustomerId
				LEFT JOIN TC_ActiveCalls AS AC WITH(NOLOCK) ON AC.TC_LeadId=TCIL.TC_LeadId
				
			-- WHERE (@versionid IS NULL OR TCBI.CarVersionId=@versionid)
			--AND(@modelid IS NULL OR TCBI.CarVersionId IN (SELECT C.ID FROM CarVersions AS C JOIN CarModels AS M ON C.CarModelId=M.ID WHERE C.CarModelId=@modelid AND M.New=1))

			
			
			
			SELECT
			 COUNT(DISTINCT(CASE WHEN (SI.CreatedOn BETWEEN @fromDate AND @toDate) AND   IL.TC_LeadStageId = 1 THEN SI.TC_SellerInquiriesId END)) +
			 COUNT(DISTINCT(CASE WHEN (SI.CreatedOn BETWEEN @fromDate AND @toDate)  AND  IL.TC_LeadStageId = 2 AND SI.TC_LeadDispositionId IS NULL  THEN SI.TC_SellerInquiriesId END)) AS Total,
			 COUNT(DISTINCT(CASE WHEN (SI.CreatedOn =GETDATE()) AND   IL.TC_LeadStageId = 1 THEN SI.TC_SellerInquiriesId END)) +
			 COUNT(DISTINCT(CASE WHEN (SI.CreatedOn =GETDATE())  AND  IL.TC_LeadStageId = 2 AND SI.TC_LeadDispositionId IS NULL  THEN SI.TC_SellerInquiriesId END)) AS TodayTotal,
				COUNT(DISTINCT(CASE WHEN (SI.TC_LeadDispositionID = 4 AND SI.PurchasedDate BETWEEN @fromDate AND @toDate) THEN SI.TC_SellerInquiriesId END)) AS Booked ,
				COUNT(DISTINCT(CASE WHEN (SI.TC_LeadDispositionID = 4 AND SI.PurchasedDate =GETDATE()) THEN SI.TC_SellerInquiriesId END)) AS TodayBooked ,
				 COUNT(DISTINCT(CASE WHEN     IL.TC_LeadStageId = 1 AND IL.TC_UserId IS NOT NULL AND SI.TC_LeadDispositionID IS NULL THEN SI.TC_SellerInquiriesId END)) AS PendingCall, 
				COUNT(DISTINCT (CASE WHEN     IL.TC_LeadStageId = 1 AND IL.TC_UserId IS NOT NULL AND SI.TC_LeadDispositionID IS NULL AND SI.CreatedOn = GETDATE()   THEN SI.TC_SellerInquiriesId END)) AS TodayCall
			FROM TC_SellerInquiries AS SI 
			 JOIN TC_InquiriesLead AS IL ON SI.TC_InquiriesLeadId=IL.TC_InquiriesLeadId
			 AND SI.CreatedOn BETWEEN @fromDate AND @toDate 
			 AND IL.TC_UserId=@userId
			 JOIN TC_ActiveCalls AS AC WITH(NOLOCK) ON AC.TC_LeadId=IL.TC_LeadId
			 JOIN DEALERS as D WITH (NOLOCK)	ON D.ID=IL.BranchId																								  
				JOIN TC_Users U WITH (NOLOCK) ON D.ID = U.BranchId AND D.ID = @BranchId AND U.BranchId = @BranchId AND U.lvl <> 0 AND U.IsActive = 1 AND  U.Id=@userId
			 WHERE (@versionid IS NULL OR SI.CarVersionId=@versionid)
			AND(@modelid IS NULL OR SI.CarVersionId IN (SELECT C.ID FROM CarVersions AS C JOIN CarModels AS M ON C.CarModelId=M.ID WHERE C.CarModelId=@modelid AND M.New=1))
		SET @table += '2,3'	
		END
END