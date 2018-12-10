IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveMultipleDealInquiries-v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveMultipleDealInquiries-v16]
GO

	

-- =============================================
-- Author:		Akansha	
-- Create date: 28/06/2016	
-- Description:	Insert multiple leads into dealinquiries table for multiple stockidds
-- Modified by: Purohith Guguloth on 8th july,2016
                -- Added a column PlatformId to be inserted
-- =============================================

CREATE PROCEDURE [dbo].[SaveMultipleDealInquiries-v16.7.3]

	@CustomerName			VarChar(50),
	@CustomerEmail			VarChar(100),
	@CustomerMobile			VarChar(10),
	@StockIds				VarChar(100),
	@CityId					int,
	@EntryDateTime			Datetime,
	@MasterCityId			INT = NULL,
	@Source                 VarChar(5) = NULL,
	@IsPaid				    BIT = 0,
	@PlatformId             INT = NULL       -- Modified by: Purohith Guguloth on 8th july,2016
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
		,PlatformId                     -- Modified by: Purohith Guguloth on 8th july,2016
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
	@IsPaid,
	@PlatformId                        -- Modified by: Purohith Guguloth on 8th july,2016
	from [dbo].[fnSplitCSV] (@StockIds)
END


