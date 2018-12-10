IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveDealInquiry]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveDealInquiry]
GO

	
-- =============================================
-- Author:		Piyush	
-- Create date: 1/7/2016	
-- Description:	Insert SaveDealInquiry
-- =============================================

CREATE PROCEDURE [dbo].[SaveDealInquiry]

	@Id					    INT,
	@CustomerName			VarChar(50),
	@CustomerEmail			VarChar(100),
	@CustomerMobile			VarChar(10),
	@StockId				int,
	@CityId					int,
	@EntryDateTime			Datetime,
	@RecordID				INT OUTPUT
				
 AS

BEGIN
 
	IF @Id = - 1
	BEGIN
		INSERT INTO DealInquiries (
			CustomerName
			,CustomerEmail
			,CustomerMobile
			,StockId
			,CityId
			,EntryDateTime
			)
		VALUES (
			@CustomerName
			,@CustomerEmail
			,@CustomerMobile
			,@StockId
			,@CityId	
			,@EntryDateTime
			)

			SET @RecordID = SCOPE_IDENTITY()
INSERT INTO TC_Deals_DroppedOffUsersCalls (DealInquiries_Id,LastCallTime,FollowUpTime,Comments,Status) 
			VALUES(@RecordID,GETDATE(),DATEADD(n,15,GETDATE()),'',1)  --Modified By: Akansha 18/02/2016
END
ELSE
        BEGIN
            SET @RecordID = 0
        END
END