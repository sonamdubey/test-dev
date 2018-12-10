IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InactiveDealerResponseDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InactiveDealerResponseDetails]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 16/03/2012
-- Description:	Get Inactive Dealer Response Details
-- =============================================
CREATE PROCEDURE InactiveDealerResponseDetails
	-- Add the parameters for the stored procedure here
	 @fromdate datetime,@todate datetime
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
select d.Organization,count(distinct s.ID) as Listing,count(distinct u.id) as response,MAX(u.RequestDateTime) as LastReposnse
from SellInquiries as s with(nolock)
join Dealers as D with(nolock) on D.ID=s.DealerId and d.Status=1
join [UsedCarPurchaseInquiries] as u with(nolock) on u.SellInquiryId=s.ID
where s.EntryDate  between @fromdate and @todate 
group by d.Organization

END

