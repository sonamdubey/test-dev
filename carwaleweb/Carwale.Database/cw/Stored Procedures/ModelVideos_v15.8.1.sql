IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[cw].[ModelVideos_v15]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [cw].[ModelVideos_v15]
GO

	--created By:Prashant vishe On 20 May 2013
--Modified By:Prashant vishe On 20 Auguest 2013  -- added insertion/updation of new column VideoTitle
--Modified By : Vinay Kumar Prajapati 19th November 2014 -- insert and update new  colomn TumbnailhostURL and TumbnailDirPath 
                         
                        
CREATE PROCEDURE [cw].[ModelVideos_v15.8.1]                 
 @ModelId int,                
 @VideoSrc varchar(500),        
 @IsActive bit,        
 @isUpdate bit ,  
 @VideoTitle varchar(500) ,
 @ThumbnailHostURL varchar(50),
 @ThumbnailDirPath varchar(20),
 @Id int ,
 @Status TINYINT = -1 OUTPUT ,
 @ReturnId INT  =-1  OUTPUT 
            
AS 
DECLARE @ThumbnailImage varchar(20)                         
BEGIN                          
	 -- SET NOCOUNT ON added to prevent extra result sets from 	                      
	 -- interfering with SELECT statements.               
     SET NOCOUNT ON
	    
	 IF  @isUpdate <> 0         
		  BEGIN 
		    SET @ThumbnailImage= CAST(@Id AS VARCHAR(15))+'.jpg' 
			          
			UPDATE Con_ModelVideos SET VideoSrc=@VideoSrc,IsActive=@IsActive ,VideoTitle=@VideoTitle,ModelId=@ModelId, OriginalImgPath = @ThumbnailDirPath + '/' + @ThumbnailImage, ThumbnailImage=@ThumbnailImage WHERE Id=@Id  
		    SET @ReturnId= @Id
			SET @Status= 1 -- using for update 
		  END            
	 ELSE            
		  BEGIN            
			 INSERT INTO Con_ModelVideos(ModelId,VideoSrc,isActive,Entrydate,VideoTitle,ThumbnailHostURL) values (@ModelId,@VideoSrc,@IsActive,GETDATE(),@VideoTitle,@ThumbnailHostURL)            
		      SET @ReturnId = SCOPE_IDENTITY()
			  SET @ThumbnailImage = CAST(@ReturnId AS VARCHAR(15)) +'.jpg'
			  UPDATE Con_ModelVideos SET OriginalImgPath= @ThumbnailDirPath + '/' + @ThumbnailImage WHERE Id=@ReturnId  		   
			  SET @Status= 2 -- using for insert 		  
		  END                 
END 


