IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[CheckUnregisteredCarOwnerMismatch]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[CheckUnregisteredCarOwnerMismatch]
GO

	-- =============================================
-- Author:		Reshma Shetty
-- Create date: 26/12/2013
-- Description:	Check if the Unregistered cars are tagged as More than 4 owners in livelisting
-- =============================================
CREATE PROCEDURE [dbo].[CheckUnregisteredCarOwnerMismatch] 
	-- Add the parameters for the stored procedure here
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for procedure here
	SELECT DISTINCT ProfileId 
	FROM livelistings AS LL WITH(NOLOCK)
	JOIN SellInquiriesDetails AS SD WITH(NOLOCK) ON SD.SellInquiryId=LL.Inquiryid AND LL.SellerType=1
	WHERE SD.Owners='0'
	AND LL.Owners='More than 4 owners'
END
