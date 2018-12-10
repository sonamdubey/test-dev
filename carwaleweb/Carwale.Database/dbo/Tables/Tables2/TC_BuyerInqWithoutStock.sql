CREATE TABLE [dbo].[TC_BuyerInqWithoutStock] (
    [TC_BuyerInqWithoutStockId] BIGINT        IDENTITY (1, 1) NOT NULL,
    [TC_InquiriesId]            BIGINT        NOT NULL,
    [MinPrice]                  BIGINT        NULL,
    [MaxPrice]                  BIGINT        NULL,
    [FromMakeYear]              SMALLINT      NULL,
    [ToMakeYear]                SMALLINT      NULL,
    [BodyType]                  VARCHAR (200) NULL,
    [FuelType]                  VARCHAR (20)  NULL,
    [ModelNames]                VARCHAR (400) NULL,
    [ModelIds]                  VARCHAR (400) NULL,
    [CreatedBy]                 INT           NULL,
    [CreatedDate]               DATETIME      NULL,
    [IsActive]                  BIT           NULL,
    [BodyTypeName]              VARCHAR (150) NULL,
    [FuelTypeName]              VARCHAR (100) NULL,
    CONSTRAINT [PK_TC_BuyerInqWithoutStock] PRIMARY KEY CLUSTERED ([TC_BuyerInqWithoutStockId] ASC)
);

