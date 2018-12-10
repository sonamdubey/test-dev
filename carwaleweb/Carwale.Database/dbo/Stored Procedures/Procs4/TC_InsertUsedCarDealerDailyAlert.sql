IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_InsertUsedCarDealerDailyAlert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_InsertUsedCarDealerDailyAlert]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 20-11-2013
-- Description:	Insert records into TC_UsedCarDealerDailyAlert for sending daily email to used car dealers.
-- Modified By: Vivek Gupta on 28th nov, 2013, modified all conditions for new car inquiries counts.
-- =============================================
CREATE PROCEDURE [dbo].[TC_InsertUsedCarDealerDailyAlert]
AS
BEGIN
	SET NOCOUNT ON;
		
		TRUNCATE TABLE TC_UsedCarDealerDailyAlert;
		
			INSERT INTO TC_UsedCarDealerDailyAlert (TC_UsersId,
			                                        CurrentDayInquiryCount,
													CurrentMonthInquiryCount,
													CurrentDayBookingCount,
													CurrentMonthBookingCount,
													CurrentDayLostCount,
													CurrentMonthLostCount,
													PendingFollowup,
													TomorrowFollowup,
													TC_LeadInquiryTypeId)

			(
				SELECT 
                TCIL.TC_UserId,
                COUNT( DISTINCT (CASE WHEN CONVERT(DATE,TCIL.CreatedDate)=CONVERT(DATE,GETDATE()) THEN  TCIL.TC_InquiriesLeadId END)) AS CurrentDayInquiryCount ,
                COUNT( DISTINCT (CASE WHEN MONTH(TCIL.CreatedDate)=MONTH(GETDATE()) AND YEAR(TCIL.CreatedDate)=YEAR(GETDATE()) THEN  TCIL.TC_InquiriesLeadId END)) AS CurrentMonthInquiryCount ,
                COUNT(DISTINCT (CASE WHEN N.TC_LeadDispositionId = 4 AND CONVERT(DATE,N.BookingDate)=CONVERT(DATE,GETDATE()) THEN   N.TC_NewCarInquiriesId END )) AS CurrentDayBookingCount,
                COUNT(DISTINCT (CASE WHEN N.TC_LeadDispositionId = 4 AND MONTH(N.BookingDate)=MONTH(GETDATE()) AND YEAR(N.BookingDate)=YEAR(GETDATE())   THEN   N.TC_NewCarInquiriesId END )) AS CurrentMonthBookingCount,                
                COUNT(DISTINCT (CASE WHEN TCIL.TC_LeadStageId=3 AND  TCIL.TC_LeadDispositionID<>4 AND CONVERT(DATE,TCL.LeadClosedDate)=CONVERT(DATE,GETDATE()) THEN  TCIL.TC_LeadId END)) CurrentDayLostCount,
                COUNT(DISTINCT (CASE WHEN TCIL.TC_LeadStageId=3 AND  TCIL.TC_LeadDispositionID<>4 AND MONTH(TCL.LeadClosedDate) = MONTH(GETDATE()) AND YEAR(TCL.LeadClosedDate) = YEAR(GETDATE()) THEN  TCIL.TC_LeadId END)) CurrentMonthLostCount,
                COUNT(DISTINCT (CASE WHEN Convert(Date, TCAC.ScheduledOn)  <= Convert(DATE, GETDATE()) THEN TCAC.TC_LeadId END)) PendingFollowup,
                COUNT(DISTINCT (CASE WHEN CONVERT(DATE,TCAC.ScheduledOn) = CONVERT(DATE,GETDATE()+1) THEN TCAC.TC_LeadId END)) TomorrowFollowup,
                3
                FROM TC_InquiriesLead AS TCIL
                JOIN Dealers AS D WITH(NOLOCK) ON TCIL.BranchId = D.ID AND IsDealerActive = 1
                JOIN TC_Users AS TCU WITH(NOLOCK) ON D.ID = TCU.BranchId AND TCU.GCMRegistrationId IS NOT NULL AND TCU.IsActive = 1
                JOIN TC_NewCarInquiries AS N ON TCIL.TC_InquiriesLeadId=N.TC_InquiriesLeadId
                LEFT JOIN TC_ActiveCalls AS TCAC ON TCAC.TC_LeadId=TCIL.TC_LeadId
                LEFT JOIN TC_Lead AS TCL WITH(NOLOCK) ON TCIL.TC_LeadId = TCL.TC_LeadId
                WHERE TCIL.TC_LeadInquiryTypeId=3                      
                GROUP BY TCIL.TC_UserId
			UNION ALL
				SELECT 
				TCIL.TC_UserId,
				COUNT( DISTINCT (CASE WHEN CONVERT(DATE,N.CreatedOn)=CONVERT(DATE,GETDATE()) THEN  N.TC_BuyerInquiriesId END)) AS CurrentDayInquiryCount ,
				COUNT( DISTINCT (CASE WHEN MONTH(N.CreatedOn)=MONTH(GETDATE()) AND YEAR(N.CreatedOn)=YEAR(GETDATE()) THEN  N.TC_BuyerInquiriesId END )) AS CurrentMonthInquiryCount ,
				COUNT(DISTINCT (CASE WHEN N.BookingStatus=34 AND CONVERT(DATE,N.BookingDate)=CONVERT(DATE,GETDATE()) THEN   N.TC_BuyerInquiriesId END )) AS CurrentDayBookingCount,
				COUNT(DISTINCT (CASE WHEN N.BookingStatus=34 AND MONTH(N.BookingDate)=MONTH(GETDATE()) AND YEAR(N.BookingDate)=YEAR(GETDATE())   THEN   N.TC_BuyerInquiriesId END )) AS CurrentMonthBookingCount,
				COUNT(DISTINCT (CASE WHEN N.TC_LeadDispositionId<>1 AND N.TC_LeadDispositionId<>3 AND N.TC_LeadDispositionId<>4 AND N.TC_LeadDispositionId IS NOT NULL AND CONVERT(DATE,TCIL.CreatedDate)=CONVERT(DATE,GETDATE()) THEN  N.TC_BuyerInquiriesId END)) CurrentDayLostCount,
				COUNT(DISTINCT (CASE WHEN N.TC_LeadDispositionId<>1 AND N.TC_LeadDispositionId<>3 AND N.TC_LeadDispositionId<>4 AND N.TC_LeadDispositionId IS NOT NULL AND MONTH(TCIL.CreatedDate)=MONTH(GETDATE()) AND YEAR(TCIL.CreatedDate)=YEAR(GETDATE()) THEN  N.TC_BuyerInquiriesId END)) CurrentMonthLostCount,
				COUNT(DISTINCT (CASE WHEN CONVERT(DATE,TCAC.ScheduledOn) < CONVERT(DATE,GETDATE()) THEN TCAC.TC_LeadId END)) PendingFollowup,
				COUNT(DISTINCT (CASE WHEN CONVERT(DATE,TCAC.ScheduledOn) = CONVERT(DATE,GETDATE()+1) THEN TCAC.TC_LeadId END)) TomorrowFollowup,
				1

				FROM TC_InquiriesLead AS TCIL WITH(NOLOCK)
				JOIN Dealers AS D WITH(NOLOCK) ON TCIL.BranchId = D.ID AND IsDealerActive = 1
				JOIN TC_Users AS TCU WITH(NOLOCK) ON D.ID = TCU.BranchId AND TCU.GCMRegistrationId IS NOT NULL AND TCU.IsActive = 1
				JOIN TC_BuyerInquiries AS N WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId=N.TC_InquiriesLeadId
				LEFT JOIN TC_ActiveCalls AS TCAC WITH(NOLOCK) ON TCAC.TC_LeadId=TCIL.TC_LeadId
				WHERE TCIL.TC_LeadInquiryTypeId=1
				GROUP BY TCIL.TC_UserId
			UNION ALL
				SELECT 
				TCIL.TC_UserId,
				COUNT( DISTINCT (CASE WHEN CONVERT(DATE,N.CreatedOn)=CONVERT(DATE,GETDATE()) THEN  N.TC_SellerInquiriesId END)) AS CurrentDayInquiryCount ,
				COUNT( DISTINCT (CASE WHEN MONTH(N.CreatedOn)=MONTH(GETDATE()) AND YEAR(N.CreatedOn)=YEAR(GETDATE()) THEN  N.TC_SellerInquiriesId END )) AS CurrentMonthInquiryCount ,
				COUNT(DISTINCT (CASE WHEN N.PurchasedStatus=33 AND CONVERT(DATE,N.PurchasedDate)=CONVERT(DATE,GETDATE()) THEN   N.TC_SellerInquiriesId END )) AS CurrentDayBookingCount,
				COUNT(DISTINCT (CASE WHEN N.PurchasedStatus=33 AND MONTH(N.PurchasedDate)=MONTH(GETDATE()) AND YEAR(N.PurchasedDate)=YEAR(GETDATE())   THEN   N.TC_SellerInquiriesId END )) AS CurrentMonthBookingCount,
				COUNT(DISTINCT (CASE WHEN N.TC_LeadDispositionId<>1 AND N.TC_LeadDispositionId<>3 AND N.TC_LeadDispositionId<>4 AND N.TC_LeadDispositionId IS NOT NULL AND CONVERT(DATE,TCIL.CreatedDate)=CONVERT(DATE,GETDATE()) THEN  N.TC_SellerInquiriesId END)) CurrentDayLostCount,
				COUNT(DISTINCT (CASE WHEN N.TC_LeadDispositionId<>1 AND N.TC_LeadDispositionId<>3 AND N.TC_LeadDispositionId<>4 AND N.TC_LeadDispositionId IS NOT NULL AND MONTH(TCIL.CreatedDate)=MONTH(GETDATE()) AND YEAR(TCIL.CreatedDate)=YEAR(GETDATE()) THEN  N.TC_SellerInquiriesId END)) CurrentMonthLostCount,
				COUNT(DISTINCT (CASE WHEN CONVERT(DATE,TCAC.ScheduledOn) < CONVERT(DATE,GETDATE()) THEN TCAC.TC_LeadId END)) PendingFollowup,
				COUNT(DISTINCT (CASE WHEN CONVERT(DATE,TCAC.ScheduledOn) = CONVERT(DATE,GETDATE()+1) THEN TCAC.TC_LeadId END)) TomorrowFollowup,
				2

				FROM TC_InquiriesLead AS TCIL WITH(NOLOCK)
				JOIN Dealers AS D WITH(NOLOCK) ON TCIL.BranchId = D.ID AND IsDealerActive = 1
				JOIN TC_Users AS TCU WITH(NOLOCK) ON D.ID = TCU.BranchId AND TCU.GCMRegistrationId IS NOT NULL AND TCU.IsActive = 1
				JOIN TC_SellerInquiries AS N WITH(NOLOCK) ON TCIL.TC_InquiriesLeadId=N.TC_InquiriesLeadId
				LEFT JOIN TC_ActiveCalls AS TCAC WITH(NOLOCK) ON TCAC.TC_LeadId=TCIL.TC_LeadId
				WHERE TCIL.TC_LeadInquiryTypeId=2
				GROUP BY TCIL.TC_UserId		
				)
END
