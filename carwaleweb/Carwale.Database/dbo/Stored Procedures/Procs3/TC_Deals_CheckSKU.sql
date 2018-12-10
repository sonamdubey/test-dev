IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_Deals_CheckSKU]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_Deals_CheckSKU]
GO

	-- =============================================
-- Author:		Ruchira Patil
-- Create date: 5th Jan 2016
-- Description:	Check the existence SKU of aged cars(MMV,dealerId,color,makeyear)
-- =============================================
CREATE PROCEDURE [dbo].[TC_Deals_CheckSKU]
	@BranchId BIGINT,
	@VersionId INT,
	@ColorId INT,
	@MakeYear DATETIME
AS
BEGIN
	DECLARE @IsExist BIT,@DealsStockId INT = NULL

	SELECT @DealsStockId = Id FROM TC_Deals_Stock WITH (NOLOCk) WHERE BranchId = @BranchId AND CarVersionId = @VersionId AND VersionColorId = @ColorId AND MONTH(MakeYear) = MONTH(@MakeYear) AND YEAR(MakeYear) = YEAR(@MakeYear)

	IF @DealsStockId IS NOT NULL
	BEGIN
		SET @IsExist = 1
	END
	ELSE
	BEGIN
		SET @IsExist = 0
	END

	SELECT @IsExist IsExist , @DealsStockId DealsStockId
END

