IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetLeadCountForCurrentDay]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetLeadCountForCurrentDay]
GO

	-- =============================================
-- Author:		Vinayak Mishra
-- Create date: 20/10/2016
-- Description:	To get lead count
-- exec [dbo].[GetLeadCountForCurrentDay] 6686353830 --9998882224
-- =============================================
CREATE PROCEDURE [dbo].[GetLeadCountForCurrentDay]
	@Mobile VARCHAR(15)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	select 	count(Id) 
	from PQDealerAdLeads with(nolock) where RequestDateTime>GETDATE()-1
	and mobile = @Mobile 
END