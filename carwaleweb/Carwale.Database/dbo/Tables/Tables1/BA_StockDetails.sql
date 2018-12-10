CREATE TABLE [dbo].[BA_StockDetails] (
    [ID]             BIGINT        IDENTITY (1, 1) NOT NULL,
    [BrokerId]       BIGINT        NOT NULL,
    [StockId]        INT           NOT NULL,
    [Kms]            FLOAT (53)    NULL,
    [Color]          VARCHAR (50)  NULL,
    [OwnerTypeId]    INT           NULL,
    [TransmissionId] TINYINT       NULL,
    [FuelTypeId]     INT           NULL,
    [Comments]       VARCHAR (500) NULL,
    [IsActive]       BIT           CONSTRAINT [DF_StockDetails_IsActive] DEFAULT ((1)) NOT NULL,
    [EntryDate]      DATETIME      NULL,
    [ModifyDate]     DATETIME      NULL,
    [DeletedDate]    DATETIME      NULL,
    [CarMakeId]      INT           NULL,
    [CarModelId]     INT           NULL,
    [CarVersionId]   INT           NULL,
    [MakeYear]       DATE          NULL,
    [Price]          VARCHAR (20)  NULL,
    CONSTRAINT [PK_StockDetails] PRIMARY KEY CLUSTERED ([ID] ASC),
    CONSTRAINT [StockIdUnique] UNIQUE NONCLUSTERED ([StockId] ASC)
);

