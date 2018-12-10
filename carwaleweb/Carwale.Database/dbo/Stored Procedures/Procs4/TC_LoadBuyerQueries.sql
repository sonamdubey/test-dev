IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_LoadBuyerQueries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_LoadBuyerQueries]
GO

	
-- =============================================
-- Author:		Vivek Gupta
-- Create date: 19-12-2014
-- Description:	Fetch BuyerQueries
-- =============================================
CREATE PROCEDURE [dbo].[TC_LoadBuyerQueries]
	@TC_BuyerInquiriesId BIGINT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    SELECT EntryDate, Comments 
	FROM TC_BuyerInquiryComments WITH(NOLOCK) 
	WHERE TC_BuyerInquiryId = @TC_BuyerInquiriesId
	ORDER BY EntryDate DESC
END

