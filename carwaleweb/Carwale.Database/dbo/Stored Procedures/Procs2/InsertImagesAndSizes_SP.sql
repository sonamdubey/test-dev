IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[InsertImagesAndSizes_SP]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[InsertImagesAndSizes_SP]
GO
	CREATE Procedure [dbo].[InsertImagesAndSizes_SP]        
(        
 @BasicId Numeric,         
 @Caption VarChar(250),        
 @CategoryId Numeric,  
 @RTImageId Int,         
 @ID Numeric Output         
)        
As         
Begin         
 Set NoCount ON        
 --Begin Transaction        
 Insert Into Con_EditCms_Images         
 (        
  BasicId,        
  ImageCategoryId,        
  Caption,        
  IsActive,        
  LastUpdatedTime,        
  LastUpdatedBy,  
  RTImageId    
 )        
 Values         
 (        
  @BasicId,        
  @CategoryId,        
  @Caption,        
  1,        
  GETDATE(),        
  13,  
  @RTImageId    
 )        
         
 Set @ID = SCOPE_IDENTITY()        
  
 Update Con_RoadTestAlbum Set IsUpdated = 1 Where Id = @RTImageId         
         
 ----If Transacation fails, stop execution of procedure, return error code and Rollback Transaction        
        
 --If @@ERROR<>0 OR @@RowCount = 0        
 --Begin        
 --Rollback Transaction        
 -- --return value        
 -- Set @ID = -1        
 -- Return        
 --End        
 --Else        
 --Begin        
 -- Insert Into Con_EditCms_ImageSizes (ImageId, ImageHeight, ImageWidth) Values ( @ID, @ImageHeight, @ImageWidth)        
 --End        
 --Commit Transaction         
End 