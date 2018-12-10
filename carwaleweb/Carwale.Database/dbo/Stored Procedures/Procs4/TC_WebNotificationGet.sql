IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_WebNotificationGet]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_WebNotificationGet]
GO

	

-- =============================================
 

-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <11 th Dec 15>
-- Description:	<Returns Notification Cnt and data>
-- Modified By : Khushaboo Patil on 19/02/2016 return inspection notifications data
-- Modified By : Khushaboo Patil on 17/03/2016 return suspendedCarsCount
--EXEC TC_WebNotificationGet 135,1,1
-- =============================================
CREATE PROCEDURE [dbo].[TC_WebNotificationGet]
@TC_UserId	INT,
@TopNData	INT = 0,
@Type		BIT = 0 , -- 1 for data and 0 for count
@ApplicationId SMALLINT = 1

AS
BEGIN
	DECLARE @TC_NotificationsCnt INT
	DECLARE @tblNotifications AS TABLE(TC_NotificationsId int, RecordType smallint,CustomerName VARCHAR(100),LeadId INT,LeadOwnerId INT,CustomerId INT, Mobile VARCHAR(15)
									 ,InterestedIn VARCHAR(MAX),ExchangeCar VARCHAR(500),ExchangeCarMakeYear DATE)
	-- NOTIFICATION COUNT
	SELECT @TC_NotificationsCnt = COUNT(TC_NotificationsId) 
	FROM TC_Notifications TN WITH(NOLOCK) 
	WHERE TC_UserID=@TC_UserId

	SELECT @TC_NotificationsCnt AS NotificationCount 

	-- DETAILS OF NOTIFICATIONS
	IF @Type = 1 AND @TC_NotificationsCnt > 0
		BEGIN
			DECLARE @FromIndex  INT
			DECLARE @ToIndex	INT
			DECLARE @DataCntPerRequest	SMALLINT = 10
			SET @FromIndex = ( @TopNData * @DataCntPerRequest ) + 1
			SET @ToIndex = @FromIndex + @DataCntPerRequest

			SELECT DISTINCT TC_NotificationsId , RecordType ,CD.Mobile, IL.CarDetails AS InterestedIn,
			ISNULL(VW1.Make,'') + ' ' +ISNULL(VW1.Model,'')+ ' '+ ISNULL(VW1.Version,'') AS ExchangeCar,ISNULL(CD.CustomerName,'')+ ' ' + ISNULL(CD.LastName,'') AS CustomerName,
			CD.Id AS CustomerId , TL.TC_LeadId, IL.TC_UserId AS LeadOwnerId,TN.NotificationDateTime ,IL.LatestInquiryDate,EN.MakeYear AS ExchangeCarMakeYear,
			ISNULL(CR.Make,'') + ' ' +ISNULL(CR.Model,'')+ ' '+ ISNULL(CR.Variant,'') AS StockMakeModelVariant,CC.ListingId AS StockId,CR.RegistrationNo,convert(varchar, CCD.InvCertifiedDate , 106)AS InspectionDate
			, (SELECT DISTINCT COUNT(ST.ID) FROM TC_STOCK ST WITH(NOLOCK) WHERE ST.BranchId = U.BranchId AND StatusId = 4 AND CONVERT(DATE,SuspendedDate) = CONVERT(DATE,TN.NotificationDateTime)) SuspendedCarsCount
			INTO #TmpNewLeadtbl
			FROM TC_Notifications TN WITH(NOLOCK) 
			LEFT JOIN TC_Calls AC WITH(NOLOCK) ON AC.TC_CallsId = TN.RecordId AND ( RecordType = 1 OR RecordType = 2 )
			LEFT JOIN TC_Lead TL WITH(NOLOCK) ON TL.TC_LeadId = AC.TC_LeadId AND ( RecordType = 1 OR RecordType = 2 )
			LEFT JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_LeadId = TL.TC_LeadId AND ( RecordType = 1 OR RecordType = 2 )
			LEFT JOIN TC_CustomerDetails CD WITH(NOLOCK) ON CD.Id = TL.TC_CustomerId AND ( RecordType = 1 OR RecordType = 2 )
			LEFT JOIN TC_NewCarInquiries NI WITH(NOLOCK) ON NI.TC_InquiriesLeadId = IL.TC_InquiriesLeadId AND ( RecordType = 1 OR RecordType = 2 )
			LEFT JOIN TC_ExchangeNewCar EN WITH(NOLOCK) ON EN.TC_NewCarInquiriesId = NI.TC_NewCarInquiriesId AND ( RecordType = 1 OR RecordType = 2 )  
			LEFT JOIN vwAllMMV VW1 WITH(NOLOCK) ON VW1.VersionId = EN.CarVersionId AND VW1.ApplicationId = @ApplicationId AND ( RecordType = 1 OR RecordType = 2 )
			LEFT JOIN TC_CarTradeCertificationLiveListing CC WITH(NOLOCK) ON CC.TC_CarTradeCertificationLiveId = TN.RecordId AND ( RecordType = 3 )
			LEFT JOIN TC_CarTradeCertificationRequests CR WITH(NOLOCK) ON CR.ListingId = CC.ListingId AND ( RecordType = 3 )
			LEFT JOIN TC_CarTradeCertificationData CCD WITH(NOLOCK) ON CCD.ListingId = CR.ListingId AND ( RecordType = 3 )
			LEFT JOIN TC_Users U WITH(NOLOCK) ON U.Id = TN.TC_UserId
			WHERE TN.TC_UserId = @TC_UserId 
			ORDER BY TN.NotificationDateTime DESC

			-- GET 5 DATA AS PER REQUEST
			SELECT DISTINCT TC_NotificationsId , RecordType ,CustomerName,LeadId,LeadOwnerId ,CustomerId, Mobile ,InterestedIn,ExchangeCar,ExchangeCarMakeYear,
			StockMakeModelVariant,StockId,RegistrationNo,InspectionDate,SuspendedCarsCount,NotificationDateTime,RowNo FROM
			(
				SELECT DISTINCT TC_NotificationsId , RecordType ,CustomerName,LeadId,LeadOwnerId ,CustomerId, Mobile ,InterestedIn,ExchangeCar ,ExchangeCarMakeYear,
				StockMakeModelVariant,StockId,RegistrationNo,InspectionDate,SuspendedCarsCount,NotificationDateTime,
				ROW_NUMBER() OVER(ORDER BY	TC_NotificationsId DESC) AS RowNo ,RNO FROM (
					SELECT TC_NotificationsId , RecordType ,CustomerName,TC_LeadId AS LeadId,LeadOwnerId ,CustomerId, Mobile ,InterestedIn,ExchangeCar , ExchangeCarMakeYear,
					StockMakeModelVariant,StockId,RegistrationNo,InspectionDate,SuspendedCarsCount,NotificationDateTime,
					ROW_NUMBER() OVER( PARTITION BY TC_NotificationsId ORDER BY  LatestInquiryDate  DESC) AS RNO
					FROM #TmpNewLeadtbl WITH(NOLOCK) )tab
				WHERE   RNO = 1 )TAB1 
			WHERE RowNo BETWEEN @FromIndex AND @ToIndex - 1 ORDER BY RowNo

			DROP TABLE #TmpNewLeadtbl
		END
END
