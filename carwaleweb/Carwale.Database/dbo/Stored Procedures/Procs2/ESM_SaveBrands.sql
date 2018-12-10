IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[ESM_SaveBrands]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[ESM_SaveBrands]
GO

	
/*
	Procedure is created 12 Jan 2010
	for ESM_brands Data Entry
*/
CREATE PROCEDURE [dbo].[ESM_SaveBrands]
(
	@BrandName AS VARCHAR(50), 
	@ClientId AS NUMERIC(18,0), 
	@CategoryId AS NUMERIC(18,0), 
	@RegionId AS NUMERIC(18,0),
	@ID AS NUMERIC(18,0), 
	@IsActive AS BIT, 
	@UpdatedOn AS DATETIME, 
	@UpdatedBy AS NUMERIC(18,0)
)
AS
BEGIN
	IF(@ID = -1)
		BEGIN

			INSERT INTO ESM_Brands ( BrandName, ClientId, CategoryId, RegionId, IsActive, UpdatedOn, UpdatedBy )
						VALUES	   ( @BrandName, @ClientId, @CategoryId, @RegionId, @IsActive, @UpdatedOn, @UpdatedBy ) 
		END				
	ELSE
		BEGIN
			UPDATE ESM_Brands SET 
			BrandName = @BrandName, ClientId = @ClientId, CategoryId = @CategoryId, RegionId = @RegionId, IsActive = @IsActive, UpdatedOn = @UpdatedOn, UpdatedBy = @UpdatedBy
			WHERE id = @ID
		END	

--SELECT * FROM ESM_Brands 
--TRUNCATE TABLE ESM_Brands 

END

