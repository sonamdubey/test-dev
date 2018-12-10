CREATE TABLE [dbo].[NewCarShowroomPrices] (
    [Id]              NUMERIC (18) IDENTITY (1, 1) NOT NULL,
    [CarVersionId]    NUMERIC (18) NOT NULL,
    [CityId]          NUMERIC (18) NOT NULL,
    [Price]           NUMERIC (18) NOT NULL,
    [RTO]             NUMERIC (18) NULL,
    [Insurance]       NUMERIC (18) NULL,
    [CorporateRTO]    NUMERIC (18) NULL,
    [MetPrice]        NUMERIC (18) NULL,
    [MetRTO]          NUMERIC (18) NULL,
    [MetInsurance]    NUMERIC (18) NULL,
    [MetCorporateRTO] NUMERIC (18) NULL,
    [LastUpdated]     DATETIME     NOT NULL,
    [IsActive]        BIT          CONSTRAINT [DF_NewCarShowroomPrices_IsActive_1] DEFAULT ((1)) NOT NULL,
    [CarModelId]      INT          NULL,
    [isMetallic]      BIT          DEFAULT ((0)) NOT NULL,
    CONSTRAINT [PK_NewCarShowroomPrices_1] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [Idx_NewCarShowroomPrices_IA_CVI]
    ON [dbo].[NewCarShowroomPrices]([IsActive] ASC)
    INCLUDE([CarVersionId]);


GO
CREATE NONCLUSTERED INDEX [ix_NewCarShowroomPrices__CarVersionId__CityId__IsActive]
    ON [dbo].[NewCarShowroomPrices]([CarVersionId] ASC, [CityId] ASC, [IsActive] ASC)
    INCLUDE([Price], [RTO], [Insurance]);


GO
CREATE NONCLUSTERED INDEX [ix_NewCarShowroomPrices__CarVersionId__IsActive]
    ON [dbo].[NewCarShowroomPrices]([CarVersionId] ASC, [IsActive] ASC)
    INCLUDE([CityId]);


GO
CREATE NONCLUSTERED INDEX [ix_NewCarShowroomPrices__CityId__IsActive__Price]
    ON [dbo].[NewCarShowroomPrices]([CityId] ASC, [IsActive] ASC, [Price] ASC)
    INCLUDE([CarVersionId]);

