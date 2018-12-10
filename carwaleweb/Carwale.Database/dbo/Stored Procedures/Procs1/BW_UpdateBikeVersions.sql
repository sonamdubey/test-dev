IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_UpdateBikeVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_UpdateBikeVersions]
GO

	

-- =============================================
-- Author:		Vivek Gupta
-- Create date: 11-07-2016
-- Description:	Update Bike Versions
-- =============================================
CREATE PROCEDURE BW_UpdateBikeVersions
@VersionId INT,
@ModelId INT = NULL,
@VersionName VARCHAR(30) = NULL,
@IsNew BIT = NULL,
@IsUsed BIT = NULL,
@IsFuturistic BIT = NULL,
@IsDeleted BIT = 0		
AS
BEGIN	
	SET NOCOUNT ON;
	UPDATE BikeVersions SET 		
		 NAME = ISNULL(@VersionName,Name)		
		,New = ISNULL(@IsNew,New)
		,Used = ISNULL(@IsUsed,Used)
		,Futuristic = ISNULL(@IsFuturistic,Futuristic)		
		,IsDeleted = ISNULL(@IsDeleted,IsDeleted)
		,BikeModelId = ISNULL(@ModelId,BikeModelId)
	WHERE ID = @VersionId
END
