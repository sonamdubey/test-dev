IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_StockStatusChange_102012]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_StockStatusChange_102012]
GO

	-- Author: Surendra Chouksey
-- Create date: 28 Sept,2011
-- Description: Changing stock status to sols or not available
-- Modified : Date-9th Aug,Desc- LastUpdated=GETDATE() in added while updating Sellinquiries
-- =============================================
CREATE PROCEDURE [dbo].[TC_StockStatusChange_102012]
(
@StockIdChain VARCHAR(1000),
@Status TINYINT
)
AS
BEGIN
-- SET NOCOUNT ON added to prevent extra result sets from
SET NOCOUNT ON;
DECLARE @Separator CHAR(1)=','
DECLARE @Separator_position INT -- This is used to locate each separator character
DECLARE @array_value VARCHAR(1000) -- this holds each array value as it is returned
-- For my loop to work I need an extra separator at the end. I always look to the
-- left of the separator character for each array value

SET @StockIdChain = @StockIdChain + @Separator
WHILE PATINDEX('%' + @Separator + '%', @StockIdChain) <> 0
BEGIN
-- patindex matches the a pattern against a string
SELECT @Separator_position = PATINDEX('%' + @Separator + '%',@StockIdChain)
SELECT @array_value = LEFT(@StockIdChain, @Separator_position - 1)
-- Checking whether Stock is availabe in carwale or not
IF EXISTS(SELECT Id FROM SellInquiries WHERE TC_StockId=@array_value)
BEGIN
-- Making same stock not available in SellInquiry table
UPDATE SellInquiries SET StatusId =2, ModifiedDate=GETDATE(),LastUpdated=GETDATE() WHERE TC_StockId=@array_value
END

-- Updating Tc_stock and making it not availabe
UPDATE TC_Stock SET StatusId =@Status,IsSychronizedCW=0 ,LastUpdatedDate=GETDATE() WHERE Id=@array_value
-- This replaces what we just processed with and empty string
SELECT @StockIdChain = STUFF(@StockIdChain, 1, @Separator_position, '')
END --while End
END
