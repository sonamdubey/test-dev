CREATE TABLE [dbo].[CarPhotos] (
    [Id]                 NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [InquiryId]          NUMERIC (18)  NOT NULL,
    [ImageUrlFull]       VARCHAR (250) NULL,
    [ImageUrlThumb]      VARCHAR (250) NULL,
    [ImageUrlThumbSmall] VARCHAR (250) NULL,
    [Description]        VARCHAR (200) NULL,
    [IsDealer]           BIT           NOT NULL,
    [IsMain]             BIT           NOT NULL,
    [IsActive]           BIT           CONSTRAINT [DF_CarPhotos_IsActive] DEFAULT ((1)) NOT NULL,
    [IsApproved]         BIT           CONSTRAINT [DF_CarPhotos_IsApproved] DEFAULT ((0)) NOT NULL,
    [DirectoryPath]      VARCHAR (200) NULL,
    [IsReplicated]       BIT           CONSTRAINT [DF__CarPhotos__IsRep__37CAD16E] DEFAULT ((0)) NULL,
    [HostURL]            VARCHAR (100) CONSTRAINT [DF_CarPhotos_HostURL] DEFAULT ('http://imgd1.aeplcdn.com/') NULL,
    [Entrydate]          DATETIME      DEFAULT (getdate()) NULL,
    [TC_CarPhotoId]      BIGINT        NULL,
    [IsScanned]          BIT           DEFAULT ((0)) NULL,
    [StatusId]           SMALLINT      CONSTRAINT [DF_CarPhotos_StatusId] DEFAULT ((1)) NULL,
    [ImageUrlMedium]     VARCHAR (250) NULL,
    [OriginalImgPath]    VARCHAR (300) NULL,
    CONSTRAINT [PK_CarPhotos] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [Idx_CP_IM_IA_IAP]
    ON [dbo].[CarPhotos]([IsMain] ASC, [IsActive] ASC, [IsApproved] ASC)
    INCLUDE([InquiryId], [ImageUrlFull], [IsDealer]);


GO
CREATE NONCLUSTERED INDEX [IX_CarPhotos_InquiryId_IsDealer]
    ON [dbo].[CarPhotos]([InquiryId] ASC, [IsDealer] ASC)
    INCLUDE([IsActive], [IsMain]);


GO
CREATE NONCLUSTERED INDEX [ix_CarPhotos__ImageUrlFull]
    ON [dbo].[CarPhotos]([ImageUrlFull] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_CarPhotos]
    ON [dbo].[CarPhotos]([TC_CarPhotoId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_CarPhotos__IsActive__IsReplicated__HostURL]
    ON [dbo].[CarPhotos]([IsActive] ASC, [IsReplicated] ASC, [HostURL] ASC)
    INCLUDE([Id], [ImageUrlFull], [ImageUrlThumb], [ImageUrlThumbSmall], [DirectoryPath]);


GO
--Created by: Manish 26-11-2012 For updating last updated date 
-- Avishkar Modified 28 nov 2012 to use deleted table  
--Modified By  Reshma Shetty 16-jan-13 Photo count has been divided into discrete ranges for score calculation. 
-- Modified By Manish on 20-05-2014 for sync of ImageUrlMedium from CarPhotos to Livelistings table.
-- Added column OriginalImgPath for new image processing by Navead on 08-Aug-2015
----Modified by Sahil Sharma to insert log for every individuals image in CustStockPhotoLog table
CREATE TRIGGER [dbo].[Tr_carphoto_insert]   
ON [dbo].[CarPhotos]   
FOR INSERT   
AS   
    DECLARE @PhotoCount AS SMALLINT   
    DECLARE @InquiryId AS INT   
    DECLARE @IsDealer AS BIT   
    DECLARE @DirectoryPath AS VARCHAR(200)   
    DECLARE @HostUrl AS VARCHAR(100)   
    DECLARE @IsReplicated BIT   
    DECLARE @OLDPhotoCount AS SMALLINT   
    DECLARE @PHOTOMULT FLOAT=0.07408   
    DECLARE @NoOfRows AS INT   
    DECLARE @LoopIndex AS INT
    DECLARE @NextRowId AS INT
    DECLARE @CurRowId AS INT
    DECLARE @old_photo_grp AS INT
    DECLARE @photo_grp AS INT
    DECLARE @InquiryIdInd AS NUMERIC
 
    SET @NoOfRows=@@ROWCOUNT   
    SET @NoOfRows = (SELECT Count(*)   
                     FROM   inserted)   
  
  --  PRINT @NoOfRows   
  					

   
    IF @NoOfRows = 1   
      BEGIN   
        --  PRINT 'Single Row'   

          SELECT TOP 1 @InquiryId = inquiryid,   
                       @IsDealer = isdealer,   
                       @HostUrl = hosturl,   
                       @IsReplicated = isreplicated
		   FROM   inserted   
          WHERE  isdealer = 1   
  
          -- SELECT PHOTO COUNT     
          SELECT @PhotoCount = Count(*)   
          FROM   carphotos  WITH(NOLOCK)
          WHERE  inquiryid = @InquiryId   
                 AND isdealer = @IsDealer   
                 AND isactive = 1   
                 AND isapproved = 1   
  
          ---------------add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring-------     
          -- SELECT OLD PHOTO COUNT FROM LIVELISTINGS  
             
          SELECT @OLDPhotoCount = (SELECT count(id)   
                                          FROM   inserted as i   
                                          WHERE  inquiryid = @InquiryId   
                                                 AND isdealer =@IsDealer)   
          
          -- Avishkar Modified 28 nov 2012 to use deleted table 
          --SELECT @OLDPhotoCount = (SELECT count(id)   
          --                                FROM   deleted   
          --                                WHERE  inquiryid = @InquiryId   
          --                                       AND isdealer =@IsDealer)   
  
  
          ------------------------------------------------------------------------------------------------------     
       --    print @oldphotocount  
        --   print @photocount  
            
            
          -- UPDATE PHOTO COUNT and score  
           --Modified By  Reshma Shetty 16-jan-13 Photo count has been divided into discrete ranges for score calculation.
          SET @photo_grp=CASE 
                            WHEN @PhotoCount IS NULL THEN 1
							WHEN @PhotoCount=0 THEN 1
							WHEN @PhotoCount BETWEEN 1 AND 5 THEN 2
							WHEN @PhotoCount > 5 THEN 3
							END
		  SET @old_photo_grp=CASE 
                            WHEN @OLDPhotoCount IS NULL THEN 1
							WHEN @OLDPhotoCount=0 THEN 1
							WHEN @OLDPhotoCount BETWEEN 1 AND 5 THEN 2
							WHEN @OLDPhotoCount > 5 THEN 3
							
							END				
                   
               UPDATE livelistings   
                  SET    photocount = @PhotoCount,   
                  score = Round(( ISNULL(score,0) - @PHOTOMULT * @old_photo_grp   
                                 +   
                                               @PHOTOMULT * @photo_grp   
                                                             ), 6)  
                  WHERE  inquiryid = @InquiryId   
                   AND sellertype = ( CASE   
                       WHEN @IsDealer = 1 THEN @IsDealer   
                                      ELSE 2   
                                    END )   

			 --Added by Sahil Sharma to insert log for every individuals image in CustStockPhotoLog table
			 IF(@IsDealer IS NULL) --individual
				BEGIN
					SELECT TOP 1 @InquiryIdInd = inquiryid FROM inserted WHERE isdealer = 0
					INSERT INTO CustStockPhotoLog(InquiryId,EntryDate)
					VALUES (@InquiryIdInd,GETDATE())
				END
			--End Added by Sahil Sharma to insert log for every individuals image in CustStockPhotoLog table

           --print @oldphotocount  
           --print @photocount  
               
             -------------------------update of last updated date------------------------  
              if @oldphotocount <> 0  
              --update SellInquiries set lastupdated=getdate()  
              --where id=@inquiryid  
               UPDATE sellinquiries set lastupdated=getdate()  
               WHERE  id = @InquiryId   
       -----------------------------------------------------------------------------------  
             
          IF @PhotoCount <> 0   
            -- IF PHOTOS AVAILABLE, UPDATE IMAGE PATH TO 'LiveListings' TABLE     
            BEGIN   
                -- UPDATE THE DIRECTORY PATH     
                UPDATE L   
                SET    FrontImagePath = DirectoryPath + ImageUrlThumbSmall,   
                       HostURL = @HostUrl,   
                       IsReplicated = @IsReplicated ,
					   ImageUrlMedium=DirectoryPath + C.ImageUrlMedium,   ----added by Manish on 20-05-2014
					   OriginalImgPath = C.OriginalImgPath			----added by Navead on 06-08-2015
                FROM   livelistings L   WITH(NOLOCK)
                       INNER JOIN carphotos C   WITH(NOLOCK)
                               ON L.inquiryid = C.inquiryid   
                                  AND C.isdealer = @IsDealer   
                                  AND L.sellertype = ( CASE   
                                                         WHEN @IsDealer = 1 THEN   
                  @IsDealer   
                                                         ELSE 2   
                                                       END )   
                WHERE  C.ismain = 1   
                       AND C.inquiryid = @InquiryId   
                       AND IsActive = 1   
                       AND IsApproved = 1   
            END   
          ELSE   
            -- Photos not available, or in case last available photo deleted     
            BEGIN   
                -- CLEAR THE DRECTORY PATH     
                UPDATE livelistings   
                SET    frontimagepath = '',   
                       isreplicated = 0,   
                       hosturl = NULL ,
					   ImageUrlMedium  ='',     ----added by Manish on 20-05-2014
					   OriginalImgPath =''	---added by Navead on 06/08/2015
                WHERE  inquiryid = @InquiryId   
                       AND sellertype = ( CASE   
                                            WHEN @IsDealer = 1 THEN @IsDealer   
                                            ELSE 2   
                                          END )   
            END   
      END   
    ELSE IF @NoOfRows > 1   
      BEGIN   
         -- PRINT 'Multiple Row'   
  
          --else iterate through the rows and update the data accordingly   
          --initilialize the values   
          SET @LoopIndex = 1   
  
          WHILE @NoOfRows >= @LoopIndex   
            BEGIN   
                IF( @LoopIndex > 1 )   
                  BEGIN   
                      SELECT @NextRowId = Min(id)   
                      FROM   inserted   
                      WHERE  id > @CurRowId   
                  END   
                ELSE   
                  BEGIN   
                      SELECT @NextRowId = Min(id)   
                      FROM   inserted   
                  END   
  
                SELECT TOP 1 @InquiryId = inquiryid,   
                             @IsDealer = isdealer,   
                             @HostUrl = hosturl,   
                             @IsReplicated = isreplicated 
				FROM   inserted AS I  
                WHERE  isdealer = 1  
                AND  I.ID=@NEXTROWID  
  
                -- SELECT PHOTO COUNT     
                SELECT @PhotoCount = Count(*)   
                FROM   carphotos  WITH(NOLOCK) 
                WHERE  inquiryid = @InquiryId   
                       AND isdealer = @IsDealer   
                       AND isactive = 1   
                       AND isapproved = 1   
  
           --------------------add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring-------     
                 SELECT @OLDPhotoCount = (SELECT count(id)   
                                          FROM   inserted as i   
                                          WHERE  inquiryid = @InquiryId   
                                                 AND isdealer =@IsDealer)  
                 -- Avishkar 28-11 added to use deleted table                                
                 --SELECT @OLDPhotoCount = (SELECT count(id)   
                 --                         FROM   deleted   
                 --                         WHERE  inquiryid = @InquiryId   
                 --                                AND isdealer =@IsDealer)    
  
                ------------------------------------------------------------------------------------------------------     
                -- UPDATE PHOTO COUNT and HostURL  
                  --Modified By  Reshma Shetty 16-jan-13 Photo count has been divided into discrete ranges for score calculation.
				  SET @photo_grp=CASE 
									WHEN @PhotoCount IS NULL THEN 1
									WHEN @PhotoCount=0 THEN 1
									WHEN @PhotoCount BETWEEN 1 AND 5 THEN 2
									WHEN @PhotoCount > 5 THEN 3
									END
				  SET @old_photo_grp=CASE 
									WHEN @OLDPhotoCount IS NULL THEN 1
									WHEN @OLDPhotoCount=0 THEN 1
									WHEN @OLDPhotoCount BETWEEN 1 AND 5 THEN 2
									WHEN @OLDPhotoCount > 5 THEN 3
									
									END				
		                
                   
                UPDATE livelistings   
                SET    photocount = @PhotoCount,   
                       score = Round(( score - @PHOTOMULT *  
                                               @old_photo_grp  
                                       +   
                                                     @PHOTOMULT *    
                                                     @photo_grp   
                                                                  ), 6)   
                ---add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring     
                WHERE  inquiryid = @InquiryId  
                       AND sellertype = ( CASE   
                                            WHEN @IsDealer = 1 THEN @IsDealer   
                                            ELSE 2   
                                          END )   
  
            
			--Added by Sahil Sharma to insert log for every individuals image in CustStockPhotoLog table	 
			 IF(@IsDealer IS NULL) --individual
				BEGIN
					SELECT TOP 1 @InquiryIdInd = inquiryid FROM inserted as I WHERE isdealer = 0 AND  I.ID=@NEXTROWID  
					INSERT INTO CustStockPhotoLog(InquiryId,EntryDate)
					VALUES (@InquiryIdInd,GETDATE())
				END
             --End: Added by Sahil Sharma to insert log for every individuals image in CustStockPhotoLog table

             -------------------------update of last updated date------------------------  
              if @oldphotocount <> 0  
    --update SellInquiries set lastupdated=getdate()  
              --where id=@inquiryid  
               UPDATE sellinquiries set lastupdated=getdate()  
               WHERE  id = @InquiryId   
                 
              -----------------------------------------------------------------------------  
            
            
            
                IF @PhotoCount <> 0   
                  -- IF PHOTOS AVAILABLE, UPDATE IMAGE PATH TO 'LiveListings' TABLE     
                  BEGIN   
                      -- UPDATE THE DIRECTORY PATH     
                      UPDATE L   
                      SET    FrontImagePath = DirectoryPath + ImageUrlThumbSmall,  
                             HostURL = @HostUrl,   
                             IsReplicated = @IsReplicated  ,
							 ImageUrlMedium= DirectoryPath + C.ImageUrlMedium,   ----added by Manish on 20-05-2014
							 OriginalImgPath = C.OriginalImgPath  ---added by Navead on 06/08/2015
                      FROM   livelistings L  WITH(NOLOCK) 
                             INNER JOIN carphotos C  WITH(NOLOCK) 
                                     ON L.inquiryid = C.inquiryid   
                                        AND C.isdealer = @IsDealer   
                                        AND L.sellertype = ( CASE   
                                                               WHEN   
                                            @IsDealer = 1   
                                                             THEN   
                                                               @IsDealer   
                                                               ELSE 2   
                                                             END )   
                      WHERE  C.ismain = 1   
                             AND C.inquiryid = @InquiryId   
                             AND IsActive = 1   
                             AND IsApproved = 1   
                  END   
                ELSE   
                  -- Photos not available, or in case last available photo deleted     
                  BEGIN   
                      -- CLEAR THE DRECTORY PATH     
                      UPDATE livelistings   
                      SET    frontimagepath = '',   
                             isreplicated = 0,   
                             hosturl = NULL ,
							 ImageUrlMedium='',    ----added by Manish on 20-05-2014
							 OriginalImgPath = ''   ---added by Navead on 06/08/2015
                      WHERE  inquiryid = @InquiryId   
                             AND sellertype = ( CASE   
                                                  WHEN @IsDealer = 1 THEN   
                                                  @IsDealer   
                                                  ELSE 2   
                                                END )   
                  END   
                SET @CurRowId =@NextRowId  
                SET @LoopIndex = @LoopIndex + 1   
            END   
      END

GO
-- Manish 16-11-2012 Modified for selecting used car score prametres  
-- Modified by Manish on 28-03-2014 adding with nolock in the table after getting blocking alert.
-- Modified by Manish on 21-05-2014 sync of ImgMediumUrl column from CarPhotos to Livelistings table.
-- Modified by Manish on 09-09-2014 added with (nolock) keyword in Update query
-- Added column OriginalImgPath for new image processing by Navead on 08-Aug-2015
----Modified by Sahil Sharma to insert log for every individuals image in CustStockPhotoLog table
CREATE TRIGGER   [dbo].[Tr_Upd_PhotoCount]            ON [dbo].[CarPhotos]   
FOR  UPDATE  
AS  
  
DECLARE @PhotoCount AS SMALLINT  
DECLARE @InquiryId AS NUMERIC  
DECLARE @IsDealer AS BIT  
Declare @DirectoryPath AS varchar(200)  
DECLARE @HostUrl AS VARCHAR(100)  
DECLARE @IsReplicated BIT  
DECLARE @OLDPhotoCount AS SMALLINT -------add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring  
DECLARE @PHOTOMULT  FLOAT=0.07408 
DECLARE @photo_grp SMALLINT
DECLARE @old_photo_grp SMALLINT


SELECT TOP 1 @InquiryId = InquiryId , @IsDealer = IsDealer,@HostUrl=HostUrl,@IsReplicated=IsReplicated from Inserted  
-- SELECT PHOTO COUNT  
SELECT @PhotoCount =count(*) FROM carphotos WITH (NOLOCK) WHERE inquiryid =@InquiryId and Isdealer = @IsDealer  
and isactive =1 and isapproved =1  
  
---------------add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring-------  
-- SELECT OLD PHOTO COUNT FROM LIVELISTINGS  
SELECT @OLDPhotoCount =PhotoCount FROM livelistings WITH (NOLOCK) WHERE inquiryid =@InquiryId   
and SellerType = (case when @IsDealer = 1 then @IsDealer else 2 end)  
------------------------------------------------------------------------------------------------------  
  
-- UPDATE PHOTO COUNT and HostURL  
--Modified By  Reshma Shetty 16-jan-13 Photo count has been divided into discrete ranges for score calculation.
SET @photo_grp=CASE 
				WHEN @PhotoCount IS NULL THEN 1
				WHEN @PhotoCount=0 THEN 1
				WHEN @PhotoCount BETWEEN 1 AND 5 THEN 2
				WHEN @PhotoCount > 5 THEN 3
				END
SET @old_photo_grp=CASE 
				WHEN @OLDPhotoCount IS NULL THEN 1
				WHEN @OLDPhotoCount=0 THEN 1
				WHEN @OLDPhotoCount BETWEEN 1 AND 5 THEN 2
				WHEN @OLDPhotoCount > 5 THEN 3
				
				END		
UPDATE livelistings SET PhotoCount = @PhotoCount,  
score=round((SCORE-@PHOTOMULT*@old_photo_grp+@PHOTOMULT*@photo_grp),6) ---add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring  
WHERE inquiryid = @InquiryId and SellerType = (case when @IsDealer = 1 then @IsDealer else 2 end)  
  
---------------add by manish(AE1665) on 16 nov 2012 for additional parameter used car scoring-------  
if update(isactive) and @isdealer=1 
UPDATE sellinquiries set lastupdated=getdate()
WHERE  id = @InquiryId 

---------------------------------------------------------------------------------------------------------

----Added by Sahil Sharma to insert log for every individuals image in CustStockPhotoLog table
IF(@IsDealer=0 AND (UPDATE(IsActive) OR UPDATE(IsApproved))) --individual
	BEGIN
		INSERT INTO CustStockPhotoLog(InquiryId,EntryDate)
		VALUES (@InquiryId,GETDATE())
	END
----End:Added by Sahil Sharma to insert log for every individuals image in CustStockPhotoLog table

if @PhotoCount <> 0 -- IF PHOTOS AVAILABLE, UPDATE IMAGE PATH TO 'LiveListings' TABLE  
begin  
 -- UPDATE THE DIRECTORY PATH  
 --Modified by Manish on 09-09-2014 added with (nolock) keyword in Update query
 UPDATE L SET FrontImagePath = DirectoryPath + ImageUrlThumbSmall, HostURL = @HostUrl, IsReplicated=@IsReplicated ,
              ImageUrlMedium=DirectoryPath + C.ImageUrlMedium , -- Added by Manish on 21-05-2014
			  OriginalImgPath= C.OriginalImgPath   --- Added by Navead on 06/08/2015
 FROM LiveListings L WITH(NOLOCK) INNER JOIN CarPhotos C WITH(NOLOCK)
 ON L.Inquiryid = C.InquiryId AND C.IsDealer = @IsDealer AND L.SellerType = (case when @IsDealer = 1 then @IsDealer else 2 end)  
 WHERE C.IsMain = 1 AND C.InquiryId = @InquiryId AND IsActive =1 and IsApproved = 1  
end  
else -- Photos not available, or in case last available photo deleted  
begin  
 -- CLEAR THE DRECTORY PATH  
 UPDATE LiveListings SET FrontImagePath = NULL, IsReplicated = 0, HostURL = NULL,
                           ImageUrlMedium=NULL, ---Added by Manish on 21-05-2014
						   OriginalImgPath=NULL ---Added by Navead on 06/08/2015
  Where InquiryId = @InquiryId and SellerType = (case when @IsDealer = 1 then @IsDealer else 2 end)  
end

GO
-- =============================================
-- Author:		<Reshma Shetty>
-- Create date: <04/09/2012>
-- Description:	<Sets the AlbumRating field in SellInquiries as NULL on update of ImageURLFull field or insertion into CarPhotos table>
-- =============================================
CREATE TRIGGER [dbo].[TR_ResetSIPhotoRating]
ON [dbo].[CarPhotos]
AFTER INSERT,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	DECLARE @Delete INT = NULL
	SELECT @Delete=InquiryID FROM Deleted
	
	IF(UPDATE(ImageURLFull) OR @Delete=NULL)
    UPDATE SellInquiries
    SET AlbumRating = NULL
    WHERE ID IN (SELECT InquiryId FROM Inserted WHERE IsDealer=1)

END

GO
DISABLE TRIGGER [dbo].[TR_ResetSIPhotoRating]
    ON [dbo].[CarPhotos];


GO
EXECUTE sp_addextendedproperty @name = N'MS_Description', @value = N'for approval', @level0type = N'SCHEMA', @level0name = N'dbo', @level1type = N'TABLE', @level1name = N'CarPhotos', @level2type = N'COLUMN', @level2name = N'IsApproved';

