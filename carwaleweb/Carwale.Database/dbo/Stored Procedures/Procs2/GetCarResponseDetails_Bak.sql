IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[GetCarResponseDetails_Bak]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[GetCarResponseDetails_Bak]
GO

	-- =============================================
-- Author:		Avishkar
-- Create date: 8/25/2011
-- Description:	Cars Response details
-- =============================================
CREATE PROCEDURE [dbo].[GetCarResponseDetails_Bak] 
	-- Add the parameters for the stored procedure here
	@Days smallint
AS
BEGIN

	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	
	DECLARE @date datetime
	SET @date = GETDATE()-@Days

	select CS.Id as ProfileId,1 as SellerType, 'Dealer' as Seller,U.CustomerId,CS.EntryDate,U.RequestDateTime as ResponseDate
    from UsedCarDealerCarResponse as U
      Join SellInquiries as CS on CS.Id=U.SellInquiryId
      where CS.EntryDate>=@date
   
      
	
	UNION
	
	select CS.Id as ProfileId,2 as SellerType, 'Individual' as Seller,CS.CustomerId,CS.EntryDate,U.RequestDateTime as ResponseDate
    from UsedCarIndividualCarResponse as U
      Join CustomerSellInquiries as CS on CS.Id=U.SellInquiryId
      where CS.EntryDate>=@date

END
