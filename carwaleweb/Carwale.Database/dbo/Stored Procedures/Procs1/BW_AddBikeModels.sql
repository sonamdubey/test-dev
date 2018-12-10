IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[BW_AddBikeModels]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[BW_AddBikeModels]
GO

	
-- =============================================
-- Author:		Vivek Gupta 
-- Create date: 11-07-2016
-- Description:	Add Bike Models
-- =============================================
CREATE PROCEDURE BW_AddBikeModels
	@modelId INT,
	@MakeId INT,
	@ModelName VARCHAR(30),
	@ModelMaskingName VARCHAR(30),
	@HostUrl VARCHAR(100) = NULL,
	@OriginalImagePath VARCHAR(150) = NULL,
	@New BIT = NULL,
	@Used BIT = NULL,
	@Futuristic BIT = NULL
AS
BEGIN	
	SET NOCOUNT ON;

	INSERT INTO BikeModels(
	  Id
	 ,BikeMakeId
	 ,Name
	 ,MaskingName	 
	 ,HostURL
	 ,OriginalImagePath
	 ,New
	 ,Used
	 ,Futuristic
	 ,IsDeleted
	 )
  VALUES(
	  @modelId
     ,@MakeId
	 ,@ModelName
	 ,@ModelMaskingName	
	 ,ISNULL(@HostUrl, NULL )
	 ,ISNULL(@OriginalImagePath, NULL)
	 ,ISNULL(@New,1)
	 ,ISNULL(@Used,1)
	 ,ISNULL(@Futuristic,0)
	 ,0
  )

END
