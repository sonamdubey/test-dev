CREATE TABLE [dbo].[CompetitorSellData] (
    [ID]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [CompetitorID] NUMERIC (18) NOT NULL,
    [EntryDate]    DATETIME     NOT NULL,
    [SellInquiry]  NUMERIC (18) CONSTRAINT [DF_CompetitorSellData_SellInquiry] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_CompetitorSellData] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_CompetitorSellData_CarwaleCompetitors] FOREIGN KEY ([CompetitorID]) REFERENCES [dbo].[CarwaleCompetitors] ([ID])
);


GO
CREATE UNIQUE NONCLUSTERED INDEX [IX_CompetitorSellData]
    ON [dbo].[CompetitorSellData]([CompetitorID] ASC, [EntryDate] ASC) WITH (FILLFACTOR = 90);

