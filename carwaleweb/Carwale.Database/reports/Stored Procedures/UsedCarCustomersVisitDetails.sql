IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[UsedCarCustomersVisitDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[UsedCarCustomersVisitDetails]
GO

	-- =============================================
-- Author:		Avishkar Meshram
-- Create date: 10 Feb 2012
-- Description:	Used Car Customers
-- =============================================
CREATE PROCEDURE [reports].[UsedCarCustomersVisitDetails]
	@fromDate datetime, @ToDate datetime
AS
BEGIN

with CTE
as
(select ROW_NUMBER() over(partition by Mobile order by RequestDateTime desc  ) as rownum,
Name,Mobile,RequestDateTime,Make,Model,Version,MakeYear,Price,Kilometers,Type
from (
select C.Name, C.Mobile,CR.RequestDateTime,vw.Make,vw.Model,vw.Version,Year(CSI.MakeYear) as MakeYear,CSI.Price,CSI.Kilometers,'Individual' as Type
from Customers as C
join [dbo].[ClassifiedRequests] as CR on C.Id=CR.CustomerId
join CustomerSellInquiries as CSI on CSI.ID=CR.SellInquiryId
join vwMMV as vw on vw.VersionId=CSI.CarVersionId
where C.RegistrationDateTime between @fromDate and @ToDate
and CR.RequestDateTime = ( select max(RequestDateTime) from ClassifiedRequests as CR2 where CR.Id=CR2.Id)
and C.CityId in (1,13,40)

union all

select C.Name, C.Mobile,CR.RequestDateTime,vw.Make,vw.Model,vw.Version,Year(CSI.MakeYear) as MakeYear,CSI.Price,CSI.Kilometers,'Dealer' as Type
from Customers as C
join UsedCarPurchaseInquiries as CR on C.Id=CR.CustomerId
join SellInquiries as CSI on CSI.ID=CR.SellInquiryId
join vwMMV as vw on vw.VersionId=CSI.CarVersionId
where C.RegistrationDateTime between  @fromDate and @ToDate
and CR.RequestDateTime = ( select max(RequestDateTime) from UsedCarPurchaseInquiries as CR2 where CR.Id=CR2.Id)
and C.CityId in (1,13,40)
) a
)
select Name,Mobile,RequestDateTime,Make,Model,Version,MakeYear,Price,Kilometers,Type 
from CTE
where rownum=1						
								

END

