IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetClassifiedData]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetClassifiedData]
GO

	create procedure GetClassifiedData

as
Begin

declare  @Date datetime =GETDATE()-1

--Distinct dealer inquires for a day
select  count(distinct CustomerID) As DealerBuyer from UsedCarPurchaseInquiries with (nolock) where convert(varchar(8),RequestDateTime,112) = convert(varchar(8),@Date,112)

--Distinct customer inquires for a day
select  count(distinct CustomerID) As IndBuyer from ClassifiedRequests with (nolock) where convert(varchar(8),RequestDateTime,112) = convert(varchar(8),@Date,112)


--All dealer inquires for a day
select  count(CustomerID) As DealerInq from UsedCarPurchaseInquiries with (nolock) where convert(varchar(8),RequestDateTime,112) = convert(varchar(8),@Date,112)

--All Individual inquires for a day
select  count(CustomerID) As IndInq from ClassifiedRequests with (nolock) where convert(varchar(8),RequestDateTime,112) = convert(varchar(8),@Date,112)


end