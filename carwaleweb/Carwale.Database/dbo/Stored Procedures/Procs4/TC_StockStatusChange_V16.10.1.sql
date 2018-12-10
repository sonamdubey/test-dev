IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_StockStatusChange_V16]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_StockStatusChange_V16]
GO

	
-- Author:  Surendra Chouksey    
-- Create date: 28 Sept,2011    
-- Description: Changing stock status to sols or not available  
-- Modified   : Date-9th Aug,Desc- LastUpdated=GETDATE() in added while updating Sellinquiries  
-- Modified   : Tejashree Patil on Date-25th Sept 2012 ,Desc- Added branchId new parameter  
-- Modified   : Tejashree Patil on Date-16th Oct 2012 ,Changed PATINDEX to CHARINDEX
-- Modified By:  Manish on 30-07-2013 for maintaining log of the removed car
-- Modified by: Manish on 27-02-2014 Removed LastUpdatedDatd update from sellinquiries since when stock status change and again upload to CarWale lastUpdated date should not be changed.
-- Modifie By : Vivek Gupta on 14th July,2014 , Added @CustomerCameFrom
-- Modified By : Chetan Navin on 21st Oct 2016 (Removed update query on SellInquiries table) 
-- =============================================    
CREATE PROCEDURE [dbo].[TC_StockStatusChange_V16.10.1] (
	@StockIdChain VARCHAR(1000)
	,@Status TINYINT
	,@BranchId BIGINT
	,@CustomerCameFrom VARCHAR(200) = NULL
	)
AS
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from      
	SET NOCOUNT ON;

	DECLARE @Separator CHAR(1) = ','
	DECLARE @Separator_position INT -- This is used to locate each separator character      
	DECLARE @array_value VARCHAR(1000) -- this holds each array value as it is returned      
		-- For my loop to work I need an extra separator at the end. I always look to the      
		-- left of the separator character for each array value      

	SET @StockIdChain = @StockIdChain + @Separator

	WHILE CHARINDEX(@Separator, @StockIdChain) <> 0
	BEGIN
		-- patindex matches the a pattern against a string      
		SELECT @Separator_position = CHARINDEX(@Separator, @StockIdChain)

		SELECT @array_value = LEFT(@StockIdChain, @Separator_position - 1)

		-- Updating Tc_stock and making it not availabe    
		UPDATE TC_Stock
		SET StatusId = @Status
			,IsSychronizedCW = 0
			,LastUpdatedDate = GETDATE()
			,SoldToCustomerFrom = @CustomerCameFrom
		WHERE Id = @array_value
			AND BranchId = @BranchId

		-- This replaces what we just processed with and empty string      
		SELECT @StockIdChain = STUFF(@StockIdChain, 1, @Separator_position, '')
	END --while End 
END
