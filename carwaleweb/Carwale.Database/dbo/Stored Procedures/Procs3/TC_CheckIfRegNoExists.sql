IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CheckIfRegNoExists]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CheckIfRegNoExists]
GO

	-- =============================================
-- Author:		Vicky gupta
-- Create date: 23-07-2015
-- Description:	Return Data if Registration no. exist in AbSure_CarDetails
-- =============================================

create PROCEDURE  [dbo].[TC_CheckIfRegNoExists]
	@RegNumber VARCHAR(50),
	@AbSure_CarDetailsId INT OUTPUT	
AS

BEGIN 

	DECLARE @StockId	INT
	SET @AbSure_CarDetailsId = -1

	SELECT TOP 1 @AbSure_CarDetailsId = AC.Id ,@StockId = StockId
	FROM  AbSure_CarDetails AC
	INNER JOIN TC_Stock TS ON AC.StockId = TS.Id  
	WHERE RegNumber = @RegNumber AND (TS.IsActive = 0 or TS.StatusId = 3)
	ORDER BY AC.EntryDate DESC

	PRINT @StockId

	-- CHECK IF THAT STOCK IS SOLD OR INACTIVE

	SELECT Id FROM TC_Stock WHERE Id = @StockId AND (StatusId = 3 OR IsActive = 0 )

	IF(@@ROWCOUNT = 0)
		SET @AbSure_CarDetailsId = -1

	PRINT @AbSure_CarDetailsId

END



----------------------------------------------------------------------------------------------------------------------------------------------------

/****** Object:  StoredProcedure [dbo].[Tc_UpdateCarDetailsStockId]    Script Date: 7/30/2015 1:49:16 PM ******/
SET ANSI_NULLS ON
