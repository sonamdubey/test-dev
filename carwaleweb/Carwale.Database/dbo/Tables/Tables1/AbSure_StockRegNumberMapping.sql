CREATE TABLE [dbo].[AbSure_StockRegNumberMapping] (
    [AbSure_StockRegNumberMappingId] BIGINT       IDENTITY (1, 1) NOT NULL,
    [TC_StockId]                     BIGINT       NULL,
    [RegistrationNumber]             VARCHAR (50) NULL,
    [EntryDate]                      DATETIME     CONSTRAINT [DF_AbSure_StockRegNumberMapping] DEFAULT (getdate()) NULL,
    [IsActive]                       BIT          DEFAULT ((1)) NULL,
    CONSTRAINT [PK_AbSure_StockRegNumberMapping] PRIMARY KEY CLUSTERED ([AbSure_StockRegNumberMappingId] ASC)
);

