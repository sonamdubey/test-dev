CREATE TABLE [dbo].[DealerOffers] (
    [ID]                 NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CityId]             NUMERIC (18)  NULL,
    [DealerId]           NUMERIC (18)  NULL,
    [OfferTitle]         VARCHAR (200) NULL,
    [OfferDescription]   VARCHAR (MAX) NULL,
    [MaxOfferValue]      NUMERIC (18)  NULL,
    [Conditions]         VARCHAR (MAX) NULL,
    [StartDate]          DATETIME      NULL,
    [EndDate]            DATETIME      NULL,
    [EntryDate]          DATETIME      NULL,
    [EnteredBy]          NUMERIC (18)  NULL,
    [SourceCategory]     NUMERIC (18)  NULL,
    [SourceDescription]  VARCHAR (MAX) NULL,
    [IsActive]           BIT           CONSTRAINT [DF_DealerOffers_IsActive] DEFAULT ((0)) NULL,
    [IsApproved]         BIT           CONSTRAINT [DF_DealerOffers_IsApproved] DEFAULT ((0)) NOT NULL,
    [IsCountryWide]      BIT           CONSTRAINT [DF_DealerOffers_IsCountryWide] DEFAULT ((0)) NOT NULL,
    [OfferType]          SMALLINT      NULL,
    [OfferUnits]         INT           NULL,
    [ClaimedUnits]       INT           CONSTRAINT [DF_DealerOffers_ClaimedUnits] DEFAULT ((0)) NULL,
    [UpdatedOn]          DATETIME      NULL,
    [UpdatedBy]          INT           NULL,
    [HostURL]            VARCHAR (250) NULL,
    [ImageName]          VARCHAR (150) NULL,
    [ImagePath]          VARCHAR (150) NULL,
    [PreBookingEmailIds] VARCHAR (200) NULL,
    [PreBookingMobile]   VARCHAR (200) NULL,
    [CouponEmailIds]     VARCHAR (200) NULL,
    [CouponMobile]       VARCHAR (200) NULL,
    [DispOnDesk]         BIT           CONSTRAINT [DF_DealerOffers_DispOnDesk] DEFAULT ((0)) NULL,
    [DispOnMobile]       BIT           CONSTRAINT [DF_DealerOffers_DispOnMobile] DEFAULT ((0)) NULL,
    [OriginalImgPath]    VARCHAR (250) NULL,
    [OriginalImg]        VARCHAR (250) NULL,
    [DispSnippetOnDesk]  BIT           NULL,
    [DispSnippetOnMob]   BIT           NULL,
    [DispOnOffersPgDesk] BIT           NULL,
    [DispOnOffersPgMob]  BIT           NULL,
    [ShortDescription]   VARCHAR (MAX) NULL,
    CONSTRAINT [PK_DealerOffers] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);


