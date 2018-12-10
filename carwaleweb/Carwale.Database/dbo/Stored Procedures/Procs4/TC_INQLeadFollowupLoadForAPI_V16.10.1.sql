IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_INQLeadFollowupLoadForAPI_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_INQLeadFollowupLoadForAPI_V16]
GO

	-- =============================================
-- Author:		<Author,Nilesh Utture>
-- Create date: <Create Date,30th April, 2013>
-- Description:	<Description,Loads Lead followup page for API>
-- Modifeid By Vivek Gupta on 14-01-2015, added @ApplicationId
-- Modifed By : Ashwini Dhamankar on Oct 3,2016 (Call versioned SP [TC_NewInquiryDetailsForAPI_V16.10.1])
-- =============================================
CREATE PROCEDURE [dbo].[TC_INQLeadFollowupLoadForAPI_V16.10.1]
	-- Add the parameters for the stored procedure here
	@LeadId INT,
	@BranchId INT,
	@UserId INT,
	@ApplicationId TINYINT = 1
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	EXEC TC_BuyerOnStockDetailsForAPI @LeadId,@BranchId,@UserId
	EXEC TC_BuyerInquiriesDetailsForAPI @LeadId,@BranchId,@UserId
	EXEC TC_SellerInquiryDetailsForAPI @LeadId,@BranchId,@UserId
	EXEC [TC_NewInquiryDetailsForAPI_V16.10.1] @LeadId,@BranchId,@UserId,@ApplicationId
    EXEC TC_INQActivityFeedLoad @LeadId
END

