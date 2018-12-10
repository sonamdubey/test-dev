IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetSecondLastMonthLeadCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetSecondLastMonthLeadCount]
GO

	-- =============================================
--Author:		PRACHI PHALAK
-- Create date: 16th SEPT,2016
-- Description:	To get lead count for all dealers for last 2 month time period. 
-- =============================================
CREATE PROCEDURE [dbo].[GetSecondLastMonthLeadCount] 
	-- Add the parameters for the stored procedure here
	@DealerId AS INT,
	@SecondLastMonthVerifiedLeadCount INT OUTPUT,
	@SecondLastMonthUnverifiedLeadCount INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

        SELECT  @SecondLastMonthVerifiedLeadCount = COUNT(UCP.ID)
		FROM USEDCARPURCHASEINQUIRIES UCP WITH (NOLOCK)
		INNER JOIN SELLINQUIRIES SI WITH (NOLOCK)
		ON UCP.SELLINQUIRYID = SI.ID WHERE
		SI.DealerId  = @DealerId AND 
		UCP.REQUESTDATETIME BETWEEN convert(DATETIME, convert(VARCHAR(10), DATEADD(MM, DATEDIFF(MM, 0, GETDATE())-2, 0), 120) + ' 00:00:00')
		AND convert(DATETIME, convert(VARCHAR(10), DATEADD(MS, -3, DATEADD(MM, DATEDIFF(MM, 0, GETDATE())-1 , 0)), 120) + ' 23:59:59')

        SELECT @SecondLastMonthUnverifiedLeadCount = COUNT(CL.Id)
		FROM ClassifiedLeads CL WITH (NOLOCK) 
		INNER JOIN SellInquiries SI WITH (NOLOCK)
		ON CL.InquiryId = SI.ID 
		WHERE SI.DealerId  = @DealerId AND
		CL.IsVerified = 0 AND CL.IsSentToSource = 1 AND
		CL.EntryDateTime BETWEEN convert(DATETIME, convert(VARCHAR(10), DATEADD(MM, DATEDIFF(MM, 0, GETDATE())-2, 0), 120) + ' 00:00:00')
		AND convert(DATETIME, convert(VARCHAR(10), DATEADD(MS, -3, DATEADD(MM, DATEDIFF(MM, 0, GETDATE())-1 , 0)), 120) + ' 23:59:59')

END

