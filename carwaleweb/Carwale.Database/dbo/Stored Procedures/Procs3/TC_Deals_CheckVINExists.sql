IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_CheckVINExists]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_CheckVINExists]
GO

	
-- ===============================================================================================
-- Author		: Khushaboo Patil
-- Create date	: 8th Jan, 2016
-- Description	: Check whether VIN already Exists or not
-- ==================================================================================================
CREATE PROCEDURE [dbo].[TC_Deals_CheckVINExists] 
	@VINNo VARCHAR(20),
	@RetVal	INT OUTPUT
AS
BEGIN
	SELECT @RetVal = TC_DealsStockVINId
	FROM TC_Deals_StockVIN WITH (NOLOCK)
	WHERE  VINNo = @VINNo			
END

