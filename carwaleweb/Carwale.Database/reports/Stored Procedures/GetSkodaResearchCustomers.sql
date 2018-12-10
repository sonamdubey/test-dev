IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[reports].[GetSkodaResearchCustomers]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [reports].[GetSkodaResearchCustomers]
GO

	-- =============================================
-- Author:		Avishkar Meshram
-- Create date: Ocy 09 2011
-- Description:	Get Skoda Research Customers
-- [reports].[GetSkodaResearchCustomers] '2011-10-01'
-- =============================================
CREATE PROCEDURE [reports].[GetSkodaResearchCustomers]
@date date
AS
BEGIN


	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	        Declare @CustomersinCRM int
	        Declare @VerifiedCustomers int
	        Declare @InLoopCustomers int
	        Declare @DealerAssignedCustomers int
	        Declare @CarBooked int
	        



			--select count(distinct CBD.PQId) as [Skoda  Car PQ Count]  -- 103948
			--from  CRM_CarBasicData as CBD with(nolock)
			--join dbo.vwMMV as vw with(nolock) on vw.VersionId=CBD.VersionId
			--where vw.MakeId=15 -- skoda
			--and CBD.CreatedOn>@date

			select @CustomersinCRM=count(distinct N.CustomerId) --as [Customers in CRM]  -- 8676
			from NewCarPurchaseInquiries as N with(nolock)
			where N.Id in 
			(select distinct CBD.PQId
			from  CRM_CarBasicData as CBD with(nolock)
			join dbo.vwMMV as vw with(nolock) on vw.VersionId=CBD.VersionId
			where vw.MakeId=15 -- skoda
			and CBD.CreatedOn>@date)
			and N.BuyTime='More than 2 months'

			select @VerifiedCustomers=count(distinct N.CustomerId) --as [Verified Customers]  -- 385
			from NewCarPurchaseInquiries as N with(nolock)
			join CRM_Customers as C on N.CustomerId=C.CWCustId
			join CRM_Leads AS CL with(nolock) on CL.CNS_CustId=C.Id
			--join CRM_Leads AS CL with(nolock) on CL.CNS_CustId=N.CustomerId
			where N.Id in 
			(select distinct CBD.PQId
			from  CRM_CarBasicData as CBD with(nolock)
			join dbo.vwMMV as vw with(nolock) on vw.VersionId=CBD.VersionId
			where vw.MakeId=15 -- skoda
			and CBD.CreatedOn>@date)
			and N.BuyTime='More than 2 months'
			and CL.LeadStatusId =2

			select @InLoopCustomers=count(distinct N.CustomerId)-- as [In Loop Customers]  -- 385  In Loop
			from NewCarPurchaseInquiries as N with(nolock)
			join CRM_Customers as C on N.CustomerId=C.CWCustId
			join CRM_Leads AS CL with(nolock) on CL.CNS_CustId=C.Id
			where N.Id in 
			(select distinct CBD.PQId
			from  CRM_CarBasicData as CBD with(nolock)
			join dbo.vwMMV as vw with(nolock) on vw.VersionId=CBD.VersionId
			where vw.MakeId=15 -- skoda
			and CBD.CreatedOn>@date)
			and N.BuyTime='More than 2 months'
			--and CL.LeadStatusId =2
			and CL.Owner=-1 and  LeadStageId=1

			select @DealerAssignedCustomers=count(distinct N.CustomerId) --as [dealer assigned Count] -- 221
			from NewCarPurchaseInquiries as N with(nolock)
			join CRM_Customers as C on N.CustomerId=C.CWCustId
			join CRM_Leads AS CL with(nolock) on CL.CNS_CustId=C.Id
			join CRM_CarBasicData as B with(nolock) on B.LeadId=CL.Id
			join CRM_CarDealerAssignment as CDA with(nolock) on CDA.CBDId=B.Id
			--join CRM_CarBookingData as CB with(nolock) on B.ID=CB.CarBasicDataId and CB.BookingStatusId=16 
			where N.Id in 
			(select distinct CBD.PQId
			from  CRM_CarBasicData as CBD with(nolock)
			join dbo.vwMMV as vw with(nolock) on vw.VersionId=CBD.VersionId
			where vw.MakeId=15 -- skoda
			and CBD.CreatedOn>@date)
			and N.BuyTime='More than 2 months'

			select @CarBooked=count(distinct N.CustomerId) -- [Booked Count]-- 18
			from NewCarPurchaseInquiries as N with(nolock)
			join CRM_Customers as C on N.CustomerId=C.CWCustId
			join CRM_Leads AS CL with(nolock) on CL.CNS_CustId=C.Id
			join CRM_CarBasicData as B with(nolock) on B.LeadId=CL.Id
			join CRM_CarDealerAssignment as CDA with(nolock) on CDA.CBDId=B.Id
			join CRM_CarBookingData as CB with(nolock) on B.ID=CB.CarBasicDataId and CB.BookingStatusId=16 
			where N.Id in 
			(select distinct CBD.PQId
			from  CRM_CarBasicData as CBD with(nolock)
			join dbo.vwMMV as vw with(nolock) on vw.VersionId=CBD.VersionId
			where vw.MakeId=15 -- skoda
			and CBD.CreatedOn>@date)
			and N.BuyTime='More than 2 months'





            select  @CustomersinCRM as CustomersinCRM ,@VerifiedCustomers as VerifiedCustomers,@InLoopCustomers as InLoopCustomers,
                    @DealerAssignedCustomers as DealerAssignedCustomers ,@CarBooked as CarBooked 







END

