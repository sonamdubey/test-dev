IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[IndividualCarListingDetails]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[IndividualCarListingDetails]
GO

	-- =============================================
-- Author:		Avishkar Meshram
-- Create date: Oct 08 2011
-- Description:	Individual Car Listing Details
-- =============================================
CREATE PROCEDURE  [reports].[IndividualCarListingDetails]
	@Month int,@Year int
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	
    -- Insert statements for procedure here
    
   select Seller,SellInquiryId,ListingTime,ScheduleCallTime,FirstCRMCallTime,PaymentTime,
        (DATEDIFF(SECOND,ListingTime,ScheduleCallTime))/60 as DelayInScheduleCall,(DATEDIFF(SECOND,ListingTime,FirstCRMCallTime))/60 as CallAge ,		
		(DATEDIFF(SECOND,ListingTime,PaymentTime))/60 as PaymentAge,
		case sign(DATEDIFF(SECOND,FirstCRMCallTime,PaymentTime)) when -1 then 'PaymentFirst' else 'CallFirst' end as PaymentPattern,
		PaymentMode,AgentName
	from 
	(select C.Name as Seller, cs.ID as SellInquiryId,min(cs.EntryDate) as ListingTime,
	min(cl.ScheduledDateTime) as ScheduleCallTime,
	min(cl.CalledDateTime) as FirstCRMCallTime,
	CPR.EntryDate as PaymentTime,P.ModeName as PaymentMode,
	O.UserName as AgentName
	from CustomerSellInquiries as cs
	join Customers as C on cs.CustomerId=C.Id
	join CH_Calls as cc on cs.ID=cc.EventId
	join CH_Logs as cl on cc.ID=cl.CallId
	join ConsumerPackageRequests as CPR on CPR.ConsumerId=cs.CustomerId and cs.ID=CPR.ItemId
	join PaymentModes as P ON P.Id=CPR.PaymentModeId
	join OprUsers as O on O.Id=cl.TCID
	where datepart(month,cs.EntryDate)=@Month
	and datepart(year,cs.EntryDate)=@Year
	and cc.tbctype = 2 and cc.calltype=1
	and cs.PackageType=2
	and CPR.isActive=1
	and CPR.isApproved=1
	group by C.Name,cs.ID,CPR.EntryDate,P.ModeName,O.UserName
	)a
	join CustomerSellInquiries as cs on CS.Id=SellInquiryId
	join vwMMV as vw on vw.VersionId=cs.CarVersionId
	join Cities as CT on CT.ID=cs.CityId
	order by PaymentTime desc

END

