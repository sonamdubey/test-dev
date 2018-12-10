IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetLeadCountOnCwCookie_v16_11_1]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetLeadCountOnCwCookie_v16_11_1]
GO

	
-- =============================================
-- Author:		Vinayak Mishra
-- Create date: 21/10/2016
-- Description:	To get lead count on cwcCookie val(IMP:check for index on this column)
-- exec [dbo].[GetLeadCountForCurrentDayOnCwCookie] '4j1begqibKn4KkdKEY1eOKGxz' --9998882224
-- Modified: Changed check for cwc to be checked for 5 days earlie it was 1 day--Vinayak
-- Modified: Vicky Lund, 28-10-2016, SP rewrite for new logic (done)
-- =============================================
CREATE PROCEDURE [dbo].[GetLeadCountOnCwCookie_v16_11_1] 
	@cwcCookieVal VARCHAR(100)
AS
BEGIN
	SELECT COUNT(Id)
	FROM PQDealerAdLeads WITH (NOLOCK)
	WHERE CWCookieValue = @cwcCookieVal
END

