IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BA_DeleteStockImage]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BA_DeleteStockImage]
GO

	-- =============================================
-- Author:		<Author,,Name>
-- Create date: <Create Date,,>
-- Description:	<Description,,>
-- =============================================
CREATE PROCEDURE [dbo].[BA_DeleteStockImage]
@ImageId INT
AS
BEGIN
	SET NOCOUNT ON;
	DECLARE @Status BIT = 0 
	UPDATE BA_StockImage  SET ModifyDate = GETDATE(), IsActive = 0 WHERE ID = @ImageId
	SET @Status =1
END

SELECT @Status AS Status ---retunr
