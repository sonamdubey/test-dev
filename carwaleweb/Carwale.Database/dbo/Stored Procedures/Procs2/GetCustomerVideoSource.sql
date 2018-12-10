IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCustomerVideoSource]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCustomerVideoSource]
GO

	-- =============================================
-- Author:		Ashish G. Kamble
-- Create date: 18 Nov 2013
-- Description:	Proc to get the customer uploade youtube video source and package id.
-- =============================================
CREATE PROCEDURE GetCustomerVideoSource

	-- Add the parameters for the stored procedure here
	@CustomerId BIGINT,
	@InquiryId BIGINT,
	@VideoSrc VARCHAR(20) OUTPUT,
	@IsPremium BIT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
   
	SELECT @IsPremium = csi.IsPremium, @VideoSrc = csid.YoutubeVideo 
	FROM CustomerSellInquiries csi
	INNER JOIN CustomerSellInquiryDetails  AS csid ON csi.ID = csid.InquiryId
	WHERE csi.ID = @InquiryId AND csi.CustomerId = @CustomerId

END
