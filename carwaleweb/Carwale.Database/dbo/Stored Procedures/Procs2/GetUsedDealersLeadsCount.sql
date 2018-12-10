IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetUsedDealersLeadsCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetUsedDealersLeadsCount]
GO

	-- =============================================
-- Author:		PRACHI PHALAK
-- Create date: 13th SEPT,2016
-- Description:	To get lead count for all dealers for current time period. 
-- =============================================
CREATE PROCEDURE [dbo].[GetUsedDealersLeadsCount]
	-- Add the parameters for the stored procedure here
	@DealerId AS INT,
	@CurrentMonthVerifiedLeadCount INT OUTPUT,
	@CurrentMonthUnverifiedLeadCount INT OUTPUT
	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
        SELECT  @CurrentMonthVerifiedLeadCount = COUNT(UCP.ID)
		FROM USEDCARPURCHASEINQUIRIES UCP  WITH (NOLOCK)
		INNER JOIN SELLINQUIRIES SI WITH (NOLOCK)
		ON UCP.SELLINQUIRYID = SI.ID WHERE
		SI.DealerId  = @DealerId AND 
		UCP.REQUESTDATETIME BETWEEN  convert(DATETIME, convert(VARCHAR(10), DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0), 120) + ' 00:00:00') AND GETDATE()
		
		SELECT @CurrentMonthUnverifiedLeadCount = COUNT(CL.Id)
		FROM ClassifiedLeads CL WITH (NOLOCK) 
		INNER JOIN SellInquiries SI WITH (NOLOCK)
		ON CL.InquiryId = SI.ID 
		WHERE SI.DealerId  = @DealerId AND
		CL.IsVerified = 0 AND CL.IsSentToSource = 1 AND 
		CL.EntryDateTime BETWEEN  convert(DATETIME, convert(VARCHAR(10), DATEADD(month, DATEDIFF(month, 0, GETDATE()), 0), 120) + ' 00:00:00') AND GETDATE()
		
END
