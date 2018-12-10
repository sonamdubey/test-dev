IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_DeleteStock]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_DeleteStock]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BA_DeleteStock]
	@StockId INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Status BIT = 0 
	UPDATE BA_Stock  SET IsActive = 0  WHERE ID = @StockId ;
	UPDATE BA_StockDetails  SET IsActive = 0  WHERE StockId = @StockId ;
	SET   @Status = 1 ;

	SELECT @Status AS Status --retunr
END
