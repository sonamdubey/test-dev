IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DiscardSuspendedStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DiscardSuspendedStock]
GO

	-- =============================================
-- Author:		Surendra Chouksey
-- Create date: 19 Dec,2011
-- Description:	This Procedure is used to discard suspended stocks
-- Modified by: Manish on 27-02-2014 Removed LastUpdatedDatd update from sellinquiries since when stock status change and again upload to CarWale lastUpdated date should not be changed.
-- =============================================
CREATE PROCEDURE [dbo].[TC_DiscardSuspendedStock]
(
@BranchId NUMERIC,-- DelerId
@StockIds VARCHAR(MAX)=NULL,--this param contain comma seperated stock ids
@UserId INT -- Primary key of TC_users's Table is used for modifiedby
)	
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @ExpDate DATETIME
	
	SELECT Top 1 @ExpDate=ExpiryDate FROM ConsumerCreditPoints WHERE ConsumerType=1 AND ConsumerId=@BranchId ORDER BY ExpiryDate DESC
	
	DECLARE @Separator_position INT -- This is used to locate each separator character  
	DECLARE @StockId VARCHAR(MAX) -- this holds each array value as it is returned  
  -- For my loop to work I need an extra separator at the end. I always look to the  
  -- left of the separator character for each array value  
  
	SET @StockIds = @StockIds + ','  
  
  -- Loop through the string searching for separtor characters    
	WHILE PATINDEX('%' + ',' + '%', @StockIds) <> 0   
		BEGIN  			
			-- patindex matches the a pattern against a string  
			SELECT  @Separator_position = PATINDEX('%' + ',' + '%',@StockIds)  
			SELECT  @StockId = LEFT(@StockIds, @Separator_position - 1)  
			
			--Updating SellInquiries Table to make stock Active by Updating Expiry date
			UPDATE SellInquiries SET PackageExpiryDate=@ExpDate,
			--LastUpdated=GETDATE(),  --- Commented by Manish on 27-02-2014 since LastUpdatedDate should change only for certain logic
			ModifiedBy=@UserId,
			StatusId=2
			WHERE DealerId=@BranchId AND TC_StockId=@StockId AND StatusId=1
			
			-- Updating TC_Stock for making stock availabe
			UPDATE TC_Stock SET StatusId=1,IsSychronizedCW=0,LastUpdatedDate=GETDATE(),ModifiedBy=@UserId
			WHERE Id=@StockId AND BranchId=@BranchId AND StatusId=4
			
	        
			-- This replaces what we just processed with and empty string  
			SELECT  @StockIds = STUFF(@StockIds, 1, @Separator_position, '')  
		END 
	RETURN 0
	
END
