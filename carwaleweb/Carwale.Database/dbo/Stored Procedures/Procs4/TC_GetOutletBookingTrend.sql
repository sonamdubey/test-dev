IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetOutletBookingTrend]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetOutletBookingTrend]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 22nd Feb 2016
-- Description:	To fetch the no of bookings across the dealership for past 6 months
-- [dbo].[TC_GetOutletBookingTrend] 9557
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetOutletBookingTrend]
	@MasterDealerId INT
AS
BEGIN
	SELECT DISTINCT DAM.DealerId OutletId,D.Organization Organization
	INTO #TempTblOutlet
	FROM TC_DealerAdmin DA WITH (NOLOCK)
	JOIN TC_DealerAdminMapping DAM WITH (NOLOCK) ON DA.Id = DAM.DealerAdminId
	JOIN Dealers D WITH (NOLOCK) ON D.ID = DAM.DealerId
	WHERE DA.IsActive = 1 AND D.IsDealerActive = 1 AND D.IsDealerDeleted = 0
	AND DA.DealerId = @MasterDealerId AND DAM.DealerId <> @MasterDealerId

	SELECT  T.OutletId,T.Organization Organization
	,COUNT(DISTINCT TCNI.TC_NewCarInquiriesId) BookedLeads
	,MONTH(TCNI.BookingDate) BookingMonth
	,DATEADD(month, DATEDIFF(month, 0, TCNI.BookingDate), 0) BookingDate
	FROM TC_InquiriesLead TCIL WITH (NOLOCK)
	 JOIN TC_NewCarInquiries TCNI WITH (NOLOCK) ON TCNI.TC_InquiriesLeadId = TCIL.TC_InquiriesLeadId
												   AND ISNULL(TCNI.TC_LeadDispositionId,0) IN(4,41)
												   AND ISNULL(TCNI.BookingStatus,0) = 32
												   AND DATEDIFF(MONTH,TCNI.BookingDate, GETDATE()) BETWEEN 1 ANd 6
	RIGHT OUTER JOIN #TempTblOutlet T WITH (NOLOCK)  ON T.OutletId = TCIL.BranchId
	GROUP BY T.OutletId,T.Organization,MONTH(TCNI.BookingDate),DATEADD(month, DATEDIFF(month, 0, TCNI.BookingDate), 0)
	ORDER BY DATEADD(month, DATEDIFF(month, 0, TCNI.BookingDate), 0) DESC

	DROP TABLE #TempTblOutlet
END

 -----------------------------------------------


 
