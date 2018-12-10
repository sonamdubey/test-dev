IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCarResponseDetailsByDate]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCarResponseDetailsByDate]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 8/25/2011
-- Description:	Cars Response details
-- [dbo].[GetCarResponseDetails] 60
-- =============================================
CREATE PROCEDURE [dbo].[GetCarResponseDetailsByDate] 
	-- Add the parameters for the stored procedure here
	@date datetime
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	--DECLARE @date datetime
	--SET @date = GETDATE()-@Days
	
	TRUNCATE TABLE TempCarResponse

	--select CS.Id as ProfileId,1 as SellerType, 'Dealer' as Seller,U.CustomerId,CS.EntryDate,U.RequestDateTime as ResponseDate
 --   from UsedCarDealerCarResponse as U
 --     Join SellInquiries as CS on CS.Id=U.SellInquiryId
 --     where CS.EntryDate>=@date
      INSERT INTO TempCarResponse
      SELECT CS.Id as ProfileId,
			   1 as SellerType, 'Dealer' as Seller,
			   CS.EntryDate,
			   U.RequestDateTime as ResponseTime,
			   U.CustomerId
		FROM  dbo.UsedCarPurchaseInquiries as U
		JOIN  SellInquiries as CS on CS.Id=U.SellInquiryId
		WHERE CS.Id IN 
		(
			select CS.Id
			from dbo.UsedCarPurchaseInquiries as U
			Join SellInquiries as CS on CS.Id=U.SellInquiryId
			where U.RequestDateTime>=@date
		   )  
	UNION	
	SELECT CS.Id as ProfileId,
			   2 as SellerType, 'Individual' as Seller,
			   CS.EntryDate,
			   U.RequestDateTime as ResponseTime,
			   CS.CustomerId
		FROM  dbo.ClassifiedRequests as U
		JOIN  CustomerSellInquiries as CS on CS.Id=U.SellInquiryId
		WHERE CS.Id IN 
		(
			select CS.Id
			from dbo.ClassifiedRequests as U
			Join CustomerSellInquiries as CS on CS.Id=U.SellInquiryId
			where U.RequestDateTime>=@date
			and CS.PackageType=2
		 )
		 and CS.PackageType=2
      
      
		

END
