IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_GetNotificationForNewInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_GetNotificationForNewInquiry]
GO

	
-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date, 16th May, 2013>
-- Description:	<Description,Get single row of Mytask page which is newly added>
-- EXEC TC_GetNotificationForNewInquiry 2088, 1, 5, NULL
-- Modified By: Nilesh Utture on 24th May, 2013 Addded Unique Id field in SELECT Query
-- Modified By: Manish on 29-09-2015 added with (nolock) keyword wherever not found.
-- Modified By: Chetan Navin on 08th Aug, 2016(Added Column DeviceTokenIOS to be fetched)
-- =============================================
CREATE PROCEDURE [dbo].[TC_GetNotificationForNewInquiry]
	-- Add the parameters for the stored procedure here
	@InquiryId INT
	,@InquiryType INT
	,@BranchId INT
	,@SellInquiryId INT
AS
BEGIN
	DECLARE @InquiriesLeadId INT

	--IF @BranchId IS NULL
	--BEGIN
	--	SELECT @BranchId = DealerId
	--	FROM SellInquiries WITH (NOLOCK)
	--	WHERE ID = @SellInquiryId
	--END
	IF @InquiryType = 1
	BEGIN
		SELECT @InquiriesLeadId = TC_InquiriesLeadId
		FROM TC_BuyerInquiries WITH (NOLOCK)
		WHERE TC_BuyerInquiriesId = @InquiryId
	END

	IF @InquiryType = 2
	BEGIN
		SELECT @InquiriesLeadId = TC_InquiriesLeadId
		FROM TC_SellerInquiries WITH (NOLOCK)
		WHERE TC_SellerInquiriesId = @InquiryId
	END

	IF @InquiryType = 3
	BEGIN
		SELECT @InquiriesLeadId = TC_InquiriesLeadId
		FROM TC_NewCarInquiries WITH (NOLOCK)
		WHERE TC_NewCarInquiriesId = @InquiryId
	END

	SELECT C.id AS [CustomerId]
		,C.CustomerName AS [CustomerName]
		,C.Email
		,C.Mobile
		,C.TC_InquirySourceId
		,tcac.TC_LeadId
		,TC_InquiryStatusId
		,ScheduledOn AS [NextFollowUpDate]
		,TCIL.CarDetails AS [InterestedIn]
		,TCAC.CallType
		,TCAC.LastCallComment
		,LatestInquiryDate
		,TS.Source AS InquirySource
		,TCIL.TC_UserId AS UserId
		,TCIL.TC_LeadStageId
		,CASE TC_LeadInquiryTypeId
			WHEN 1
				THEN 'Used Buy'
			WHEN 2
				THEN 'Used Sell'
			WHEN 3
				THEN 'New Buy'
			END AS InquiryType
		,TU.UniqueId AS Abk -- Abk is Unique key for user -- Modified By: Nilesh Utture on 24th May, 2013
		,TU.GCMRegistrationId
		,TU.DeviceTokenIOS
	FROM TC_ActiveCalls AS TCAC WITH (NOLOCK)
	JOIN TC_CustomerDetails AS C WITH (NOLOCK) ON TCAC.TC_LeadId = C.ActiveLeadId
	JOIN TC_InquiriesLead AS TCIL WITH (NOLOCK) ON TCAC.TC_LeadId = TCIL.TC_LeadId
	JOIN TC_Users AS TU WITH (NOLOCK) ON TCIL.TC_UserId = TU.Id
	JOIN TC_InquirySource AS TS WITH (NOLOCK) ON C.TC_InquirySourceId = TS.Id
	WHERE TCIL.BranchId = @BranchId
		AND TCIL.TC_InquiriesLeadId = @InquiriesLeadId
END
