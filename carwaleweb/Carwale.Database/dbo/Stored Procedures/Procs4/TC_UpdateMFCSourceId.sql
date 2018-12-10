IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_UpdateMFCSourceId]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_UpdateMFCSourceId]
GO

	-- =============================================
-- Author:	Ruchira Patil
-- Create date: 6th Nov 2014
-- Description:	To Update MFCSourceId in Tc_Stock
-- =============================================
CREATE PROCEDURE [dbo].[TC_UpdateMFCSourceId] 
	@StockId INT,
	@MFCSourceId INT
AS
BEGIN
	UPDATE TC_Stock SET MFCSourceId = @MFCSourceId WHERE Id= @StockId
END
