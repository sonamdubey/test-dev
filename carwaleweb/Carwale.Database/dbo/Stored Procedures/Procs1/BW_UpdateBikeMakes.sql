IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_UpdateBikeMakes]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_UpdateBikeMakes]
GO

	-- =============================================
-- Author:		Vivek Gupta
-- Create date: 11-07-2016
-- Description:	Update Bike Makes
-- =============================================
create PROCEDURE BW_UpdateBikeMakes
@MakeId INT = null,
@MakeName VARCHAR(30) = null,
@IsNew BIT = null,
@IsUsed BIT = null,
@IsFuturistic BIT = null,
@MaskingName varchar(30) = null,
@IsDeleted BIT = null	
AS
BEGIN	
	SET NOCOUNT ON;
	UPDATE BikeMakes SET 		
		 Name = ISNULL(@MakeName,Name)		
		,New = ISNULL(@IsNew,New)
		,Used = ISNULL(@IsUsed,Used)
		,MaskingName = ISNULL(@MaskingName,MaskingName)
		,Futuristic = ISNULL(@IsFuturistic,Futuristic)
		,IsDeleted = ISNULL(@IsDeleted,IsDeleted)
	WHERE ID = @MakeId

	IF(ISNULL(@IsDeleted,0) = 1)
	BEGIN
		UPDATE BikeModels
		SET IsDeleted = @IsDeleted
		WHERE BikeMakeId = @MakeId

		UPDATE BikeVersions
		SET IsDeleted = @IsDeleted
		WHERE BikeModelId IN 
		 (SELECT Id FROM BikeModels WITH(NOLOCK) 
		   WHERE BikeMakeId = @MakeId)
	END

END

