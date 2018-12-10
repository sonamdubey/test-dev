CREATE TABLE [dbo].[OprDealerTicket] (
    [ID]         NUMERIC (18)  IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealersId]  NUMERIC (18)  NOT NULL,
    [Reason]     VARCHAR (500) NOT NULL,
    [StatusId]   NUMERIC (18)  NOT NULL,
    [EntryDate]  DATETIME      NOT NULL,
    [OpenedById] NUMERIC (18)  NOT NULL,
    CONSTRAINT [PK_OprDealerTicket] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_OprDealerTicket_Dealers] FOREIGN KEY ([DealersId]) REFERENCES [dbo].[Dealers] ([ID]),
    CONSTRAINT [FK_OprDealerTicket_OprCurrentDealerStatus] FOREIGN KEY ([StatusId]) REFERENCES [dbo].[OprCurrentDealerStatus] ([ID]),
    CONSTRAINT [FK_OprDealerTicket_OprUsers] FOREIGN KEY ([OpenedById]) REFERENCES [dbo].[OprUsers] ([Id])
);

