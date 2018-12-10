IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_WebNotificationAll]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_WebNotificationAll]
GO

	-- =============================================
-- Author:		<Afrose>
-- Create date: <18th Dec 15>
-- Description:	<Get all notifications(read and unread)>
-- Modified By : Khushaboo Patil on 19/02/2016 return inspection notifications data
-- Modified By : Khushaboo Patil on 17/03/2016 return suspendedCarsCount
-- EXEC TC_WebNotificationAll 243, 1,1
-- EXEC TC_WebNotificationAll 88638, 1,1
-- Modified by Manish on 29-09-2016 added statement for create index on temp table.
-- =============================================
CREATE PROCEDURE [dbo].[TC_WebNotificationAll]	
	@TC_UserId INT,
	@ApplicationId INT,
	@PageNumber TINYINT=1
AS
BEGIN	
		
		SELECT distinct Id, RecordType,CustomerName,InterestedIn,LeadId,LeadOwnerId,CustomerId AS CustomerId, Mobile ,ExchangeCar,
		ExchangeCarMakeYear,ReadDateTime, StockMakeModelVariant,StockId,RegistrationNo,InspectionDate,SuspendedCarsCount,ROW_NUMBER() OVER(ORDER BY ReadDateTime desc) AS ROWNUMBER INTO #TableForPaging FROM(
			SELECT distinct
			TC_NotificationsId AS Id, RecordType ,ISNULL(CD.CustomerName,'')+ ' ' + ISNULL(CD.LastName,'') AS CustomerName,TL.TC_LeadId AS LeadId,IL.TC_UserId AS LeadOwnerId ,
			CD.Id AS CustomerId, CD.Mobile ,IL.CarDetails AS InterestedIn,ISNULL(VW1.Make,'') + ' ' +ISNULL(VW1.Model,'')+ ' '+ ISNULL(VW1.Version,'') AS  ExchangeCar,EN.MakeYear AS ExchangeCarMakeYear,
			NULL AS ReadDateTime,ISNULL(CR.Make,'') + ' ' +ISNULL(CR.Model,'')+ ' '+ ISNULL(CR.Variant,'') AS StockMakeModelVariant,CC.ListingId AS StockId,CR.RegistrationNo,convert(varchar, CCD.InvCertifiedDate , 106) AS InspectionDate --  Row_Number() OVER (ORDER BY (SELECT 1)) AS ROWNUMBER
			, (SELECT COUNT(ST.ID) FROM TC_STOCK ST WITH(NOLOCK) WHERE ST.BranchId = U.BranchId AND StatusId = 4 AND CONVERT(DATE,SuspendedDate) = CONVERT(DATE,TN.NotificationDateTime))AS SuspendedCarsCount
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
			
	UNION ALL

			SELECT distinct
			TC_NotificationsLogId AS Id , TNL.RecordType AS RecordType ,ISNULL(CD.CustomerName,'')+ ' ' + ISNULL(CD.LastName,'') AS CustomerName,TL.TC_LeadId AS LeadId,IL.TC_UserId AS LeadOwnerId ,
			CD.Id AS CustomerId, CD.Mobile ,IL.CarDetails AS InterestedIn,ISNULL(VW1.Make,'') + ' ' +ISNULL(VW1.Model,'')+ ' '+ ISNULL(VW1.Version,'') AS ExchangeCar,EN.MakeYear AS ExchangeCarMakeYear,	TNL.ReadDateTime AS ReadDateTime ,--ROW_NUMBER() OVER(ORDER BY TNL.ReadDateTime desc) AS ROWNUMBER
			ISNULL(CR.Make,'') + ' ' +ISNULL(CR.Model,'')+ ' '+ ISNULL(CR.Variant,'') AS StockMakeModelVariant,CC.ListingId AS StockId,CR.RegistrationNo,convert(varchar, CCD.InvCertifiedDate , 106) AS InspectionDate
	        , (SELECT COUNT(ST.ID) FROM TC_STOCK ST WITH(NOLOCK) WHERE ST.BranchId = U.BranchId AND StatusId = 4 AND CONVERT(DATE,SuspendedDate) = CONVERT(DATE,TNL.NotificationDateTime)) AS SuspendedCarsCount
			FROM TC_NotificationsLog TNL WITH(NOLOCK)
	
			LEFT JOIN TC_Calls AC WITH(NOLOCK) ON AC.TC_CallsId = TNl.RecordId AND ( RecordType = 1 OR RecordType = 2 )
			LEFT JOIN TC_Lead TL WITH(NOLOCK) ON TL.TC_LeadId = AC.TC_LeadId AND ( RecordType = 1 OR RecordType = 2 )
			LEFT JOIN TC_InquiriesLead IL WITH(NOLOCK) ON IL.TC_LeadId = TL.TC_LeadId AND ( RecordType = 1 OR RecordType = 2 )
			LEFT JOIN TC_CustomerDetails CD WITH(NOLOCK) ON CD.Id = TL.TC_CustomerId AND ( RecordType = 1 OR RecordType = 2 )
			LEFT JOIN TC_NewCarInquiries NI WITH(NOLOCK) ON NI.TC_InquiriesLeadId = IL.TC_InquiriesLeadId AND ( RecordType = 1 OR RecordType = 2 )
			LEFT JOIN TC_ExchangeNewCar EN WITH(NOLOCK) ON EN.TC_NewCarInquiriesId = NI.TC_NewCarInquiriesId AND ( RecordType = 1 OR RecordType = 2 )  
			LEFT JOIN vwAllMMV VW1 WITH(NOLOCK) ON VW1.VersionId = EN.CarVersionId AND VW1.ApplicationId = @ApplicationId AND ( RecordType = 1 OR RecordType = 2 )
			LEFT JOIN TC_CarTradeCertificationLiveListing CC WITH(NOLOCK) ON CC.TC_CarTradeCertificationLiveId = TNl.RecordId AND ( RecordType = 3 )
			LEFT JOIN TC_CarTradeCertificationRequests CR WITH(NOLOCK) ON CR.ListingId = CC.ListingId AND ( RecordType = 3 )
			LEFT JOIN TC_CarTradeCertificationData CCD WITH(NOLOCK) ON CCD.ListingId = CR.ListingId AND ( RecordType = 3 )
			LEFT JOIN TC_Users U WITH(NOLOCK) ON U.Id = TNL.TC_UserId
			WHERE TNl.TC_UserId = @TC_UserId
			) AS TableForPaging
			

			-- Modified by Manish on 29-09-2016 added statement for create index on temp table.
			create index ix_TableForPaging on #TableForPaging(ReadDateTime);


	DECLARE @FirstRow INT, @LastRow INT ,@PageSize INT--To Calculate paging parameters
	SET @PageSize = 10
	SET @FirstRow = ((@PageNumber - 1) * @PageSize) + 1
    SET @LastRow    = @FirstRow + @PageSize -1
			   
	
	SELECT  TBLPG.Id AS Id, TBLPG.RecordType AS RecordType,TBLPG.CustomerName AS CustomerName,TBLPG.InterestedIn AS InterestedIn,TBLPG.LeadId AS LeadId,TBLPG.LeadOwnerId AS LeadOwnerId,
			TBLPG.CustomerId AS CustomerId, TBLPG.Mobile AS Mobile ,TBLPG.ExchangeCar AS ExchangeCar,
		TBLPG.ExchangeCarMakeYear,TBLPG.ReadDateTime AS ReadDateTime, StockMakeModelVariant,StockId,RegistrationNo,InspectionDate,SuspendedCarsCount--, TBLPG.ROWNUMBER 
	FROM 
	#TableForPaging TBLPG WITH(NOLOCK) WHERE TBLPG.ROWNUMBER BETWEEN @FirstRow AND @LastRow  
	ORDER BY TBLPG.ReadDateTime desc
 	
	
	SELECT COUNT(*) FROM #TableForPaging TBLPG

	DROP TABLE #TableForPaging
END

