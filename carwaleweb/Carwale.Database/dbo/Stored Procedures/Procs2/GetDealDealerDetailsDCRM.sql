IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetDealDealerDetailsDCRM]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetDealDealerDetailsDCRM]
GO

	-- =============================================
-- Author:		Anchal Gupta
-- Create date: 11-01-2015
-- Description:	selecting the deals dealer details in tc_DEALS_DEALERS from dcrm
-- select * from TC_Deals_Dealers
-- =============================================
CREATE PROCEDURE [dbo].[GetDealDealerDetailsDCRM]
	-- Add the parameters for the stored procedure here
	@dealerId int
AS
BEGIN
	select ContactEmail, ContactMobile, IsDealerDealActive from TC_Deals_Dealers WITH (NOLOCK)
	where DealerId = @dealerId
END

