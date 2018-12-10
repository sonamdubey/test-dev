IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_CarPhotosInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_CarPhotosInsert]
GO

	-- Author		:	Surendra
-- Create date	:	09/10/2012 14:00 PM
-- Description	:	This SP used to maintain record of uploaded images in the Trading Cars Software       
--					image will get upload in three sizes i.e. 640x428|300x225|80x60 ,this record initially will inactive       
-- =============================================    
CREATE PROCEDURE [dbo].[TC_CarPhotosInsert]    
 -- Add the parameters for the stored procedure here    
 @StockId   BIGINT,
 @FileExtension VARCHAR(100),     
 @IsMain    BIT,  
 @DirPath   VARCHAR(200),   
 @PhotoId  BIGINT OUTPUT,
 @ImageName VARCHAR(100)  OUTPUT,
 @HostUrl VARCHAR(100),
 @BranchId BIGINT
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
SET NOCOUNT ON;

DECLARE 
	@ImageUrlFull  VARCHAR(100),
	@ImageUrlThumb  VARCHAR(100),
	@ImageUrlThumbSmall VARCHAR(100),
	@CarName VARCHAR(100),
	@InquiryId Numeric,
	@CarMake VARCHAR(100),
	@CarModel VARCHAR(100),
	@CarVersion VARCHAR(100),
	@MakeYear VARCHAR(20)
	
	IF NOT EXISTS(SELECT TOP 1 Id FROM TC_Stock WHERE Id=@StockId AND BranchId=@BranchId)
		BEGIN
			RETURN -1 
		END 
	ELSE
		BEGIN
			SELECT @MakeYear=CONVERT(CHAR(4), S.MakeYear, 120), @CarMake=REPLACE(V.Make,' ','-'),@CarModel=REPLACE(V.Model,' ','-'),  
			@CarVersion=REPLACE(V.Version,' ','-')  
			FROM TC_Stock S INNER JOIN vwMMV V ON S.VersionId=V.VersionId  
			WHERE S.Id=@StockId   
			  
			SELECT @CarMake=dbo.ReplaceCharacters(@CarMake,'a-z0-9-')  
			SELECT @CarModel=dbo.ReplaceCharacters(@CarModel,'a-z0-9-')  
			SELECT @CarVersion=dbo.ReplaceCharacters(@CarVersion,'a-z0-9-')  
			  
			/*  
			SET @CarModel=REPLACE(@CarModel,'+','')  
			SET @CarModel=REPLACE(@CarModel,'/','')  
			SET @CarModel=REPLACE(@CarModel,'\','')  
			SET @CarModel=REPLACE(@CarModel,'*','')  
			SET @CarModel=REPLACE(@CarModel,'(','')  
			SET @CarModel=REPLACE(@CarModel,')','')  
			SET @CarModel=REPLACE(@CarModel,'.','')  
			SET @CarModel=REPLACE(@CarModel,'--','-')  
			  
			SET @CarVersion=REPLACE(@CarVersion,'+','')  
			SET @CarVersion=REPLACE(@CarVersion,'/','')  
			SET @CarVersion=REPLACE(@CarVersion,'\','')  
			SET @CarVersion=REPLACE(@CarVersion,'*','')  
			SET @CarVersion=REPLACE(@CarVersion,'(','')  
			SET @CarVersion=REPLACE(@CarVersion,')','')  
			SET @CarVersion=REPLACE(@CarVersion,'.','')  
			SET @CarVersion=REPLACE(@CarVersion,'--','-')  
			*/  
			  
			SET @CarName= @MakeYear + '-' + @CarMake + '-' + @CarModel + '-' + @CarVersion   
			SET @CarName= REPLACE(@CarName,'--','')  
			   
			--SELECT @CarName=CONVERT(CHAR(4), S.MakeYear, 120) + '-' +REPLACE(V.Make,' ','-') + '-' + REPLACE(V.Model,' ','-') + '-' + REPLACE(V.Version,' ','-')  
			----CONVERT(CHAR(4), S.MakeYear, 120) + '-' + V.Make + '-' + V.Model + '-' + V.Version  
			--FROM TC_Stock S INNER JOIN vwMMV V ON S.VersionId=V.VersionId  
			--WHERE S.Id=@StockId   
			      
			 IF NOT EXISTS( SELECT TOP 1 Id FROM TC_CarPhotos WHERE StockId = @StockId AND IsActive = 1 AND IsMain = 1 )      
				 BEGIN    -- if main image is not set  
					SET @IsMain = 1      
				 END      
			       
			--inserting record with inactive status,later once image will save in appropriate folder need to activate  
			INSERT INTO TC_CarPhotos(StockId, ImageUrlFull, ImageUrlThumb, ImageUrlThumbSmall, IsMain,DirectoryPath,HostUrl,IsActive)      
			VALUES(@StockId, @CarName, @CarName, @CarName, @IsMain,@DirPath,@HostUrl,0)   
			        
			SET @PhotoId = SCOPE_IDENTITY()  
			SET @ImageName=@CarName + '-' + CAST(@PhotoId AS VARCHAR)  
			SET @ImageUrlFull=@ImageName + '-640x428' + @FileExtension    
			SET @ImageUrlThumb=@ImageName + '-300x225' + @FileExtension   
			SET @ImageUrlThumbSmall=@ImageName + '-80x60' + @FileExtension  
			  
			-- updating final image name  
			UPDATE TC_CarPhotos SET ImageUrlFull=@ImageUrlFull, ImageUrlThumb=@ImageUrlThumb, ImageUrlThumbSmall=@ImageUrlThumbSmall  
			WHERE Id=@PhotoId  
			RETURN 1
		END
END    


