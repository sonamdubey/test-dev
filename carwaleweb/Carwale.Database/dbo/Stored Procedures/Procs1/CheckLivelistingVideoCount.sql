IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckLivelistingVideoCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckLivelistingVideoCount]
GO

	-- =============================================
-- Author:		Reshma Shetty
-- Create date: 30/12/2013
-- Description:	Returns the count of dealer and individual profiles which have video count as 0 in Livelistings table even when it has approved video.
-- =============================================
CREATE PROCEDURE [dbo].[CheckLivelistingVideoCount]
	-- Add the parameters for the stored procedure here
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
    
    -- Insert statements for procedure here
    
    SELECT SellerType,InquiryCount
    FROM(
	SELECT 'Dealer' SellerType,COUNT(LL.Inquiryid) AS InquiryCount
	FROM LiveListings LL WITH (NOLOCK)
	INNER JOIN SellInquiriesDetails SID  WITH (NOLOCK) ON SID.SellInquiryId=LL.Inquiryid
	WHERE SellerType=1 and IsPremium=1
	--and YoutubeVideo is not null
	and VideoCount<>IsYouTubeVideoApproved
	UNION 
	SELECT 'Individual' SellerType,COUNT(LL.Inquiryid) AS InquiryCount
	FROM LiveListings LL  WITH (NOLOCK)
	INNER JOIN CustomerSellInquiryDetails CSD WITH (NOLOCK)  ON CSD.InquiryId=LL.Inquiryid
	WHERE SellerType=2 and IsPremium=1
	--and YoutubeVideo is not null
	and VideoCount<>IsYouTubeVideoApproved
	)A
	WHERE InquiryCount>0
	
	
END
