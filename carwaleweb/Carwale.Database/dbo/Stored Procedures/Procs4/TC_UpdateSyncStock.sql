IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateSyncStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateSyncStock]
GO

	
-- =============================================
-- Author      : Chetan Navin
-- Create date : 19th Oct 2016 
-- Description : To update stocks synchronization flag 
-- =============================================
CREATE PROCEDURE [dbo].[TC_UpdateSyncStock]
	-- Add the parameters for the stored procedure here
	@StockIds VARCHAR(1000)
AS
BEGIN
	UPDATE TC_Stock
	SET IsSychronizedCW = 1
	WHERE Id IN (
			SELECT ListMember
			FROM fnSplitCSVValuesWithIdentity(@StockIds)
			)
END
