IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[AbSure_UpdateCarPhoto]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[AbSure_UpdateCarPhoto]
GO

	




-- ===========================================================================
-- Author:        Ruchira Patil
-- Create date: 5th Jun 2015
-- Description:    To Update AbSure CarPhoto tags
-- Modified By : Kartik Rathod And Nilima More on 17th July 2015
-- Description: Set Front View as Default Image of every certificate
-- ============================================================================
CREATE PROCEDURE [dbo].[AbSure_UpdateCarPhoto] 
    @Id                INT,
    @ImageCaption    VARCHAR(1000),
    @ModifiedBy        INT,
    @ImgTagId        INT,
    @Status            BIT OUTPUT
AS
BEGIN
    -- SET NOCOUNT ON added to prevent extra result sets from
    -- interfering with SELECT statements.
    
    SET NOCOUNT ON;
    SET @Status = 0
    
    DECLARE @ImageTagType INT
    DECLARE @Absure_CardetailsId INT
    SELECT @Absure_CardetailsId = Absure_CardetailsId, @ImageTagType = ImageTagType FROM AbSure_CarPhotos WHERE AbSure_CarPhotosId = @Id

    --Temporary arrangement for converting id for carimaages. As desktop and mobile have different ids.
    /*IF @ImageTagType=2
        BEGIN
            SELECT @ImgTagId = TagId,@ImageCaption = ImageDescription
            FROM AbSure_ImageTagsMapping
            WHERE NewTagId = @ImgTagId AND ImageTypeId = 2
            
        END
    */    
  IF (@ImageCaption LIKE '%Front View%' OR @ImgTagId=2)
   BEGIN
        -- to make IsMain of all photos 0

        UPDATE AbSure_CarPhotos SET IsMain = 0 WHERE AbSure_CarDetailsId = @Absure_CardetailsId  
                
        UPDATE    AbSure_CarPhotos 
        SET        ImageCaption = @ImageCaption,
                ModifiedBy = @ModifiedBy,
                IsMain=1,
                ImageTagId = @ImgTagId, 
                ModifiedDate = GETDATE() 
        OUTPUT inserted.AbSure_CarPhotosId
        WHERE    AbSure_CarPhotosId = @Id
    END    
    ELSE
    BEGIN
         UPDATE    AbSure_CarPhotos 
        SET        ImageCaption = @ImageCaption,
                ModifiedBy = @ModifiedBy,
                IsMain=0,
                ImageTagId = @ImgTagId, 
                ModifiedDate = GETDATE() 
        OUTPUT inserted.AbSure_CarPhotosId
        WHERE    AbSure_CarPhotosId = @Id
   END

    IF @@ROWCOUNT = 1
        SET @Status = 1
END




---------------------------------------------------------------------------------------------


