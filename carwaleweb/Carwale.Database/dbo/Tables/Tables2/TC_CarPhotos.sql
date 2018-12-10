CREATE TABLE [dbo].[TC_CarPhotos] (
    [Id]                     NUMERIC (18)  IDENTITY (1, 1) NOT NULL,
    [StockId]                NUMERIC (18)  NOT NULL,
    [ImageUrlFull]           VARCHAR (250) NOT NULL,
    [ImageUrlThumb]          VARCHAR (250) NULL,
    [ImageUrlThumbSmall]     VARCHAR (250) NOT NULL,
    [IsMain]                 BIT           NOT NULL,
    [IsActive]               BIT           CONSTRAINT [DF_TC_CarPhotos_IsActive] DEFAULT ((1)) NOT NULL,
    [DirectoryPath]          VARCHAR (200) NULL,
    [IsReplicated]           BIT           CONSTRAINT [DF__TC_CarPho__IsRep__5372EBE3] DEFAULT ((0)) NULL,
    [HostURL]                VARCHAR (250) CONSTRAINT [DF__TC_CarPho__HostU__75C803E7] DEFAULT ('img.carwale.com') NULL,
    [EntryDate]              DATETIME      CONSTRAINT [DF__TC_CarPho__Entry__2C241498] DEFAULT (getdate()) NULL,
    [ModifiedBy]             INT           NULL,
    [ModifiedDate]           DATETIME      NULL,
    [IsSellerInq]            BIT           CONSTRAINT [DF__TC_CarPho__IsSel__36627740] DEFAULT ((0)) NULL,
    [StatusId]               SMALLINT      NULL,
    [OrigFileName]           VARCHAR (250) NULL,
    [ImageUrlMedium]         VARCHAR (250) NULL,
    [TC_ActionApplicationId] INT           NULL,
    [OriginalImgPath]        VARCHAR (250) NULL,
    CONSTRAINT [PK_TC_CarPhotos] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [ix_TC_CarPhotos__StockId__IsMain__IsActive]
    ON [dbo].[TC_CarPhotos]([StockId] ASC, [IsMain] ASC, [IsActive] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_TC_CarPhotos_StatusId]
    ON [dbo].[TC_CarPhotos]([StatusId] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_TC_CarPhotosIsMainIsActive]
    ON [dbo].[TC_CarPhotos]([IsMain] ASC, [IsActive] ASC)
    INCLUDE([StockId], [ImageUrlThumbSmall], [DirectoryPath], [HostURL]);


GO
-- =============================================
-- Author:		Surendra
-- Create date: 25 Aug 2011
-- Description:	-- Purpose of this Trigger is to reflect changes immidiately in carwale(CarPhotos) from trading car application.
-- Modified By : Surendra on 26 Oct 2012 Desc : varaible @ImageUrlFull lenth increased from 50 to 150
-- Modified by : Manish on 28-04-2014 added  WITH (NOLOCK) keyword
-- Modified By Vivek Gupta on 17-12-2015, Removed check of ImageUrlFull and added check of TC_CarPhotoId
-- Modified By Chetan Navin on 21st Dec 2015 (Added update of original path,ImageUrlFull,ImageUrlThumb,ImageUrlThumbSmall,ImageUrlMedium)
-- =============================================
CREATE TRIGGER [dbo].[Trg_TC_CarPhotos] 
   ON  [dbo].[TC_CarPhotos]
   AFTER  UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;
	Declare @StockId NUMERIC, @IsMain Bit, @IsActive Bit, @BranchId NUMERIC
	DECLARE @InquiryId NUMERIC, @ImageUrlFull VARCHAR(150),@OriginalPath VARCHAR(150),
	@ImageUrlThumb VARCHAR(250),@ImageUrlThumbSmall VARCHAR(250),@ImageUrlMedium VARCHAR(250),@TC_CarPhotoId INT

	SELECT TOP 1 @StockId = StockId , @IsMain = IsMain, @IsActive = IsActive, @ImageUrlFull=ImageUrlFull, @ImageUrlThumb = ImageUrlThumb,
				@ImageUrlThumbSmall = ImageUrlThumbSmall,@ImageUrlMedium = ImageUrlMedium,
				@OriginalPath = OriginalImgPath,@TC_CarPhotoId = Id from Inserted
	
	SELECT @BranchId = BranchId FROM TC_Stock WHERE Id = @StockId --Added by Deepak on 26th Aug - CTE migration
	SELECT @InquiryId = Id FROM SellInquiries Si WITH (NOLOCK) WHERE Si.Tc_StockId = @StockId AND SourceId = 2 -- 2 for Autobiz
		AND DealerId = @BranchId --Added by Deepak on 26th Aug - CTE migration
	
	
	IF (@InquiryId IS NOT NULL)
	BEGIN
		IF(@IsMain=1)
		BEGIN
		
			-- need to reset IsMain=o if current image is going to main image
			UPDATE CarPhotos SET IsMain=0 WHERE InquiryId = @InquiryId AND IsDealer = 1 
			
			UPDATE CP SET CP.IsMain = @IsMain, CP.IsActive = @IsActive,OriginalImgPath = @OriginalPath,ImageUrlFull = @ImageUrlFull,
			ImageUrlThumb = @ImageUrlThumb,ImageUrlThumbSmall = @ImageUrlThumbSmall,ImageUrlMedium = @ImageUrlMedium 
			FROM CarPhotos CP WITH (NOLOCK) WHERE CP.InquiryId = @InquiryId AND CP.IsDealer = 1 AND  CP.TC_CarPhotoId = @TC_CarPhotoId AND CP.IsActive = 1
			--AND CP.ImageUrlFull = @ImageUrlFull
		END
		ELSE
		BEGIN
			UPDATE CP SET CP.IsActive = @IsActive,OriginalImgPath = @OriginalPath,ImageUrlFull = @ImageUrlFull,
			ImageUrlThumb = @ImageUrlThumb,ImageUrlThumbSmall = @ImageUrlThumbSmall,ImageUrlMedium = @ImageUrlMedium 
			FROM CarPhotos CP WITH (NOLOCK) WHERE CP.InquiryId = @InquiryId AND CP.IsDealer = 1 AND  CP.TC_CarPhotoId = @TC_CarPhotoId AND CP.IsActive = 1
			--AND CP.ImageUrlFull = @ImageUrlFull
		END
	END
	
END







