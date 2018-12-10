IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_AddBikeVersions]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_AddBikeVersions]
GO

	

-- =============================================
-- Author:		Vivek Gupta 
-- Create date: 11-07-2016
-- Description:	Add Bike Versions
-- =============================================
create PROCEDURE BW_AddBikeVersions
 @versionId INT 
,@ModelId INT = NULL
,@VersionName VARCHAR(30) = NULL
,@IsNew BIT = NULL
,@IsUsed BIT = NULL
,@IsFuturistic BIT = NULL

AS
BEGIN
	SET NOCOUNT ON;
	
	INSERT INTO BikeVersions(
	 Id
	 ,BikeModelId
	 ,Name	 
	 ,New
	 ,Used
	 ,Futuristic	
	 ,IsDeleted
	 )
  VALUES(
	  @versionId
     ,@ModelId
	 ,@VersionName	
	 ,ISNULL(@IsNew,1)
	 ,ISNULL(@IsUsed,0)
	 ,ISNULL(@IsFuturistic,0)
	 ,0
  )
END
