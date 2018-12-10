IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[SaveDealInquiry-v16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[SaveDealInquiry-v16]
GO

	


-- =============================================
-- Author:		Piyush	
-- Create date: 1/7/2016	
-- Description:	Insert SaveDealInquiry
-- Modified By: Sourav 13/05/2016
-- Modification Description: added src as input parameter
--  Modified By: Mukul ON 17/05/2016  Addde NULL value for @Source parameter   
-- Modified by Purohith on 1st June,2016
-- =============================================

CREATE PROCEDURE [dbo].[SaveDealInquiry-v16.5.5]

	@Id					    INT,
	@CustomerName			VarChar(50),
	@CustomerEmail			VarChar(100),
	@CustomerMobile			VarChar(10),
	@StockId				int,
	@CityId					int,
	@EntryDateTime			Datetime,
	@RecordID				INT OUTPUT,
	@MasterCityId			INT = NULL,
	@Source                 VarChar(5) = NULL, -- Addde null by Mukul 17/05/2016
	@IsPaid				    BIT = 0      --Added by Purohith on 1st June,2016
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
			,MasterCityId
			,Source
			,isPaid
			)
		VALUES (
			@CustomerName
			,@CustomerEmail
			,@CustomerMobile
			,@StockId
			,@CityId	
			,@EntryDateTime
			,@MasterCityId
			,@Source
			,@IsPaid
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


