IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dba].[GetDealerStockPerformance]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dba].[GetDealerStockPerformance]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 2-5-2013
-- Description:	To capture Dealer stock details and rsponse
-- =============================================
CREATE PROCEDURE  [dba].[GetDealerStockPerformance]	
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	create table #tempstock(	
				DealerId int,
				Inquiryid int,
				MakeId int,
				MakeName varchar(100),
				ModelId int,
				ModelName varchar(100),
				VersionId int,
				VersionName varchar(100),
				CityId int,
				CityName varchar(100),
				MakeYear datetime,
				Price int,
				PhotoCount smallint,
				Kilometers int,
				InquiryAdded datetime,
				AdditionalFuel varchar(50),
				Owners varchar(50),
				Insurance varchar(50),
				InsuranceExpiry datetime,
				CertificationId smallint,
				CarFuelType tinyint,
				OneTimeTax varchar(50),
				Warranties varchar(500),
				Response int,
				CarAge int
				)
				
				insert into #tempstock
				select  d.Id as DealerId,l.Inquiryid,l.MakeId,l.MakeName,l.ModelId,l.ModelName,l.VersionId,l.VersionName,l.CityId,l.CityName,l.MakeYear,l.Price,
				l.PhotoCount,l.Kilometers,l.EntryDate as InquiryAdded,l.AdditionalFuel,
				sd.Owners,sd.Insurance,sd.InsuranceExpiry,l.CertificationId,cv.CarFuelType,sd.OneTimeTax,tc.Warranties,
				0 as Response,DATEDIFF(DAY,l.EntryDate ,GETDATE()) as CarAge
				--into dba.DealerStockPerformance
				from LiveListings as l with (nolock)
				join SellInquiries as s with (nolock) on s.ID=l.Inquiryid and l.SellerType=1
				join SellInquiriesDetails as sd with (nolock) on sd.SellInquiryId=l.Inquiryid and l.SellerType=1
				join dealers as d with (nolock) on d.ID=s.DealerId and d.CityId=1
				join TC_Stock as ts with (nolock) on  ts.Id=s.TC_StockId and ts.BranchId=d.id
				join TC_CarCondition as tc with (nolock) on  tc.StockId=ts.Id
				join CarVersions as cv on cv.ID=l.VersionId
				where  l.EntryDate >GETDATE()-30 				
				
				
				UPDATE #tempstock
				set Response=t1.rsp
				from (
				   select Inquiryid,COUNT(u.Id) as rsp
				   from UsedCarPurchaseInquiries as U
				   join #tempstock as t on t.Inquiryid=u.SellInquiryId
				   group by Inquiryid
				) t1
				join #tempstock as t on t.Inquiryid=t1.Inquiryid
				 

insert into dba.DealerStockPerformance
(DealerId,
Inquiryid,
MakeId,
MakeName,
ModelId,
ModelName,
VersionId,
VersionName,
CityId,
CityName,
MakeYear,
Price,
PhotoCount,
Kilometers,
InquiryAdded,
AdditionalFuel,
Owners,
Insurance,
InsuranceExpiry,
CertificationId,
CarFuelType,
OneTimeTax,
Warranties,
Response,
carage
)select DealerId,
Inquiryid,
MakeId,
MakeName,
ModelId,
ModelName,
VersionId,
VersionName,
CityId,
CityName,
MakeYear,
Price,
PhotoCount,
Kilometers,
InquiryAdded,
AdditionalFuel,
Owners,
Insurance,
InsuranceExpiry,
CertificationId,
CarFuelType,
OneTimeTax,
Warranties,
Response,
carage
from #tempstock

drop table #tempstock
   
END
