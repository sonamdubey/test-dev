IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Microsite_AddDealerModelFeatureDescription]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Microsite_AddDealerModelFeatureDescription]
GO

	
------------------------------------------------------------------------
-- =============================================
-- Author:		Mihir A Chheda
-- Create date: 01-06-2015
-- Description:	Add model feature description 
-- Modified by : Kritika Choudhary on 11th August 2015, done versioning of originalimgpath 
-- =============================================
CREATE PROCEDURE [dbo].[Microsite_AddDealerModelFeatureDescription] 
@DWModelFeatureCategoriesId INT,
@Title VARCHAR(100),
@Description VARCHAR(MAX),
@HostUrl VARCHAR(100)=NULl,
@ImgPath VARCHAR(100)=NULl,
@ImgName VARCHAR(100)=NULl,
@OriginalImgPath VARCHAR(300)=NULL,
@SortOrder INT,
@operationType INT,
@isActive INT,
@StatusCode INT=NULL,
@Id INT OUTPUT,
@Status INT OUTPUT
AS
BEGIN
 
	DECLARE @todaydate datetime=GETDATE() 

	IF(@ImgName IS NOT NULL) -- Versioning of the Image on basis of current date time
	BEGIN
		SET @ImgName = @ImgName + '?v=' + REPLACE(CONVERT(VARCHAR,@todaydate,112)+ CONVERT(VARCHAR,@todaydate,114),':','');
	END
	IF(@OriginalImgPath IS NOT NULL) -- Versioning of the Image on basis of current date time
	BEGIN
		SET @OriginalImgPath = @OriginalImgPath + '?v=' + REPLACE(CONVERT(VARCHAR,@todaydate,112)+ CONVERT(VARCHAR,@todaydate,114),':','');
	END

 IF @operationType=0
 BEGIN

		INSERT INTO    Microsite_DWModelFeatures
					  (Microsite_DWModelFeatureCategoriesId,FeatureTitle,FeatureDescription,SortOrder)
		VALUES        (@DWModelFeatureCategoriesId,@Title,@Description,@SortOrder)

		SET            @Id=SCOPE_IDENTITY()
  END

 ELSE IF @operationType=1
 BEGIN
    UPDATE Microsite_DWModelFeatures
	SET    HostUrl=@HostUrl,ImgPath=@ImgPath,ImgName=@ImgName,OriginalImgPath=@OriginalImgPath
	WHERE  Id=@Id
  END

  ELSE IF @operationType=2
  BEGIN
  IF(@StatusCode=1)
	BEGIN
	   UPDATE Microsite_DWModelFeatures 
	   SET    IsActive=0
	   WHERE  ID=@Id
	   SET    @Status=0
	END
	ELSE
	BEGIN
		UPDATE Microsite_DWModelFeatures 
		SET    IsActive=1
		WHERE  ID=@Id
		SET    @Status=1
	END
  END

  ELSE IF @operationType=3
  BEGIN     
		UPDATE Microsite_DWModelFeatures
		SET	FeatureTitle=@Title,FeatureDescription=@Description,SortOrder=@SortOrder,ModifiedDate=GETDATE(),
			ImgName=ISNULL(@ImgName, ImgName)
		WHERE  Id=@Id
  END

END