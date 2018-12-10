IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Acc_InsertItems]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Acc_InsertItems]
GO

	

CREATE PROCEDURE [dbo].[Acc_InsertItems]
	@Id						NUMERIC,
	@BrandId				NUMERIC,
	@ProductId				NUMERIC,
	@Title					VARCHAR(100),
	@OEM_Number				VARCHAR(50),
	@Dimension_Length		INT,
	@Dimension_Width		INT,
	@Dimension_Height		INT,
	@ShippingWeight			VARCHAR(50),
	@MRP					NUMERIC,
	@InclusiveOfTax			NUMERIC,
	@FeaturesLocation		VARCHAR(50),
	@DescriptionLocation	VARCHAR(50),
	@PrimaryImageLocation	VARCHAR(50),
	@ProductFeature			VARCHAR(MAX),
	@ProductDescription		VARCHAR(MAX),
	@EntryDate				DATETIME,
	@Url					VARCHAR(100),
	@ItemId					NUMERIC OUTPUT
 AS
	
BEGIN
	SET NOCOUNT ON
	IF  @Id = -1
		BEGIN
			INSERT INTO Acc_Items
			 ( 
				BrandId,			ProductId,		Title,			OEM_Number,		Dimension_Length,	
				
				Dimension_Width,		Dimension_Height,	ShippingWeight, 	MRP,			InclusiveOfTax,		

				FeaturesLocation,		DescriptionLocation,	PrimaryImageLocation,ProductFeature,ProductDecription,	EntryDate, HostUrl
			)
			VALUES
			 (
				@BrandId,			@ProductId,		@Title,			@OEM_Number,	@Dimension_Length,	
				
				@Dimension_Width,		@Dimension_Height,	@ShippingWeight, 	@MRP,			@InclusiveOfTax,		

				@FeaturesLocation,		@DescriptionLocation,	@PrimaryImageLocation,@ProductFeature,@ProductDescription,	@EntryDate, @Url			
			)
			SET @ItemId = SCOPE_IDENTITY()

		END
	ELSE
		BEGIN

			UPDATE  Acc_Items  SET
			
				BrandId 			= @BrandId,			
				ProductId 			= @ProductId,		
				Title 				= @Title,
				OEM_Number 			= @OEM_Number,		
				Dimension_Length	= @Dimension_Length,	
				Dimension_Width 	= @Dimension_Width,		
				Dimension_Height 	= @Dimension_Height,		
				ShippingWeight 		= @ShippingWeight, 	
				MRP 				= @MRP,			
				InclusiveOfTax 		= @InclusiveOfTax, 
				ProductFeature		= @ProductFeature,
				ProductDecription	= @ProductDescription,
				EntryDate 			= @EntryDate,
				HostUrl				= @Url
			WHERE
				Id = @Id

			SET @ItemId = @Id
		END

		UPDATE  Acc_Items  SET PrimaryImageLocation = 'pri_s' + CAST(@ItemId As varchar(100)) + '.jpg'
		WHERE Id = @ItemId
END


