IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveMultipleDealInquiries]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveMultipleDealInquiries]
GO

	

-- =============================================
-- Author:		Akansha	
-- Create date: 28/06/2016	
-- Description:	Insert multiple leads into dealinquiries table for multiple stockidds
-- =============================================

CREATE PROCEDURE [dbo].[SaveMultipleDealInquiries]

	@CustomerName			VarChar(50),
	@CustomerEmail			VarChar(100),
	@CustomerMobile			VarChar(10),
	@StockIds				VarChar(100),
	@CityId					int,
	@EntryDateTime			Datetime,
	@MasterCityId			INT = NULL,
	@Source                 VarChar(5) = NULL,
	@IsPaid				    BIT = 0      
 AS

BEGIN
	INSERT INTO DealInquiries
	(
		CustomerName
		,CustomerEmail
		,CustomerMobile
		,StockId
		,CityId
		,EntryDateTime
		,MasterCityId
		,Source
		,isPaid
	)
	OUTPUT inserted.ID,GETDATE(),DATEADD(n,15,GETDATE()),'',1 
	INTO TC_Deals_DroppedOffUsersCalls(DealInquiries_Id,LastCallTime,FollowUpTime,Comments,Status)
	SELECT 	
	@CustomerName,
	@CustomerEmail,
	@CustomerMobile,
	ListMember,
	@CityId,
	@EntryDateTime,
	@MasterCityId,
	@Source,
	@IsPaid
	from [dbo].[fnSplitCSV] (@StockIds)
END

















