CREATE TABLE [dbo].[DealerAssociates] (
    [DealerID]    NUMERIC (18) NOT NULL,
    [AssociateId] NUMERIC (18) NOT NULL,
    CONSTRAINT [PK_DealerAssociates] PRIMARY KEY CLUSTERED ([DealerID] ASC, [AssociateId] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_DealerAssociates_Dealers] FOREIGN KEY ([DealerID]) REFERENCES [dbo].[Dealers] ([ID]) ON UPDATE CASCADE,
    CONSTRAINT [FK_DealerAssociates_Dealers1] FOREIGN KEY ([AssociateId]) REFERENCES [dbo].[Dealers] ([ID])
);

