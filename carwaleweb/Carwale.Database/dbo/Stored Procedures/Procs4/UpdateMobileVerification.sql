IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateMobileVerification]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateMobileVerification]
GO

	-- =============================================
-- Author:		Raghu
-- Create date: 27-11-2013
-- Description:	Set MobileVerified in NewPurchaseCities table
-- =============================================
CREATE PROCEDURE [dbo].[UpdateMobileVerification]
	@PQId numeric(18,0)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    UPDATE NewPurchaseCities	
    SET MobileVerified = 1
    WHERE InquiryId=@PQId
END

