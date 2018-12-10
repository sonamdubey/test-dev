CREATE TABLE [dbo].[FOB_Stocks1] (
    [Id]            NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [VersionId]     NUMERIC (18) NULL,
    [ReferenceId]   VARCHAR (50) NULL,
    [IsAvailable]   BIT          NULL,
    [StockStatus]   SMALLINT     NULL,
    [AvailableDate] DATETIME     NULL,
    [Color]         NUMERIC (18) NULL,
    [DealerId]      NUMERIC (18) NULL,
    CONSTRAINT [PK_FOB_Stocks] PRIMARY KEY CLUSTERED ([Id] ASC) WITH (FILLFACTOR = 90)
);