GO
CREATE NONCLUSTERED INDEX [IX_DealerOffers_StartDate]
    ON [dbo].[DealerOffers]([StartDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DealerOffers_EndDate]
    ON [dbo].[DealerOffers]([EndDate] ASC);


GO
CREATE NONCLUSTERED INDEX [IX_DealerOffers_IsActive]
    ON [dbo].[DealerOffers]([IsActive] ASC, [IsApproved] ASC, [StartDate] ASC, [EndDate] ASC, [OfferType] ASC);


GO
-- =============================================
-- Author:		Vaibhav K
-- Create date: 30 Nov 2014
-- Description:	Trigger on DealerOffers Table to insert all models in ModelOffers when offer is actiaved and remove model if offer ends
-- =============================================
CREATE TRIGGER [dbo].[Trig_UpdateModelOffers_DealerOffers]
   ON  [dbo].[DealerOffers]
   AFTER INSERT,UPDATE
AS 
BEGIN
	-- SET NOCOUNT ON added to prevent extra result sets from
	-- interfering with SELECT statements.
	SET NOCOUNT ON;

    -- Insert statements for trigger here
	DECLARE @OfferId INT, @IsActive BIT, @IsApproved BIT, @ClaimedUnits INT, @OfferUnits INT, @StartDate DATETIME, @EndDate DATETIME


	SELECT @OfferId = I.ID, @IsActive = I.IsActive, @IsApproved = I.IsApproved, @ClaimedUnits = I.ClaimedUnits, @OfferUnits = I.OfferUnits,
	@StartDate = I.StartDate , @EndDate = I.EndDate
	FROM Inserted I WHERE I.OfferType <> 2

	DECLARE @TempModelTbl TABLE (ModelId INT)
	
	--Get all models of that offer if end date is not reached and insert in Temp table
	INSERT INTO @TempModelTbl
	SELECT DISTINCT ModelId
	FROM DealerOffersVersion DOV WITH (NOLOCK)
	JOIN DealerOffers DO WITH (NOLOCK) ON DOV.OfferId = DO.ID
	WHERE DOV.OfferId = @OfferId AND DOV.ModelId <> -1 --AND CAST(DO.StartDate AS DATE) <= CAST(GETDATE() AS DATE) AND CAST(DO.EndDate AS DATE) >= CAST(GETDATE() AS DATE) 
	UNION
	SELECT DISTINCT CMO.Id ModelId
	FROM DealerOffersVersion DOV WITH (NOLOCK)
	JOIN DealerOffers DO WITH (NOLOCK) ON DOV.OfferId = DO.ID
	JOIN CarMakes CMK WITH (NOLOCK) ON DOV.MakeId = CMK.ID
	JOIN CarModels CMO WITH (NOLOCK) ON CMK.ID = CMO.CarMakeId AND CMO.New = 1 AND CMO.Futuristic = 0 AND CMO.IsDeleted = 0
	WHERE DOV.OfferId = @OfferId AND DOV.ModelId = -1 --AND CAST(DO.StartDate AS DATE) <= CAST(GETDATE() AS DATE) AND CAST(DO.EndDate AS DATE) >= CAST(GETDATE() AS DATE)

	IF @IsActive = 1 AND @IsApproved = 1 AND CAST(@StartDate AS DATE) <= CAST(GETDATE() AS DATE) AND CAST(@EndDate AS DATE) >= CAST(GETDATE() AS DATE)
	BEGIN		
		--Delete previous models from ModelOffers 
		DELETE FROM ModelOffers WHERE ModelId IN 
		(
			SELECT ModelId FROM @TempModelTbl
		)

		--Insert fresh models in ModelOffers
		INSERT INTO ModelOffers
		SELECT ModelId FROM @TempModelTbl
	END
	ELSE IF @IsActive = 0 OR @ClaimedUnits >= @OfferUnits OR CAST(@StartDate AS DATE) > CAST(GETDATE() AS DATE) OR CAST(@EndDate AS DATE) < CAST(GETDATE() AS DATE)
	BEGIN
		DELETE FROM ModelOffers WHERE ModelId IN
		(
			SELECT TmpTbl.ModelId 
			FROM @TempModelTbl TmpTbl
			LEFT JOIN
				(SELECT DISTINCT ModelId 
				FROM DealerOffersVersion DOV WITH (NOLOCK)
				JOIN DealerOffers DO WITH (NOLOCK) ON DOV.OfferId = DO.ID
				WHERE DOV.OfferId <> @OfferId AND DOV.ModelId <> -1
				AND DO.IsActive = 1 AND DO.IsApproved = 1 AND DO.ClaimedUnits < DO.OfferUnits AND DO.OfferType <> 2
				AND CAST(Do.StartDate AS DATE) <= CAST(GETDATE() AS DATE) AND CAST(Do.EndDate AS DATE) >= CAST(GETDATE() AS DATE)				
				UNION
				SELECT DISTINCT CMO.ID ModelId
				FROM DealerOffersVersion DOV WITH (NOLOCK)
				JOIN DealerOffers DO WITH (NOLOCK) ON DOV.OfferId = DO.ID
				JOIN CarMakes CMK WITH (NOLOCK) ON DOV.MakeId = CMK.ID
				JOIN CarModels CMO WITH (NOLOCK) ON CMK.ID = CMO.CarMakeId AND CMO.New = 1 AND CMO.Futuristic = 0 AND CMO.IsDeleted = 0
				WHERE DOV.OfferId <> @OfferId AND DOV.ModelId = -1
				AND DO.IsActive = 1 AND DO.IsApproved = 1 AND DO.ClaimedUnits < DO.OfferUnits AND DO.OfferType <> 2
				AND CAST(Do.StartDate AS DATE) <= CAST(GETDATE() AS DATE) AND CAST(Do.EndDate AS DATE) >= CAST(GETDATE() AS DATE)
				) 
			TBL 
			ON TBL.ModelId = TmpTbl.ModelId
			WHERE TBL.ModelId IS NULL
		)
	END
END

GO
DISABLE TRIGGER [dbo].[Trig_UpdateModelOffers_DealerOffers]
    ON [dbo].[DealerOffers];

