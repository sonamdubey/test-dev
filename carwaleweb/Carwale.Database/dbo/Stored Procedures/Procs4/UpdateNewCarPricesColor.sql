IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[UpdateNewCarPricesColor]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[UpdateNewCarPricesColor]
GO

	-- =============================================
-- Author:		<Jitendra>
-- Create date: <10/12/2013>
-- Description:	<This Sp is used to update NewCarShowroomPrices prices>
-- =============================================
/* [dbo].[UpdateNewCarPricesColor] 10,493 */
CREATE PROCEDURE [dbo].[UpdateNewCarPricesColor]
	-- Add the parameters for the stored procedure here
	@VersionId	INT,
	@CityId     INT,
	@delColor   BIT
 AS
	DECLARE 
			@Price			NUMERIC(18,0),
			@RTO			NUMERIC(18,0), 
			@Insurance		NUMERIC(18,0), 
			@CorporateRTO	NUMERIC(18,0),
			@CarModelId		INT,
			@CategoryId_Price		SMALLINT = 3, 
			@CategoryId_RTO			SMALLINT = 4,
			@CategoryId_Insurance	SMALLINT = 5,			

			@ItemId_ExShowroomPrice SMALLINT = 2,
			@ItemId_RTO				SMALLINT = 3,
			@ItemId_CorporateRTO	SMALLINT = 4,
			@ItemId_Insurance		SMALLINT = 5
	BEGIN		
		
		DECLARE @ExistingColor BIT
		DECLARE @RecordExist INT

		SELECT @ExistingColor=isMetallic FROM NewCarShowroomPrices WITH(NOLOCK) WHERE CarVersionId=@VersionId AND CityId=@CityId
		
		SET  @RecordExist=@@ROWCOUNT
		
		IF @RecordExist > 0
			BEGIN
				IF @ExistingColor = @delColor
					BEGIN
						
						
						DELETE FROM NewCarShowroomPrices where CarVersionId =@VersionId AND CityId=@CityId
						
						SELECT @CarModelId=CarModelId FROM CarVersions WITH(NOLOCK) WHERE ID=@VersionId
						 
						DECLARE @newInsertcolor BIT
						SET @newInsertcolor = CASE WHEN @ExistingColor = 1  THEN 0 ELSE 1 END

						SELECT 
							@Price=PQ_CategoryItemValue 
						FROM 
							CW_NewCarShowroomPrices WITH(NOLOCK) 
						WHERE 
							CarVersionId =@VersionId AND CityId=@CityId AND isMetallic=@newInsertcolor AND PQ_CategoryItem=@ItemId_ExShowroomPrice
						
						SELECT 
							@Insurance=PQ_CategoryItemValue 
						FROM 
							CW_NewCarShowroomPrices WITH(NOLOCK) 
						WHERE CarVersionId =@VersionId AND CityId=@CityId AND isMetallic=@newInsertcolor AND PQ_CategoryItem=@ItemId_Insurance
						
						SELECT 
							@RTO=PQ_CategoryItemValue 
						FROM 
							CW_NewCarShowroomPrices WITH(NOLOCK)   
						WHERE 
							CarVersionId =@VersionId AND CityId=@CityId AND isMetallic=@newInsertcolor AND PQ_CategoryItem=@ItemId_RTO
							
						SELECT 
							@RTO=PQ_CategoryItemValue 
						FROM 
							CW_NewCarShowroomPrices WITH(NOLOCK)   
						WHERE 
							CarVersionId =@VersionId AND CityId=@CityId AND isMetallic=@newInsertcolor AND PQ_CategoryItem=@ItemId_CorporateRTO
							
						IF @Price > 0
							BEGIN
								INSERT INTO NewCarShowroomPrices
								(	CarVersionId, CityId, 
									Price, RTO, Insurance, CorporateRTO,
									LastUpdated,IsActive,CarModelId,isMetallic
								)

								VALUES
								(	@VersionId, @CityId, 
									@Price, @RTO, @Insurance, @CorporateRTO,
									GETDATE(),1, @CarModelId,@newInsertcolor
								)
							END
					END
			END
	
	  
	END


