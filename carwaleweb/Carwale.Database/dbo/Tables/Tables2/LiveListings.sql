CREATE TABLE [dbo].[LiveListings] (
    [ProfileId]             VARCHAR (50)    NOT NULL,
    [SellerType]            SMALLINT        NULL,
    [Seller]                VARCHAR (50)    NULL,
    [Inquiryid]             NUMERIC (18)    NULL,
    [MakeId]                NUMERIC (18)    NULL,
    [MakeName]              VARCHAR (100)   NULL,
    [ModelId]               NUMERIC (18)    NULL,
    [ModelName]             VARCHAR (100)   NULL,
    [VersionId]             NUMERIC (18)    NULL,
    [VersionName]           VARCHAR (100)   NULL,
    [StateId]               NUMERIC (18)    NULL,
    [StateName]             VARCHAR (100)   NULL,
    [CityId]                NUMERIC (18)    NULL,
    [CityName]              VARCHAR (100)   NULL,
    [AreaId]                NUMERIC (18)    NULL,
    [AreaName]              VARCHAR (100)   NULL,
    [Lattitude]             DECIMAL (18, 4) NULL,
    [Longitude]             DECIMAL (18, 4) NULL,
    [MakeYear]              DATETIME        NULL,
    [Price]                 NUMERIC (18)    NULL,
    [Kilometers]            NUMERIC (18)    NULL,
    [Color]                 VARCHAR (100)   NULL,
    [Comments]              VARCHAR (500)   NULL,
    [EntryDate]             DATETIME        NULL,
    [LastUpdated]           DATETIME        NULL,
    [PackageType]           SMALLINT        NULL,
    [ShowDetails]           BIT             NULL,
    [Priority]              SMALLINT        NULL,
    [PhotoCount]            SMALLINT        CONSTRAINT [DF__LiveListi__Photo__619229FE] DEFAULT ((0)) NOT NULL,
    [FrontImagePath]        VARCHAR (500)   CONSTRAINT [DF__LiveListi__Front__62864E37] DEFAULT ('') NULL,
    [CertificationId]       SMALLINT        NULL,
    [AdditionalFuel]        VARCHAR (50)    NULL,
    [IsReplicated]          BIT             CONSTRAINT [DF__LiveListi__IsRep__4DBA128D] DEFAULT ((1)) NULL,
    [HostURL]               VARCHAR (100)   CONSTRAINT [DF_LiveListings_HostURL] DEFAULT ('http://imgd1.aeplcdn.com/') NULL,
    [CalculatedEMI]         INT             CONSTRAINT [DF__LiveListi__Calcu__323CE0A1] DEFAULT (NULL) NULL,
    [Score]                 FLOAT (53)      NULL,
    [Responses]             SMALLINT        CONSTRAINT [DF_LiveListings_Responses] DEFAULT ((0)) NULL,
    [CertifiedLogoUrl]      VARCHAR (200)   NULL,
    [Owners]                VARCHAR (20)    NULL,
    [InsertionDate]         DATETIME        DEFAULT (getdate()) NULL,
    [DealerId]              INT             NULL,
    [IsPremium]             BIT             DEFAULT ((0)) NULL,
    [VideoCount]            TINYINT         DEFAULT ((0)) NULL,
    [OfferStartDate]        DATETIME        NULL,
    [OfferEnddate]          DATETIME        NULL,
    [SortScore]             FLOAT (53)      NULL,
    [ImageUrlMedium]        VARCHAR (250)   NULL,
    [RootId]                SMALLINT        NULL,
    [RootName]              VARCHAR (50)    NULL,
    [OwnerTypeId]           TINYINT         NULL,
    [UsedCarMasterColorsId] SMALLINT        NULL,
    [EMI]                   INT             NULL,
    [AbsureWarrantyType]    VARCHAR (100)   NULL,
    [AbsureScore]           INT             NULL,
    [SellerName]            VARCHAR (100)   NULL,
    [SellerContact]         VARCHAR (20)    NULL,
    [OriginalImgPath]       VARCHAR (300)   NULL,
    [SVScore]               FLOAT (53)      DEFAULT ((0)) NULL
);


GO
CREATE CLUSTERED INDEX [IX_Livelistings_InquiryId]
    ON [dbo].[LiveListings]([Inquiryid] ASC, [SellerType] ASC);


