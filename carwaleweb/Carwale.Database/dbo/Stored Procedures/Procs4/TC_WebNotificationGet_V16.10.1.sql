IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_WebNotificationGet_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_WebNotificationGet_V16]
GO

	
-- =============================================
-- =============================================
-- Author:		<Khushaboo Patil>
-- Create date: <11 th Dec 15>
-- Description:	<Returns Notification Cnt and data>
-- Modified By : Khushaboo Patil on 19/02/2016 return inspection notifications data
-- Modified By : Khushaboo Patil on 17/03/2016 return suspendedCarsCount
-- EXEC TC_WebNotificationGet 243,1,1
-- Modified By : Ashwini Dhamankar on Oct 10,2016 (added condition if cardetails are null and customer name for masking number lead) 
-- =============================================
CREATE PROCEDURE [dbo].[TC_WebNotificationGet_V16.10.1] @TC_UserId INT
	,@TopNData INT = 0
	,@Type BIT = 0
	,-- 1 for data and 0 for count
	@ApplicationId SMALLINT = 1
AS
BEGIN
	DECLARE @TC_NotificationsCnt INT

	-- NOTIFICATION COUNT
	SELECT @TC_NotificationsCnt = COUNT(TC_NotificationsId)
	FROM TC_Notifications TN WITH (NOLOCK)
	WHERE TC_UserID = @TC_UserId

	SELECT @TC_NotificationsCnt AS NotificationCount

	--modified by : Ashwini Dhamankar mon Oct 17,2016(created table)
	CREATE TABLE #TmpNewLeadtbl (
		TC_NotificationsId INT
		,RecordType SMALLINT
		,Mobile VARCHAR(15)
		,InterestedIn VARCHAR(500)
		,ExchangeCar VARCHAR(500)
		,CustomerName VARCHAR(100)
		,CustomerId INT
		,TC_LeadId BIGINT
		,LeadOwnerId BIGINT
		,NotificationDateTime DATETIME
		,LatestInquiryDate DATETIME
		,ExchangeCarMakeYear DATE
		,StockMakeModelVariant VARCHAR(500)
		,StockId INT
		,RegistrationNo VARCHAR(20)
		,InspectionDate VARCHAR(20)
		,SuspendedCarsCount INT
		)

	-- DETAILS OF NOTIFICATIONS
	IF @Type = 1
		AND @TC_NotificationsCnt > 0
	BEGIN
		DECLARE @FromIndex INT
		DECLARE @ToIndex INT
		DECLARE @DataCntPerRequest SMALLINT = 10

		SET @FromIndex = (@TopNData * @DataCntPerRequest) + 1
		SET @ToIndex = @FromIndex + @DataCntPerRequest

		INSERT INTO #TmpNewLeadtbl (
			TC_NotificationsId
			,RecordType
			,Mobile
			,InterestedIn
			,ExchangeCar
			,CustomerName
			,CustomerId
			,TC_LeadId
			,LeadOwnerId
			,NotificationDateTime
			,LatestInquiryDate
			,ExchangeCarMakeYear
			,StockMakeModelVariant
			,StockId
			,RegistrationNo
			,InspectionDate
			,SuspendedCarsCount
			)
		SELECT DISTINCT TC_NotificationsId
			,RecordType
			,CD.Mobile
			,ISNULL(IL.CarDetails, 'Car Not Specified') AS InterestedIn
			,-- Modified By : Ashwini Dhamankar on Oct 10,2016 
			ISNULL(VW1.Make, '') + ' ' + ISNULL(VW1.Model, '') + ' ' + ISNULL(VW1.Version, '') AS ExchangeCar
			,CASE 
				WHEN (
						lower(CD.CustomerName) = 'missed call'
						OR (
							lower(CD.CustomerName) = 'unknown'
							AND IL.InqSourceId = 6
							)
						)
					THEN 'Masking number lead'
				ELSE ISNULL(CD.CustomerName, '') + ' ' + ISNULL(CD.LastName, '')
				END AS CustomerName
			,-- Modified By : Ashwini Dhamankar on Oct 10,2016 
			CD.Id AS CustomerId
			,TL.TC_LeadId
			,IL.TC_UserId AS LeadOwnerId
			,TN.NotificationDateTime
			,IL.LatestInquiryDate
			,EN.MakeYear AS ExchangeCarMakeYear
			,ISNULL(CR.Make, '') + ' ' + ISNULL(CR.Model, '') + ' ' + ISNULL(CR.Variant, '') AS StockMakeModelVariant
			,CC.ListingId AS StockId
			,CR.RegistrationNo
			,convert(VARCHAR, CCD.InvCertifiedDate, 106) AS InspectionDate
			,(
				SELECT DISTINCT COUNT(ST.ID)
				FROM TC_STOCK ST WITH (NOLOCK)
				WHERE ST.BranchId = U.BranchId
					AND StatusId = 4
					AND CONVERT(DATE, SuspendedDate) = CONVERT(DATE, TN.NotificationDateTime)
				) SuspendedCarsCount
		--INTO #TmpNewLeadtbl
		FROM TC_Notifications TN WITH (NOLOCK)
		LEFT JOIN TC_Calls AC WITH (NOLOCK) ON AC.TC_CallsId = TN.RecordId
			AND (
				RecordType = 1
				OR RecordType = 2
				)
		LEFT JOIN TC_Lead TL WITH (NOLOCK) ON TL.TC_LeadId = AC.TC_LeadId
			AND (
				RecordType = 1
				OR RecordType = 2
				)
		LEFT JOIN TC_InquiriesLead IL WITH (NOLOCK) ON IL.TC_LeadId = TL.TC_LeadId
			AND (
				RecordType = 1
				OR RecordType = 2
				)
		LEFT JOIN TC_CustomerDetails CD WITH (NOLOCK) ON CD.Id = TL.TC_CustomerId
			AND (
				RecordType = 1
				OR RecordType = 2
				)
		LEFT JOIN TC_NewCarInquiries NI WITH (NOLOCK) ON NI.TC_InquiriesLeadId = IL.TC_InquiriesLeadId
			AND (
				RecordType = 1
				OR RecordType = 2
				)
		LEFT JOIN TC_ExchangeNewCar EN WITH (NOLOCK) ON EN.TC_NewCarInquiriesId = NI.TC_NewCarInquiriesId
			AND (
				RecordType = 1
				OR RecordType = 2
				)
		LEFT JOIN vwAllMMV VW1 WITH (NOLOCK) ON VW1.VersionId = EN.CarVersionId
			AND VW1.ApplicationId = @ApplicationId
			AND (
				RecordType = 1
				OR RecordType = 2
				)
		LEFT JOIN TC_CarTradeCertificationLiveListing CC WITH (NOLOCK) ON CC.TC_CarTradeCertificationLiveId = TN.RecordId
			AND (RecordType = 3)
		LEFT JOIN TC_CarTradeCertificationRequests CR WITH (NOLOCK) ON CR.ListingId = CC.ListingId
			AND (RecordType = 3)
		LEFT JOIN TC_CarTradeCertificationData CCD WITH (NOLOCK) ON CCD.ListingId = CR.ListingId
			AND (RecordType = 3)
		LEFT JOIN TC_Users U WITH (NOLOCK) ON U.Id = TN.TC_UserId
		WHERE TN.TC_UserId = @TC_UserId
		ORDER BY TN.NotificationDateTime DESC

		-- GET 5 DATA AS PER REQUEST
		SELECT DISTINCT TC_NotificationsId
			,RecordType
			,CustomerName
			,LeadId
			,LeadOwnerId
			,CustomerId
			,Mobile
			,InterestedIn
			,ExchangeCar
			,ExchangeCarMakeYear
			,StockMakeModelVariant
			,StockId
			,RegistrationNo
			,InspectionDate
			,SuspendedCarsCount
			,NotificationDateTime
			,RowNo
		FROM (
			SELECT DISTINCT TC_NotificationsId
				,RecordType
				,CustomerName
				,LeadId
				,LeadOwnerId
				,CustomerId
				,Mobile
				,InterestedIn
				,ExchangeCar
				,ExchangeCarMakeYear
				,StockMakeModelVariant
				,StockId
				,RegistrationNo
				,InspectionDate
				,SuspendedCarsCount
				,NotificationDateTime
				,ROW_NUMBER() OVER (
					ORDER BY TC_NotificationsId DESC
					) AS RowNo
				,RNO
			FROM (
				SELECT TC_NotificationsId
					,RecordType
					,CustomerName
					,TC_LeadId AS LeadId
					,LeadOwnerId
					,CustomerId
					,Mobile
					,InterestedIn
					,ExchangeCar
					,ExchangeCarMakeYear
					,StockMakeModelVariant
					,StockId
					,RegistrationNo
					,InspectionDate
					,SuspendedCarsCount
					,NotificationDateTime
					,ROW_NUMBER() OVER (
						PARTITION BY TC_NotificationsId ORDER BY LatestInquiryDate DESC
						) AS RNO
				FROM #TmpNewLeadtbl WITH (NOLOCK)
				) tab
			WHERE RNO = 1
			) TAB1
		WHERE RowNo BETWEEN @FromIndex
				AND @ToIndex - 1
		ORDER BY RowNo
	END

	DROP TABLE #TmpNewLeadtbl;
END
