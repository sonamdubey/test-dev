IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetLastMonthsLeadCount]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetLastMonthsLeadCount]
GO

	-- =============================================
--Author:		PRACHI PHALAK
-- Create date: 15th SEPT,2016
-- Description:	To get lead count for all dealers for last 1 month time period. 
-- =============================================
CREATE PROCEDURE [dbo].[GetLastMonthsLeadCount] 
	-- Add the parameters for the stored procedure here
	@DealerId AS INT,
	@LastMonthVerifiedLeadCount INT OUTPUT,
	@LastMonthUnverifiedLeadCount INT OUTPUT
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

	    SELECT  @LastMonthVerifiedLeadCount = COUNT(UCP.ID)
		FROM USEDCARPURCHASEINQUIRIES UCP WITH (NOLOCK) 
		INNER JOIN SELLINQUIRIES SI  WITH (NOLOCK)
		ON  UCP.SELLINQUIRYID = SI.ID WHERE
		SI.DealerId  = @DealerId AND 
		UCP.REQUESTDATETIME  BETWEEN convert(DATETIME, convert(VARCHAR(10), DATEADD(MM, DATEDIFF(MM, 0, GETDATE())-1, 0), 120) + ' 00:00:00')
		AND convert(DATETIME, convert(VARCHAR(10), DATEADD(MS, -3, DATEADD(MM, DATEDIFF(MM, 0, GETDATE()) , 0)), 120) + ' 23:59:59')
		
		SELECT @LastMonthUnverifiedLeadCount = COUNT(CL.Id)
		FROM ClassifiedLeads CL WITH (NOLOCK)
		INNER JOIN SellInquiries SI WITH (NOLOCK)
		ON CL.InquiryId = SI.ID 
		WHERE SI.DealerId  = @DealerId AND
		CL.IsVerified = 0 AND CL.IsSentToSource = 1 AND
		CL.EntryDateTime BETWEEN convert(DATETIME, convert(VARCHAR(10), DATEADD(MM, DATEDIFF(MM, 0, GETDATE())-1, 0), 120) + ' 00:00:00')
		AND convert(DATETIME, convert(VARCHAR(10), DATEADD(MS, -3, DATEADD(MM, DATEDIFF(MM, 0, GETDATE()) , 0)), 120) + ' 23:59:59')
END





