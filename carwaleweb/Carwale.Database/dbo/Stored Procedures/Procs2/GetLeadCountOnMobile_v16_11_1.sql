IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetLeadCountOnMobile_v16_11_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetLeadCountOnMobile_v16_11_1]
GO

	-- =============================================
-- Author:		Vinayak Mishra
-- Create date: 07/10/2016
-- Description:	To get lead count
-- Modified: Vicky Lund, 28-10-2016, SP rewrite for new logic
-- exec [dbo].[GetLeadCountForCurrentDay] 9998882224 (done)
-- =============================================
CREATE PROCEDURE [dbo].[GetLeadCountOnMobile_v16_11_1] 
	@Mobile VARCHAR(15)
AS
BEGIN
	SELECT COUNT(Id)
	FROM PQDealerAdLeads WITH (NOLOCK)
	WHERE mobile = @Mobile
END