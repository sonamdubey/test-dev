IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_SaveCarPhotos]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_SaveCarPhotos]
GO

	-- =============================================
-- Author:        Ruchira Patil
-- Create date: 16th Dec 2014
-- Description:    To save AbSure Car photos against the carId
-- Modified By: Ashwini Dhamankar on 16-01-2015
-- Description: Assigned NULL values to @ImageUrlOriginal and @ImageCaption
-- Modified By : Ruchira Patil on 8th June 2015 (added parameter userid)
-- Modified By : Ashwini Dhamankar on June 2015 (added parameters ImageTagType,ImageTagId)
-- Modified By : Kartik Rathod And Nilima More on 17th July 2015 (Set Front View as Default Image of every certificate.)
-- Modified By : Kartik Rathod on 30th July 2015 (to save data in Timestamp )
-- Modified By : Ruchira Patil on 4th Aug 2015 (updated the RCImagePending=0 when rc gets uploaded )
-- Modified By : vinay kumar prajapati 29th Sept 2014.....save image url ( added @AspectRatio parameter new technique to save and replicate data in RabbitMQ (Directory Path will be blank)) 
-- =============================================
CREATE PROCEDURE [dbo].[AbSure_SaveCarPhotos]
    @AbSure_CarDetailsId       INT,
    @ImageUrlOriginal          VARCHAR(250) = NULL,
    @ImageUrlXL                   VARCHAR(250) = NULL,
    @ImageUrlLarge             VARCHAR(250) = NULL,
    @ImageUrlThumb             VARCHAR(250) = NULL,
    @ImageUrlSmall             VARCHAR(250) = NULL,
    @DirectoryPath             VARCHAR(200) = NULL,
    @ImageCaption              VARCHAR(1000)= NULL,
    @IsMain                    BIT = 0,       
    @IsChassisImage            BIT = 0,
    @HostUrl                   VARCHAR(100),
    @UserId                    INT = NULL,
    @AbSure_CarPhotosId        INT = NULL OUTPUT,   
    @ImageTagType              INT = NULL,
    @ImageTagId                INT = NULL,
    @Timestamp                   DATETIME = NULL,
    @AspectRatio               DECIMAL(5,2) = NULL
AS
BEGIN

     DECLARE @UrlOriginal     VARCHAR(100)
    
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    SET NOCOUNT ON;
    
    -- Modified By : Kartik Rathod And Nilima More on 17th July 2015 (set IsMain Condition true for Front View Photo)
    /**************************************************************************************************/
   
   /*SELECT TOP 1 AbSure_CarSurveyorMappingId FROM AbSure_CarSurveyorMapping WHERE TC_UserId = @UserId
   
   IF @@ROWCOUNT > 0
    BEGIN
           IF @ImageTagType=2
                BEGIN
                    SELECT @ImageTagId = TagId,@ImageCaption = ImageDescription
                    FROM AbSure_ImageTagsMapping
                    WHERE NewTagId = @ImageTagId AND ImageTypeId = 2
                    
                END
    END
   */

    IF ((@ImageTagId=2 AND @ImageTagType=2) OR @ImageCaption LIKE '%Front View%')
        SET @IsMain=1
    ELSE
        SET @IsMain=0    
    /**************************************************************************************************/
    SET @UrlOriginal   =  @DirectoryPath + @ImageUrlOriginal

    INSERT INTO AbSure_CarPhotos (AbSure_CarDetailsId,ImageUrlOriginal,ImageUrlLarge,ImageUrlThumb,ImageUrlSmall,DirectoryPath,ImageCaption,
    IsMain,IsChassisImage,HostUrl,EntryDate,TC_UserId, ImageUrlExtraLarge,ImageTagType,ImageTagId,Timestamp, OriginalImgPath,AspectRatio,IsActive,StatusId)
    VALUES(@AbSure_CarDetailsId,@ImageUrlOriginal,@UrlOriginal,@UrlOriginal,@UrlOriginal,'',@ImageCaption,@IsMain,@IsChassisImage,
    @HostUrl,GETDATE(),@UserId, @UrlOriginal,@ImageTagType,@ImageTagId,@Timestamp,@UrlOriginal,@AspectRatio,1,1)

    ---Return the ID
    SET @AbSure_CarPhotosId = SCOPE_IDENTITY()

    IF (((SELECT RCImagePending FROM AbSure_CarDetails WITH(NOLOCK) WHERE ID = @AbSure_CarDetailsId) = 1) AND ((@ImageTagId=1 AND @ImageTagType=2) OR @ImageCaption LIKE '%RC%'))
        UPDATE AbSure_CarDetails SET RCImagePending = 0 WHERE id = @AbSure_CarDetailsId
END

