IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_GetItems]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_GetItems]
GO

	
---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------
/*
	THIS STORED PROCEDURE RETURN COMPLETE DETAILS OF ITEM ALONG WITH
	BRAND AND PRODUCT TO WHOME ITEM BLONGS

	WRITTEN BY : SATISH SHARMA ON 12 JUN 08

	Changes History:
       
       Edited By               		EditedON               		Description
       -----------------------------                	-------------------------               	---------------------
       Satish Sharma                	12-Jun-2008 5:37 PM       	 Created
*/

CREATE PROCEDURE [dbo].[Acc_GetItems] 
	@ItemId		NUMERIC,
	@BrandId		NUMERIC		OUTPUT,
	@ProductId		NUMERIC		OUTPUT,
	@BrandName		VARCHAR(50)		OUTPUT,
	@ProductName		VARCHAR(50)		OUTPUT,
	@Title			VARCHAR(100)		OUTPUT,
	@OEM_No		VARCHAR(50)		OUTPUT,
	@Length		INTEGER		OUTPUT,
	@Width			INTEGER		OUTPUT,
	@Height		INTEGER		OUTPUT,
	@ShippingWeight	VARCHAR(50)		OUTPUT,
	@MRP			NUMERIC		OUTPUT,
	@InclusiveOfTax	SMALLINT		OUTPUT,
	@ProductFeature	VARCHAR(max)		OUTPUT,
	@ProductDecription	VARCHAR(max)		OUTPUT,
	@PrimaryImageLocation	VARCHAR(50)		OUTPUT,
	@ReviewRating		DECIMAL(18,2)		OUTPUT,
	@ReviewCount		NUMERIC		OUTPUT,
	@EntryDate		DATETIME		OUTPUT,
	@IsExist		BIT			OUTPUT,
	@HostUrl	VARCHAR(100) OUTPUT
AS
	
BEGIN
	SELECT 
			@BrandId 		= Itm.BrandId, 
			@ProductId 		= Itm.ProductId, 
			@BrandName 		= Br.BrandName, 
			@ProductName 		= Pr.ProductName, 
			@Title 			= Itm.Title,
			@OEM_No 		= Itm.OEM_Number,
			@Length 		= Itm.Dimension_Length,
			@Width 		= Itm.Dimension_Width,
			@Height 		= Itm.Dimension_Height,
			@ShippingWeight 	= Itm.ShippingWeight,
			@MRP 			= Itm.MRP,
			@InclusiveOfTax 	= Itm.InclusiveOfTax,
			@ProductFeature 	= Itm.ProductFeature,
			@ProductDecription 	= Itm.ProductDecription,
			@PrimaryImageLocation 	= Itm.PrimaryImageLocation,
			@ReviewRating 	= Itm.ReviewRating,
			@ReviewCount 		= Itm.ReviewCount,
			@EntryDate 		= Itm.EntryDate,
			@HostUrl = Itm.HostUrl
		
	FROM		 Acc_Items Itm, Acc_Brands Br, Acc_Products Pr
	WHERE 	Itm.BrandId = Br.Id AND Itm.ProductId = Pr.Id AND Itm.Id = @ItemId AND 
			Itm.IsActive = 1 AND Br.IsActive = 1 AND Pr.IsActive = 1

	IF @@ROWCOUNT > 0 SET @IsExist = 1
	ELSE Set @IsExist = 0
END


