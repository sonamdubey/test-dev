IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CH_MailerTrackGetRecord]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CH_MailerTrackGetRecord]
GO

	CREATE PROCEDURE [dbo].[CH_MailerTrackGetRecord]
@From DATETIME,
@To DATETIME
AS
BEGIN
	SELECT COUNT(CUMT.Id) MailSentCount, SUM( CASE WHEN IsClicked = 1 THEN 1 ELSE 0 END) ClickedCount,
		CONVERT(CHAR, MailDate,110) AS MailDate,
		SUM( CASE WHEN CPR.isActive=1 AND CPR.isApproved=1 THEN 1 ELSE 0 END) TotalPaid,
		SUM( CASE WHEN CPR.isActive=1 AND CPR.isApproved=1 AND CUMT.IsClicked = 1 THEN 1 ELSE 0 END) MailPaid
	FROM CH_UnpaidMailerTrack CUMT WITH (NOLOCK)
		JOIN CustomerSellInquiries CSI WITH (NOLOCK) ON CUMT.InquiryId = CSI.ID
		LEFT JOIN ConsumerPackageRequests CPR WITH(NOLOCK) ON CUMT.InquiryId = CPR.ItemId AND CUMT.CustomerId=CPR.ConsumerId 
	WHERE CUMT.MailDate BETWEEN @From AND @To
	GROUP BY CONVERT(CHAR, MailDate,110) 
	ORDER BY MailDate
	
END