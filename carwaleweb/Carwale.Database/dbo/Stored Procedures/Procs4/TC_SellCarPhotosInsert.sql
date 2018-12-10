IF EXISTS (
    SELECT * FROM sysobjects WHERE id = object_id(N'[dbo].[TC_SellCarPhotosInsert]') 
    AND xtype IN (N'P')
)
    DROP PROCEDURE [dbo].[TC_SellCarPhotosInsert]
GO

	-- Author		:	Surendra
-- Create date	:	09/10/2012 14:00 PM
-- Description	:	This SP used to maintain record of uploaded images in the Trading Cars Software       
--					image will get upload in three sizes i.e. 640x428|300x225|80x60 ,this record initially will inactive  
-- Modified By  :	Nilesh Utture on 29th October, 2012. Added REPLACE to @CarVersion 
-- Modified By : Tejashree Patil on 16 Jan 2013 at 5.30pm : Joined with TC_InquiriesLead instead of TC_Inquiries 
-- Modified By Vivek Gupta on 12-08-2015, added OriginalImgPath
-- =============================================    
CREATE PROCEDURE [dbo].[TC_SellCarPhotosInsert]    
 -- Add the parameters for the stored procedure here    
 @SellerInquiriesId  BIGINT,  
 @FileExtension VARCHAR(10),     
 @IsMain    BIT,  
 @DirPath   VARCHAR(200),   
 @PhotoId  BIGINT OUTPUT,
 @ImageName VARCHAR(100)  OUTPUT,
 @HostUrl VARCHAR(100)  
AS    
BEGIN    
 -- SET NOCOUNT ON added to prevent extra result sets from    
 -- interfering with SELECT statements.    
SET NOCOUNT ON;
DECLARE @ImageUrlFull  VARCHAR(100),
		@ImageUrlThumb  VARCHAR(100),
		@ImageUrlThumbSmall VARCHAR(100),
		@CarName VARCHAR(100), 
		@CarMake VARCHAR(100),
		@CarModel VARCHAR(100),
		@CarVersion VARCHAR(100),
		@MakeYear VARCHAR(20),

	    @OrgImgPath VARCHAR(250)
 
SELECT @MakeYear=CONVERT(CHAR(4), S.MakeYear, 120), @CarMake=REPLACE(V.Make,' ','-'),@CarModel=REPLACE(V.Model,' ','-'),@CarVersion=REPLACE(V.Version,' ','-')
FROM TC_SellerInquiries S WITH(NOLOCK)
	INNER JOIN TC_InquiriesLead I ON S.TC_InquiriesLeadId=I.TC_InquiriesLeadId -- Modified By : Tejashree Patil on 16 Jan 2013
	INNER JOIN vwMMV V ON S.CarVersionId=V.VersionId
WHERE S.TC_SellerInquiriesId = @SellerInquiriesId 

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

--SET @CarName= @MakeYear + '-' + @CarMake + '-' + @CarModel + '-' + @CarVersion 
--SET @CarName= REPLACE(@CarName,'--','')


SET @CarName= ISNULL(@MakeYear,'') +  '-' + ISNULL(@CarMake,'') + '-' + ISNULL(@CarModel,'') + '-' + ISNULL(@CarVersion,'')   --IsNull check : Added By Chetan Navin on 10 Aug 2015
SET @CarName= REPLACE(@CarName,'--','')
    
	 
 SET @OrgImgPath = @DirPath

 IF NOT EXISTS( SELECT TOP 1 Id FROM TC_SellCarPhotos WHERE TC_SellerInquiriesId = @SellerInquiriesId AND IsActive = 1 AND IsMain = 1 )    
 BEGIN    -- if main image is not set
	SET @IsMain = 1    
 END    
     
--inserting record with inactive status,later once image will save in appropriate folder need to activate
INSERT INTO TC_SellCarPhotos(TC_SellerInquiriesId, ImageUrlFull, ImageUrlThumb, ImageUrlThumbSmall, IsMain,DirectoryPath,HostUrl,IsActive,OriginalImgPath)    
VALUES(@SellerInquiriesId, @CarName, @CarName, @CarName, @IsMain,@DirPath,@HostUrl,1,@OrgImgPath)
--isactive cahnged t0 1 by chetan devv 
      
SET @PhotoId = SCOPE_IDENTITY()
SET @ImageName=@CarName + '-' + CAST(@PhotoId AS VARCHAR)
SET @ImageUrlFull=@ImageName + '-640x428' + @FileExtension  
SET @ImageUrlThumb=@ImageName + '-300x225' + @FileExtension 
SET @ImageUrlThumbSmall=@ImageName + '-80x60' + @FileExtension

SET @OrgImgPath = @DirPath + @ImageName + @FileExtension;

-- updating final image name
UPDATE TC_SellCarPhotos SET ImageUrlFull=@ImageUrlFull, ImageUrlThumb=@ImageUrlThumb, ImageUrlThumbSmall=@ImageUrlThumbSmall,
							OriginalImgPath = @OrgImgPath
WHERE Id=@PhotoId
  
END    









/****** Object:  StoredProcedure [dbo].[TC_StockDetailsPrint]    Script Date: 8/14/2015 11:51:30 AM ******/
SET ANSI_NULLS ON