GO
CREATE NONCLUSTERED INDEX [Idx_LiveListings_ModelId]
    ON [dbo].[LiveListings]([ModelId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_LiveListings_PackageType]
    ON [dbo].[LiveListings]([PackageType] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_LiveListings__Price]
    ON [dbo].[LiveListings]([Price] ASC)
    INCLUDE([VersionId], [StateId]);


GO
CREATE NONCLUSTERED INDEX [IX_LiveListings_VersionId]
    ON [dbo].[LiveListings]([VersionId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_LiveListings__CityId]
    ON [dbo].[LiveListings]([CityId] ASC, [IsPremium] ASC, [DealerId] ASC)
    INCLUDE([VersionId]);


GO
CREATE NONCLUSTERED INDEX [IX_LiveListings_Lattitude_Longitude]
    ON [dbo].[LiveListings]([Lattitude] ASC, [Longitude] ASC)
    INCLUDE([Inquiryid], [ModelId], [Price]);


GO
CREATE NONCLUSTERED INDEX [IX_Livelistings_ProfileId]
    ON [dbo].[LiveListings]([ProfileId] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_LiveListings__MakeId]
    ON [dbo].[LiveListings]([MakeId] ASC)
    INCLUDE([MakeName]);


GO
CREATE NONCLUSTERED INDEX [ix_LiveListings__SellerType__ProfileId__Price]
    ON [dbo].[LiveListings]([SellerType] ASC, [ProfileId] ASC, [Price] ASC)
    INCLUDE([Inquiryid], [MakeName], [ModelId], [ModelName], [VersionName], [CityId], [MakeYear], [Kilometers], [Color], [PhotoCount], [FrontImagePath], [HostURL]);


GO
CREATE NONCLUSTERED INDEX [ix_LiveListings__ModelId__CityId__ProfileId]
    ON [dbo].[LiveListings]([ModelId] ASC, [CityId] ASC, [ProfileId] ASC)
    INCLUDE([MakeName], [ModelName], [VersionName], [AreaName], [MakeYear], [Price], [Kilometers], [PhotoCount], [FrontImagePath], [HostURL], [DealerId], [EMI], [AbsureScore]);


GO
CREATE NONCLUSTERED INDEX [ix_LiveListings__SellerType__DealerId__RootId]
    ON [dbo].[LiveListings]([SellerType] ASC, [DealerId] ASC, [RootId] ASC)
    INCLUDE([ProfileId], [Inquiryid], [MakeName], [ModelName], [VersionName], [CityName], [MakeYear], [Price], [Kilometers], [PhotoCount]);


GO
CREATE NONCLUSTERED INDEX [ix_LiveListings__SellerType__DealerId__PhotoCount]
    ON [dbo].[LiveListings]([SellerType] ASC, [DealerId] ASC, [PhotoCount] ASC);


GO
CREATE NONCLUSTERED INDEX [ix_LiveListings__ShowDetails]
    ON [dbo].[LiveListings]([ShowDetails] ASC)
    INCLUDE([ProfileId], [MakeName], [ModelId], [ModelName], [VersionId], [VersionName], [CityName], [MakeYear], [Price], [Kilometers], [EntryDate], [FrontImagePath], [HostURL]);


GO
CREATE NONCLUSTERED INDEX [ix_LiveListings__MakeId__CityId__Price]
    ON [dbo].[LiveListings]([MakeId] ASC, [CityId] ASC, [Price] ASC)
    INCLUDE([Inquiryid]);


GO
CREATE NONCLUSTERED INDEX [ix_LiveListings__CityId__RootId__Price]
    ON [dbo].[LiveListings]([CityId] ASC, [RootId] ASC, [Price] ASC)
    INCLUDE([Inquiryid]);


GO
CREATE NONCLUSTERED INDEX [ix_LiveListings__CityId__Price]
    ON [dbo].[LiveListings]([CityId] ASC, [Price] ASC)
    INCLUDE([Inquiryid], [VersionId]);


GO
CREATE NONCLUSTERED INDEX [ix_LiveListings__CityId__Lattitude__Longitude]
    ON [dbo].[LiveListings]([CityId] ASC, [Lattitude] ASC, [Longitude] ASC)
    INCLUDE([ProfileId], [Seller], [MakeName], [ModelId], [ModelName], [VersionName], [CityName], [AreaName], [MakeYear], [Price], [Kilometers], [FrontImagePath], [HostURL], [IsPremium], [ImageUrlMedium]);


GO
CREATE NONCLUSTERED INDEX [ix_Inquiryid]
    ON [dbo].[LiveListings]([Inquiryid] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_Livelistings_DealerId]
    ON [dbo].[LiveListings]([DealerId] ASC);


GO

----Modified by Manish on 24-04-2014 added with (nolock) keyword.
-- Added By : Sadhana Upadhyay on 03 Mar 2015 to update Elastic search index on updation of listing details	
--Modified by Sahil on 14th Oct, inserting into CustStockLog for individual cars,
CREATE TRIGGER [dbo].[TrigDeleteLiveListingCities] ON [dbo].[livelistings]
FOR DELETE
AS
DECLARE @CityId AS INT

BEGIN
	-- Added By : Sadhana Upadhyay on 16 Apr 2015
	DECLARE @ProfileIdList AS VARCHAR(MAX)

	IF NOT EXISTS (
			SELECT TOP 1 LL.CityId
			FROM LiveListings AS LL WITH (NOLOCK)
			WHERE LL.CityId = @CityId
			)
	BEGIN
		--delete this entry from the live listing cities
		DELETE
		FROM LL_Cities
		WHERE CityId = @CityId
	END

	IF EXISTS (SELECT TOP 1 ProfileId FROM deleted)
	BEGIN
		--SELECT @ProfileIdList = ISNULL(STUFF((SELECT ',' + ProfileId FROM deleted FOR XML PATH('')), 1, 1, ''), '')
		--EXEC APILivelistings @ProfileIdList, 'GET', 'DELETE'
		INSERT INTO ES_LiveListings(ProfileID,Action,ActionType,LastUpdateTime) SELECT ProfileId,'DELETE',3,GETDATE() FROM deleted

		DECLARE @SellerType SMALLINT, @InquiryId NUMERIC;

		SELECT @SellerType = SellerType, @InquiryId = InquiryId FROM deleted;
	
			--Modified by Afrose on 14th Oct, inserting into CustStockLog for individual cars,
		IF(@SellerType=2) --individual
				BEGIN
					INSERT INTO CustStockLog(CustSellInquiryId,EntryTime,ActionType,Action)
					VALUES (@InquiryId,GETDATE(),3,'DELETE')
				END
				--Modified by Afrose on 14th Oct, inserting into CustStockLog for individual cars,
	END
END

GO
--Modified by Afrose on 14th Oct, inserting into CustStockLog for individual cars, 
CREATE TRIGGER [dbo].[TrigUpdateLiveListingCities]
   ON [dbo].[livelistings]
   FOR INSERT,UPDATE
AS 
	DECLARE
		@CityId			AS NUMERIC, 
		@CityName		AS VARCHAR(100),
		@Lattitude		AS DECIMAL(18,4), 
		@Longitude		AS DECIMAL(18,4),
		@ProfileId      AS VARCHAR(50),
		@SellerType     AS TINYINT,
		@InquiryId		AS NUMERIC
BEGIN
	DECLARE @ProfileIdList VARCHAR(MAX)
	SET @CityId = -1	

	SELECT
		@CityId		= IsNull(I.CityId, -1),
		@CityName	= I.CityName,
		@Lattitude	= I.Lattitude,
		@Longitude	= I.Longitude,
		@SellerType = I.SellerType,
		@InquiryId  = I.Inquiryid
	FROM
		Inserted AS I
	IF @CityId <> -1
	BEGIN
		EXEC LL_UpdateCities @CityId, @CityName, @Lattitude, @Longitude
	END

	 --Added By : Sadhana Upadhyay on 26 Feb 2015 to update Elastic search index on updation of listing details
	 -- Modified By : Sadhana Upadhyay on 16 Apr 2015
	 --SELECT @ProfileIdList=ISNULL(STUFF ((select ','+ ProfileId from inserted FOR XML PATH('')),1,1,''),'') 
	
	IF EXISTS( SELECT TOP 1 ProfileId FROM deleted )
	BEGIN
		INSERT INTO ES_LiveListings(ProfileID,Action,ActionType,LastUpdateTime) SELECT ProfileId,'UPDATE',2,GETDATE() FROM inserted
		--EXEC APILivelistings @ProfileIdList, 'GET', 'UPDATE'
		--Modified by Afrose on 14th Oct, inserting into CustStockLog for individual cars,
			IF(@SellerType=2) --individual
			BEGIN
				INSERT INTO CustStockLog(CustSellInquiryId,EntryTime,ActionType,Action)
				VALUES (@InquiryId,GETDATE(),2,'UPDATE')
			END
			--Modified by Afrose on 14th Oct, inserting into CustStockLog for individual cars,
	END
	ELSE IF EXISTS( SELECT TOP 1 ProfileId FROM inserted )
		BEGIN
		INSERT INTO ES_LiveListings(ProfileID,Action,ActionType,LastUpdateTime) SELECT ProfileId,'CREATE',1,GETDATE() FROM inserted
			--EXEC APILivelistings @ProfileIdList, 'GET', 'CREATE'
			--Modified by Afrose on 14th Oct, inserting into CustStockLog for individual cars,
			IF(@SellerType=2) --individual
				BEGIN
					INSERT INTO CustStockLog(CustSellInquiryId,EntryTime,ActionType,Action)
					VALUES (@InquiryId,GETDATE(),1,'CREATE')
				END
				--Modified by Afrose on 14th Oct, inserting into CustStockLog for individual cars,
		END

	
END
