CREATE TABLE [dbo].[NewCarDealerCarMakes] (
    [ID]           NUMERIC (18) IDENTITY (1, 1) NOT FOR REPLICATION NOT NULL,
    [DealerId]     NUMERIC (18) NOT NULL,
    [CarMakeId]    NUMERIC (18) NOT NULL,
    [IsAuthorised] BIT          CONSTRAINT [DF_NewCarDealerCarMakes_IsAuthorised] DEFAULT (0) NOT NULL,
    [Status]       BIT          CONSTRAINT [DF_NewCarDealerCarMakes_Status] DEFAULT (0) NOT NULL,
    CONSTRAINT [PK_NewCarDealerCarMakes] PRIMARY KEY CLUSTERED ([ID] ASC) WITH (FILLFACTOR = 90),
    CONSTRAINT [FK_NewCarDealerCarMakes_CarMakes] FOREIGN KEY ([CarMakeId]) REFERENCES [dbo].[CarMakes] ([ID]),
    CONSTRAINT [FK_NewCarDealerCarMakes_Dealers] FOREIGN KEY ([DealerId]) REFERENCES [dbo].[Dealers] ([ID]),
    CONSTRAINT [IX_NewCarDealerCarMakes] UNIQUE NONCLUSTERED ([DealerId] ASC, [CarMakeId] ASC) WITH (FILLFACTOR = 90)
);

