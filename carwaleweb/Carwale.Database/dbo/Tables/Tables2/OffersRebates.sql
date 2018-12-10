CREATE TABLE [dbo].[OffersRebates] (
    [ID]                NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealerId]          NUMERIC (18)  NOT NULL,
    [CategoryId]        NUMERIC (18)  NOT NULL,
    [Title]             VARCHAR (200) NULL,
    [Priority]          SMALLINT      NULL,
    [EntryDate]         DATETIME      NOT NULL,
    [ExpiryDate]        DATETIME      NOT NULL,
    [RebateDescription] VARCHAR (MAX) NULL,
    CONSTRAINT [PK_OffersRebates] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90)
);

