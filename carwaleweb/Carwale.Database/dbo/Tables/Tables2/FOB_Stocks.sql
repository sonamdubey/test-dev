CREATE TABLE [dbo].[FOB_Stocks] (
    [Id]         NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VCId]       NUMERIC (18) NULL,
    [StockCount] INT          NULL,
    [IsFactory]  BIT          CONSTRAINT [DF_FOB_Stock_IsFactory] DEFAULT ((1)) NULL,
    [DealerId]   NUMERIC (18) CONSTRAINT [DF_FOB_Stock_DealerId] DEFAULT ((-1)) NULL,
    [UpdatedOn]  DATETIME     NULL,
    CONSTRAINT [PK_FOB_Stock] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

