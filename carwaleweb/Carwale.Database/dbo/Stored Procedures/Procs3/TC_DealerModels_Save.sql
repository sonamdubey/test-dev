IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_DealerModels_Save]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_DealerModels_Save]
GO

	
---------------------------------------------------------
-- =============================================      
-- Author:  Kritika Choudhary
-- Create date:  19th May 2015 
-- Description: To Save Dealer Models   
-- Modified by:Komal Manjare on 7th August 2015
-- New parameter @OriginalImgPath added
-- Modified by:Komal Manjare on 11th August 2015
-- versioning of OriginalImgPath
-- =============================================      
CREATE PROCEDURE [dbo].[TC_DealerModels_Save]  
(      
 @DealerId		INT=NULL,      
 @CWModelId		INT=NULL,  
 @DWBodyStyleId INT=NULL,
 @DWModelName	VARCHAR(50)=NULL,
 @HostUrl		VARCHAR(50)=NULL,
 @ImgPath       VARCHAR(100)=NULL,
 @ImgName       VARCHAR(50)=NULL,
 @ID            INT=NULL OUTPUT,
 @OriginalImgPath VARCHAR(300)=NULL
 )      
AS      
BEGIN   

  -- SET NOCOUNT ON added to prevent extra result sets from      
 SET NOCOUNT ON;   

	DECLARE @Todaydate DATETIME
	SET @Todaydate = GETDATE(); 

	IF(@ImgName IS NOT NULL) -- Versioning of the Image on basis of current date time
	BEGIN
		SET @ImgName = @ImgName + '?v=' + REPLACE(CONVERT(VARCHAR,@Todaydate,112)+ CONVERT(VARCHAR,@Todaydate,114),':','');
	END
	IF(@OriginalImgPath IS NOT NULL)
	BEGIN 
	SET @OriginalImgPath = @OriginalImgPath + '?v=' + REPLACE(CONVERT(VARCHAR,@Todaydate,112)+ CONVERT(VARCHAR,@Todaydate,114),':','');
	END
    
   IF(@ID IS NULL AND @DealerId IS NOT NULL)
	   BEGIN
		   INSERT INTO TC_DealerModels(DealerId,CWModelId,DWModelName,DWBodyStyleId)    
		   VALUES(@DealerId,@CWModelId,@DWModelName,@DWBodyStyleId)
		   SET @ID = SCOPE_IDENTITY();
		  
	   END
  ELSE
	BEGIN
		UPDATE TC_DealerModels
		SET HostUrl  = ISNULL(@HostUrl, HostUrl), ImgPath = ISNULL(@ImgPath, ImgPath), ImgName= ISNULL(@ImgName, ImgName),
		DWModelName =ISNULL(@DWModelName, DWModelName),DWBodyStyleId =ISNULL(@DWBodyStyleId, DWBodyStyleId) , ModifiedDate= GETDATE(),
		OriginalImgPath=ISNULL(@OriginalImgPath,OriginalImgPath) 
        WHERE ID=@ID;

		IF(@DWBodyStyleId = -1)
		BEGIN
			UPDATE TC_DealerModels
			SET DWBodyStyleId = NULL, ModifiedDate= GETDATE() 
			WHERE ID=@ID;
		END

	END
END 
 