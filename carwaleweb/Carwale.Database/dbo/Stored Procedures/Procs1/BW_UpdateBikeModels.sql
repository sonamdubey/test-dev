IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_UpdateBikeModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_UpdateBikeModels]
GO

	

-- =============================================
-- Author:		Vivek Gupta
-- Create date: 11-07-2016
-- Description:	Update Bike Details
-- =============================================
CREATE PROCEDURE BW_UpdateBikeModels
	@MakeId INT = NULL,
	@ModelName VARCHAR(30) = NULL,
	@ModelMaskingName VARCHAR(30) = NULL,
	@HostUrl VARCHAR(100) = NULL,
	@OriginalImagePath VARCHAR(150) =NULL ,
	@IsUsed BIT = NULL,
    @IsNew BIT = NULL,
	@IsFuturistic BIT = NULL,
	@IsDeleted BIT = NULL,
	@ModelId INT = 0
AS
BEGIN	
	SET NOCOUNT ON;
	UPDATE BikeModels SET 		
		 Name = ISNULL(@ModelName,Name)	
		,Used = ISNULL(@IsUsed,Used)	
		,New = ISNULL(@IsNew,New)
		,Futuristic = ISNULL(@IsFuturistic,Futuristic)
		,MaskingName = ISNULL(@ModelMaskingName,MaskingName)
		,IsDeleted = ISNULL(@IsDeleted,IsDeleted)
		,HostURL = ISNULL(@HostUrl,HostURL)
		,OriginalImagePath = ISNULL(@OriginalImagePath,OriginalImagePath)
		,BikeMakeId = ISNULL(@MakeId,BikeMakeId)
	WHERE ID = @ModelId

	IF(ISNULL(@IsDeleted,0) = 1)
	BEGIN		
		UPDATE BikeVersions
		SET IsDeleted = @IsDeleted
		WHERE BikeModelId = @ModelId 		 
	END
END
