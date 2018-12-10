IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[Con_EditCms_Images_Save]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[Con_EditCms_Images_Save]
GO

	CREATE PROCEDURE [dbo].[Con_EditCms_Images_Save]                
@ID NUMERIC(18,0),                
@BasicId NUMERIC(18,0),                
@ImageCategoryId NUMERIC(18,0),                
@Caption VARCHAR(250),             
@LastUpdatedBy NUMERIC(18,0),        
@HostUrl VARCHAR(250),               
@IsReplicated BIT,      
@ImageId NUMERIC(18,0) OUT,    
@MakeId Int,    
@ModelId Int,  
@ImageName VarChar(100),  
@IsMainImage Bit,
@HasCustomImage Bit,
@ImagePath VarChar(50)                
AS                
BEGIN                
                
	IF @ID = -1                 
	BEGIN            
		DECLARE @LastSequence AS INT		        
		SELECT @LastSequence = Max(Sequence) FROM Con_EditCms_Images WHERE BasicId = @BasicId And IsActive = 1
		SET @LastSequence = ISNULL(@LastSequence,0)		       
		Declare @Cnt Int = 0
		If ( @IsMainImage = 1)
			Begin			
				Select @Cnt = COUNT(*)	From Con_EditCms_Images Where BasicId = @BasicId And IsMainImage = 1			
				If(@Cnt > 0)
					Begin					
						Select @ImageId = ID from Con_EditCms_Images Where BasicId = @BasicId And IsMainImage = 1					
						Exec Con_EditCms_Images_Update @BasicId, @ImageCategoryId, @Caption, @LastUpdatedBy, @HostUrl, 
														 @IsReplicated, @ImageId, @MakeId, @ModelId, @ImageName, @IsMainImage, @HasCustomImage, @ImagePath  
					End
			End
		If ( @Cnt = 0 )
			Begin
				INSERT INTO Con_EditCms_Images 
					(BasicId, ImageCategoryId, Caption, LastUpdatedTime, LastUpdatedBy, Sequence, HostUrl, MakeId, ModelId, IsMainImage, HasCustomImg)                
				VALUES
					(@BasicId, @ImageCategoryId, @Caption, GETDATE(), @LastUpdatedBy, (@LastSequence + 1), @HostUrl, @MakeId, @ModelId, @IsMainImage, @HasCustomImage)					             
				SET @ImageId = SCOPE_IDENTITY()
				Update Con_EditCms_Images 
				Set ImageName = @ImageName + '-' + CONVERT(VarChar, @ImageId) + '.jpg', 
					ImagePathThumbnail = ( Case When IsMainImage = 1 Then @ImagePath + @ImageName + '-' + CONVERT(VarChar, @ImageId) + '_m.jpg' Else @ImagePath + 't/' + @ImageName + '-' + CONVERT(VarChar, @ImageId) + '.jpg' End ),
					ImagePathLarge = ( Case When IsMainImage = 1 Then @ImagePath + @ImageName + '-' + CONVERT(VarChar, @ImageId) + '_l.jpg' Else @ImagePath + 'l/' + @ImageName + '-' + CONVERT(VarChar, @ImageId) + '.jpg' End ),
					ImagePathOriginal = ( Case When IsMainImage = 1 Then @ImagePath + @ImageName + '-' + CONVERT(VarChar, @ImageId) + '_ol.jpg' Else @ImagePath + 'ol/' + @ImageName + '-' + CONVERT(VarChar, @ImageId) + '.jpg' End ),
					ImagePathCustom = ( Case When HasCustomImg = 1 Then Case When IsMainImage = 1 Then @ImagePath + @ImageName + '-' + CONVERT(VarChar, @ImageId) + '_c.jpg' Else @ImagePath + 'c/' + @ImageName + '-' + CONVERT(VarChar, @ImageId) + '.jpg' End Else NULL End )
				Where Id = @ImageId	
			End		
	END                
 ELSE IF @ID <> -1
	 BEGIN	          
		Set @ImageId = @ID         
		Exec Con_EditCms_Images_Update @BasicId, @ImageCategoryId, @Caption, @LastUpdatedBy, @HostUrl, 
													 @IsReplicated, @ImageId, @MakeId, @ModelId, @ImageName, @IsMainImage, 
													 @HasCustomImage, @ImagePath  	                  
		--SET @ImageId = @ID 
 END
END